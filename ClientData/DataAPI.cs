using ClientData.Abstract;
using ClinetAPI;

namespace ClientData
{
    internal class DataAPI : AbstractDataAPI
    {
        private IUser _user = null;

        public override IUser User => _user;

        private readonly IBookRepository _bookRepository = new BookRepository();

        public override IBookRepository BookRepository => _bookRepository;

        private int _nextBookId = 0;

        private readonly IConnectionService _connectionService;

        public DataAPI(string userName)
        {
            //_user = new User(userName, initialBalance);
            _bookRepository = new BookRepository();
            _connectionService = new ConnectionService();
            _connectionService.OnMessage += OnMessage;
        }

        public override int CountBooks()
        {
            return _bookRepository.CountBooks();
        }

        private void OnMessage(string message)
        {
            Serializer serializer = Serializer.Create();

            if(serializer.GetCommandHeader(message) == UserChangedResponse.StaticHeader)
            {
                UserChangedResponse response = serializer.Deserialize<UserChangedResponse>(message);
                if(_user == null || _user.Name == response.User.Username)
                {
                    _user = response.User.ToUser();
                }
            }
            else if(serializer.GetCommandHeader(message) == BookChangedResponse.StaticHeader)
            {

            } 
            else if(serializer.GetCommandHeader(message) == AllBooksUpdateResponse.StaticHeader)
            {

            } 
            else if(serializer.GetCommandHeader(message) == TransactionResultResponse.StaticHeader)
            {

            }
        }

        private void GetUser(string username)
        {
            Serializer serializer = Serializer.Create();
            _connectionService.SendAsync(serializer.Serialize(new GetUserCommand(username)));
        }
    }
}
