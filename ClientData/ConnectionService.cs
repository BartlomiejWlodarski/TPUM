﻿using ClientData.Abstract;
using ConnectionAPI;
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

        internal WebSocketConnection? WebSocketConnection { get; private set; }
        public async Task Connect(Uri peerUri)
        {
            try
            {
                Logger?.Invoke($"Connecting to {peerUri}");
                WebSocketConnection = await WebSocketClient.Connect(peerUri, Logger);
                OnConnectStateChanged?.Invoke();
                WebSocketConnection.OnMessage = (message,guid) => OnMessage?.Invoke(message);
                WebSocketConnection.OnError = (guid) => OnError?.Invoke();
                WebSocketConnection.OnClose = (guid) => OnDisconnect?.Invoke();
            }
            catch (WebSocketException exception)
            {
                Logger?.Invoke($"WebSocked exception: {exception.Message}");
                OnError?.Invoke();
            }
        }

        public async Task Disconnect()
        {
            if (WebSocketConnection != null)
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
            if (WebSocketConnection != null)
            {
                await WebSocketConnection.SendAsync(message);
            }
        }
    }
}
