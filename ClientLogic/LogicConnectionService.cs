using ClientData.Abstract;
using ClientLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic
{
    internal class LogicConnectionService : ILogicConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;

        private readonly IConnectionService _connectionService;

        public LogicConnectionService(IConnectionService connectionService)
        {
            _connectionService = connectionService;

            _connectionService.Logger += (message) => Logger?.Invoke(message);
            _connectionService.OnConnectStateChanged += () => OnConnectStateChanged?.Invoke();
            _connectionService.OnMessage += (message) => OnMessage?.Invoke(message);
            _connectionService.OnError += () => OnError?.Invoke();

        }

        public async Task Connect(Uri uri)
        {
            await _connectionService.Connect(uri);
        }

        public async Task Disconnect()
        {
            await _connectionService.Disconnect();
        }

        public bool IsConnected()
        {
            return _connectionService.IsConnected();
        }
    }
}
