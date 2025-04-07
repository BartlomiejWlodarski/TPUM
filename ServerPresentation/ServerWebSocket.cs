using ClinetAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPresentation
{
    internal static class ServerWebSocket
    {
        public static async Task StartServer(int port, Action<WebSocketConnection> onConnetion)
        {
            Uri uri = new Uri($@"http://localhost:{port}/");
        }
    }
}
