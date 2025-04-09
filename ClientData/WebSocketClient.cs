using ClinetAPI;
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
        public static async Task<WebSocketConnection> Connect(Uri uri, Action<string>? logger)
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(uri, CancellationToken.None);
            switch (clientWebSocket.State)
            {
                case WebSocketState.Open:
                    logger?.Invoke("Opening socket connection to server" + uri);
                    WebSocketConnection connection = new ClientWebSocketConnection(clientWebSocket,logger,uri);
                    return connection;
                default:
                    logger?.Invoke("Connecting to server failed. " + clientWebSocket.State);
                    throw new WebSocketException("Connecting to server failed." + clientWebSocket.State);
            }
        }

        private class ClientWebSocketConnection : WebSocketConnection
        {
            private readonly ClientWebSocket clientWebSocket;
            private readonly Action<string> log;
            private readonly Uri uri;

            public ClientWebSocketConnection(ClientWebSocket clientWebSocket, Action<string> log, Uri uri)
            {
                this.clientWebSocket = clientWebSocket;
                this.log = log;
                this.uri = uri;
                Task.Factory.StartNew(ClientMessageLoop);
            }

            public override Task DisconnectAsync()
            {
                return clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,"Shutting down the connection",CancellationToken.None);
            }

            protected override Task SendTask(string message)
            {
                return clientWebSocket.SendAsync(message.ToArraySegment(), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            public override string ToString()
            {
                return uri.ToString();
            }

            private void ClientMessageLoop()
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                        WebSocketReceiveResult result = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                        if(result.MessageType == WebSocketMessageType.Close)
                        {
                            OnClose?.Invoke();
                            clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing web socket", CancellationToken.None).Wait();
                            return;
                        }

                        int count = result.Count;
                        while (!result.EndOfMessage)
                        {
                            if(count >= buffer.Length)
                            {
                                OnClose?.Invoke();
                                clientWebSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Closing web socket. Buffer size exceeded!", CancellationToken.None).Wait();
                                return;
                            }

                            segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                            result = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                            count += result.Count;
                        }
                        string message = Encoding.UTF8.GetString(buffer, 0, count);
                        OnMessage?.Invoke(message);
                    }
                }
                catch (Exception ex)
                {
                    log("Connection interrupted by exception: " + ex);
                    Debug.WriteLine(ex);
                    clientWebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Connection interrupted by exception", CancellationToken.None).Wait();
                }
            }

        }
    }
}
