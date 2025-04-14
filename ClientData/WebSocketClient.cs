using ConnectionAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientData
{
    internal class WebSocketClient
    {
        public static async Task<WebSocketConnection> Connect(Uri peer, Action<string>? logger)
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(peer, CancellationToken.None);
            switch (clientWebSocket.State)
            {
                case WebSocketState.Open:
                    logger?.Invoke($"Opening WebSocket connection to remote server {peer}");
                    WebSocketConnection socket = new ClientWebSocketConnection(clientWebSocket, peer, logger);
                    return socket;
                default:
                    logger?.Invoke($"Cannot connect to remote node status {clientWebSocket.State}");
                    throw new WebSocketException($"Cannot connect to remote node status {clientWebSocket.State}");
            }
        }

        private class ClientWebSocketConnection : WebSocketConnection
        {
            private readonly ClientWebSocket clientWebSocket;
            private readonly Action<string> log;
            private readonly Uri peer;

            public ClientWebSocketConnection(ClientWebSocket clientWebSocket, Uri peer, Action<string> log)
            {
                this.clientWebSocket = clientWebSocket;
                this.peer = peer;
                this.log = log;
                _ = Task.Run(ClientMessageLoop);
            }

            protected override async Task SendTask(string message)
            {
                if (clientWebSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        await clientWebSocket.SendAsync(
                            message.ToArraySegment(),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        log($"[SendTask] Error while sending: {ex}");
                        OnError?.Invoke();
                    }
                }
                else
                {
                    log($"[SendTask] WebSocket is not open. Current state: {clientWebSocket.State}");
                    OnError?.Invoke();
                }
            }

            public override async Task DisconnectAsync()
            {
                if (clientWebSocket.State == WebSocketState.Open || clientWebSocket.State == WebSocketState.CloseReceived)
                {
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown procedure started", CancellationToken.None);
                }
                clientWebSocket.Dispose();
                OnClose?.Invoke();
            }

            public override string ToString() => peer.ToString();

            private async Task ClientMessageLoop()
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    while (clientWebSocket.State == WebSocketState.Open)
                    {
                        var segment = new ArraySegment<byte>(buffer);
                        var result = await clientWebSocket.ReceiveAsync(segment, CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                            OnClose?.Invoke();
                            return;
                        }

                        int count = result.Count;
                        while (!result.EndOfMessage)
                        {
                            if (count >= buffer.Length)
                            {
                                await clientWebSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Too long message", CancellationToken.None);
                                OnClose?.Invoke();
                                return;
                            }

                            segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                            result = await clientWebSocket.ReceiveAsync(segment, CancellationToken.None);
                            count += result.Count;
                        }

                        string message = Encoding.UTF8.GetString(buffer, 0, count);
                        OnMessage?.Invoke(message);
                    }
                }
                catch (Exception ex)
                {
                    log($"[ClientMessageLoop] Exception: {ex}");
                    OnError?.Invoke();
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Exception occurred", CancellationToken.None);
                }
            }
        }

    }
}
