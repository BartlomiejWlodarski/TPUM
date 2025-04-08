using ClientLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Presentation.Model
{
    internal class ModelConnectionService : IModelConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectionStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;

        private readonly ILogicConnectionService connectionService;

        public ModelConnectionService(ILogicConnectionService connectionService)
        {
            this.connectionService = connectionService;

            this.connectionService.Logger += (message) => Logger?.Invoke(message);
            this.connectionService.OnConnectStateChanged += () => OnConnectionStateChanged?.Invoke();
            this.connectionService.OnMessage += (message) => OnMessage?.Invoke(message);
            this.connectionService.OnError += () => OnError?.Invoke();
        }


        public async Task Connect(Uri uri)
        {
            await connectionService.Connect(uri);
        }

        public async Task Disconnect()
        {
            await connectionService.Disconnect();
        }

        public bool IsConnected()
        {
            return connectionService.IsConnected();
        }
    }
}
