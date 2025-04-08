using ClinetAPI;
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
                Task.Factory.StartNew(() => ServerMessageLoop(socket));
            }

            private void ServerMessageLoop(WebSocket webSocket)
            {
                byte[] socketBuffer = new byte[1024];

                while (true)
                {
                    ArraySegment<byte> segments = new ArraySegment<byte>(socketBuffer);
                    WebSocketReceiveResult receiveResult = webSocket.ReceiveAsync(segments,CancellationToken.None).Result;
                    if ((receiveResult.MessageType == WebSocketMessageType.Close))
                    {
                        OnClose?.Invoke();
                        webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutting down the socket",CancellationToken.None);
                        return;
                    }

                    int count = receiveResult.Count;

                    while (!receiveResult.EndOfMessage)
                    {
                        if( count >= socketBuffer.Length)
                        {
                            OnClose?.Invoke();
                            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Received message exceeds the limit of 1024 bytes", CancellationToken.None);
                            return;
                        }

                        segments = new ArraySegment<byte>(socketBuffer, count, socketBuffer.Length - count);
                        receiveResult = webSocket.ReceiveAsync(segments, CancellationToken.None).Result;
                        count += receiveResult.Count;
                    }
                    string message = Encoding.UTF8.GetString(socketBuffer,0,count);
                    OnMessage?.Invoke(message);
                }
            }

            public override string ToString()
            {
                return _endpoint.ToString();
            }

            public override Task DisconnectAsync()
            {
                return socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutting down started", CancellationToken.None);
            }

            protected override Task SendTask(string message)
            {
                return socket.SendAsync(message.ToArraySegment(), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
