using ClientData.Abstract;
using ClinetAPI;
using System.Diagnostics;

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

        public event EventHandler<UserChangedEventArgs>? UserChanged;
        public event Action<int>? TransactionResult;

        public DataAPI(IConnectionService connectionService)
        {
            //_user = new User(userName, initialBalance);
            _bookRepository = new BookRepository();
            _connectionService = connectionService;
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
                    UserChanged?.Invoke(this, new UserChangedEventArgs(_user));
                }
            }
            else if(serializer.GetCommandHeader(message) == BookChangedResponse.StaticHeader)
            {
                BookChangedResponse response = serializer.Deserialize<BookChangedResponse>(message);
                switch (response.changeType)
                {
                    case 0:
                        _bookRepository.AddBook(response.book.ToBook());
                        break;
                    case 1:
                        _bookRepository.RemoveBook(response.book.Id);
                        break;
                    case 2:
                        _bookRepository.ChangeBook(response.book.ToBook());
                        break;
                    default:
                        break;
                }
            } 
            else if(serializer.GetCommandHeader(message) == AllBooksUpdateResponse.StaticHeader)
            {
                AllBooksUpdateResponse response = serializer.Deserialize<AllBooksUpdateResponse>(message);
                if(response.Books == null)return;
                _bookRepository.LoadAllBooks(response.Books.Select(x => x.ToBook()));
            } 
            else if(serializer.GetCommandHeader(message) == TransactionResultResponse.StaticHeader)
            {
                TransactionResultResponse response = serializer.Deserialize<TransactionResultResponse>(message);
                TransactionResult?.Invoke(response.ResultCode);
            }
        }

        private async Task RequestGetUser(string username)
        {
            Serializer serializer = Serializer.Create();
            await _connectionService.SendAsync(serializer.Serialize(new GetUserCommand(username)));
        }

        public override IConnectionService GetConnectionService()
        {
            return _connectionService;
        }

        public async Task RequestBooksData()
        {
            Serializer serializer = Serializer.Create();
            string temp = serializer.Serialize(new GetBooksCommand());
            await _connectionService.SendAsync(temp);
        }

        public override void RequestBooks()
        {
            if (_connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await RequestBooksData());
            }
        }

        public async Task RequestBuyBook(int bookID)
        {
            Serializer serializer = Serializer.Create();
            await _connectionService.SendAsync(serializer.Serialize(new SellBookCommand(bookID,_user.Name)));
        }

        public override void BuyBook(int bookID)
        {
            if (_connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await RequestBuyBook(bookID));
            }
        }

        public override void GetUser(string username)
        {
            if (_connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await RequestGetUser(username));
            }
        }
    }
}
