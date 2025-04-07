using ClientData.Abstract;
using ClinetAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientData
{
    internal class ConnectionService : IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        internal WebSocketConnection? WebSocketConnection {  get; private set; }
        public async Task Connect(Uri uri)
        {
            try
            {
                Logger?.Invoke("Connecting to " + uri);
                WebSocketConnection = await WebSocketClient.Connect(uri,Logger);
                OnConnectStateChanged?.Invoke();
                WebSocketConnection.OnMessage = (message) => OnMessage?.Invoke(message);
                WebSocketConnection.OnClose = () => OnDisconnect?.Invoke();
                WebSocketConnection.OnError = () => OnError?.Invoke();
            }
            catch(WebSocketException ex) 
            {
                Logger?.Invoke("Web socket exception occured: " + ex);
                OnError?.Invoke();
            }
        }

        public async Task Disconnect()
        {
            if(WebSocketConnection != null)
            {
                await WebSocketConnection.DisconnectAsync();
            }
        }

        public bool IsConnected()
        {
            return WebSocketConnection != null;
        }

        public async Task SendAsync(string message)
        {
            if(WebSocketConnection != null)
            {
                await WebSocketConnection.SendAsync(message);
            }
        }
    }
}
