using ClientData.Abstract;
using ConnectionAPI;
using System.Diagnostics;

namespace ClientData
{
    internal class DataAPI : AbstractDataAPI
    {
        private readonly IUserContainer _userContainer;

        private readonly IBookRepository _bookRepository;

        private readonly IConnectionService _connectionService;
        public DataAPI(IConnectionService connectionService)
        {
            _connectionService = connectionService ?? new ConnectionService();
            _bookRepository = new BookRepository(_connectionService);
            _userContainer = new UserContainer();
            _connectionService.OnMessage += OnMessage;
        }

        public override IConnectionService GetConnectionService()
        {
            return _connectionService;
        }

        public override IBookRepository GetBookRepository()
        {
            return _bookRepository;
        }

        public override IUserContainer GetUserContainer()
        {
            return _userContainer;
        }
        private void OnMessage(string message)
        {
            Serializer serializer = Serializer.Create();

            if(serializer.GetCommandHeader(message) == ServerStatics.UserChangedResponseHeader)
            {
                UserChangedResponse response = serializer.Deserialize<UserChangedResponse>(message);
                if(_userContainer.user == null || _userContainer.user.Name == response.User.Username)
                {
                    _userContainer.ChangeUser(response.User.ToUser());
                }
            } 
        }

        public override void GetUser(string username)
        {
            if (_connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await RequestGetUser(username));
            }
        }

        private async Task RequestGetUser(string username)
        {
            Serializer serializer = Serializer.Create();
            await _connectionService.SendAsync(serializer.Serialize(new GetUserCommand
            {
                Header = ServerStatics.GetUserCommandHeader,
                Username = username
            }
            ));
        }

        private async Task Subscibe(bool value)
        {
            Serializer serializer = Serializer.Create();
            await _connectionService.SendAsync(serializer.Serialize(new SubscribeToNewsletterUpdatesCommand
            {
                Header = ServerStatics.SubscribeToNewsletterUpdatesHeader,
                Subscribed = value
            }
            ));
        }

        public override void SubscibeToNewsLetterUpdates(bool value)
        {
            if (_connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await Subscibe(value));
            }
        }
    }
}
