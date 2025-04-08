namespace ClientLogic.Abstract
{
    public interface ILogicConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;

        public event Action<string>? OnMessage;
        public event Action? OnError;

        public Task Connect(Uri uri);
        public Task Disconnect();

        public bool IsConnected();
    }
}
