using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData.Abstract
{
    public interface IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public Task Connect(Uri uri);
        public Task Disconnect();

        public bool IsConnected();
        public Task SendAsync(string message);
    }
}
