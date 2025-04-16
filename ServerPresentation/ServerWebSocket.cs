using ConnectionAPI;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace ServerPresentation
{
    internal static class ServerWebSocket
    {
        public static async Task StartServer(int port, Action<WebSocketConnection> onConnetion)
        {
            Uri uri = new Uri($@"http://localhost:{port}/");
            await ServerLoop(uri, onConnetion);
        }

        private static async Task ServerLoop(Uri uri, Action<WebSocketConnection> onConnection)
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add(uri.ToString());
            server.Start();
            while (true)
            {
                HttpListenerContext context = await server.GetContextAsync();
                if (!context.Request.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
                HttpListenerWebSocketContext webContext = await context.AcceptWebSocketAsync(null);
                WebSocketConnection webConnection = new ServerWebSockerConnection(webContext.WebSocket, context.Request.RemoteEndPoint);
                onConnection?.Invoke(webConnection);
            }
        }

        private class ServerWebSockerConnection : WebSocketConnection
        {
            private readonly IPEndPoint _endpoint;
            private readonly WebSocket socket;

            public ServerWebSockerConnection(WebSocket socket, IPEndPoint endpoint)
            {
                this.socket = socket;
                this._endpoint = endpoint;
                this.connectionID = Guid.NewGuid();
                _ = Task.Run(() => ServerMessageLoop(socket));
            }

            protected override async Task SendTask(string message)
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(
                            message.ToArraySegment(),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SendTask] Server send error: {ex}");
                        OnError?.Invoke(connectionID);
                    }
                }
                else
                {
                    Console.WriteLine($"[SendTask] Cannot send — socket state: {socket.State}");
                    OnError?.Invoke(connectionID);
                }
            }

            public override async Task DisconnectAsync()
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutting down", CancellationToken.None);
                }
                OnClose?.Invoke(connectionID);
            }

            public override string ToString() => _endpoint.ToString();

            private async Task ServerMessageLoop(WebSocket webSocket)
            {
                byte[] buffer = new byte[1024];
                try
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var segment = new ArraySegment<byte>(buffer);
                        var result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closing", CancellationToken.None);
                            OnClose?.Invoke(connectionID);
                            return;
                        }

                        int count = result.Count;
                        while (!result.EndOfMessage)
                        {
                            if (count >= buffer.Length)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Too long message", CancellationToken.None);
                                OnClose?.Invoke(connectionID);
                                return;
                            }

                            segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                            result = await webSocket.ReceiveAsync(segment, CancellationToken.None);
                            count += result.Count;
                        }

                        string message = Encoding.UTF8.GetString(buffer, 0, count);
                        OnMessage?.Invoke(message, connectionID);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ServerMessageLoop] Exception: {ex}");
                    await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Exception occurred", CancellationToken.None);
                    OnError?.Invoke(connectionID);
                }
            }
        }

    }
}
