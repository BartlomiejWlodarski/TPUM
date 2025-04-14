using ClientData.Abstract;
using ConnectionAPI;
using System.Net.WebSockets;

namespace TPUMProject.ClientLogicTests
{
    internal class FakeDataAPI : AbstractDataAPI
    {
        private readonly IUserContainer _userContainer;

        private readonly IBookRepository _bookRepository;

        private readonly IConnectionService _connectionService;
        public FakeDataAPI()
        {
            _connectionService = new FakeConnectionService();
            _bookRepository = new FakeBookRepository();
            _userContainer = new FakeUserContainer();
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
            throw new NotImplementedException();
        }

        public override void GetUser(string username)
        {
            if (_connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await RequestGetUser(username));
            }
        }

        private Task RequestGetUser(string username)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeBookRepository : IBookRepository
    {
        private List<IBook> books = new();
        private object bookLock = new object();

        private HashSet<IObserver<BookRepositoryChangedEventArgs>> observers;

        public event Action? AllBooksUpdated;
        public event Action<int>? TransactionResult;
        public event EventHandler<BookRepositoryChangedEventArgs>? BookRepositoryChangedHandler;

        IConnectionService connectionService;

        public FakeBookRepository()
        {
            books = new List<IBook>
            {
                new FakeBook("Adam Mickiewicz", "Pan Tadeusz", 20, false),
                new FakeBook("Henryk Sienkiewicz", "Quo vadis", 40, false)
            };
        }

        ~FakeBookRepository()
        {
            List<IObserver<BookRepositoryChangedEventArgs>> cachedObservers = observers.ToList();
            foreach (IObserver<BookRepositoryChangedEventArgs>? observer in cachedObservers)
            {
                observer?.OnCompleted();
            }
        }

        private void OnMessage(string message)
        {
            throw new NotImplementedException();
        }

        public Task RequestBooks()
        {
            throw new NotImplementedException();
        }

        public Task SellBook(int id, string username)
        {
            throw new NotImplementedException();
        }

        public void RequestUpdate()
        {
            if (connectionService.IsConnected())
            {
                Task task = Task.Run(async () => await RequestBooks());
            }
        }


        public List<IBook> GetAllBooks()
        {
            return books;
        }

        public void ReplaceBook(IBook replacement)
        {
            lock (bookLock)
            {
                int index = books.FindIndex(book => book.Id == replacement.Id);
                if (index > -1 && index < books.Count)
                {
                    books[index] = replacement;
                }
            }

            foreach (IObserver<BookRepositoryChangedEventArgs>? observer in observers)
            {
                observer.OnNext(new BookRepositoryChangedEventArgs(replacement, BookRepositoryChangedEventType.Modified));
            }
        }

        public void AddBook(IBook book)
        {
            lock (bookLock)
            {
                books.Add(book);
            }
        }

        public void RemoveBook(int bookID)
        {
            lock (bookLock)
            {
                books.RemoveAll(x => x.Id == bookID);
            }
        }

        public IBook? GetBookByID(int bookID)
        {
            IBook? result = null;
            lock (bookLock)
            {
                result = books.Find(x => x.Id == bookID);
            }
            return result;
        }

        private void UpdateAllBooks(AllBooksUpdateResponse response)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<BookRepositoryChangedEventArgs> observer)
        {
            return new BookRepositoryDisposable(this, observer);
        }

        private void UnSubscribe(IObserver<BookRepositoryChangedEventArgs> observer)
        {
            observers.Remove(observer);
        }

        private class BookRepositoryDisposable : IDisposable
        {
            private readonly FakeBookRepository _bookRepository;
            private readonly IObserver<BookRepositoryChangedEventArgs> _observer;

            public BookRepositoryDisposable(FakeBookRepository bookRepository, IObserver<BookRepositoryChangedEventArgs> observer)
            {
                _bookRepository = bookRepository;
                _observer = observer;
            }
            public void Dispose()
            {
                _bookRepository.UnSubscribe(_observer);
            }
        }
    }

    internal class FakeBook : IBook, ICloneable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; } = false;
        public Genre Genre { get; set; }

        public FakeBook(string author, string title, decimal price, bool recommended)
        {
            Author = author;
            Title = title;
            Price = price;
            Recommended = recommended;
        }
        
        public object Clone()
        {
            FakeBook clone = (FakeBook)MemberwiseClone();
            clone.Title = string.Copy(Title);
            clone.Author = string.Copy(Author);
            return clone;
        }

        public override string ToString()
        {
            return $"{Title} - {Author} ({Price:C})";
        }
    }

    internal class FakeConnectionService : IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        internal WebSocketConnection? WebSocketConnection { get; private set; }
        public Task Connect(Uri peerUri)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeUserContainer : IUserContainer
    {
        public IUser user { get; private set; }

        public event EventHandler<UserChangedEventArgs>? UserChanged;

        public FakeUserContainer()
        {
            user = new FakeUser("Marcin", 30);
        }

        public void ChangeUser(IUser user)
        {
            this.user = user;
            UserChanged?.Invoke(this, new UserChangedEventArgs(user));
        }
    }

    internal class FakeUser : IUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        private List<IBook> _purchasedBooks;
        public FakeUser(string name, decimal initialBalance)
        {
            Name = name;
            Balance = initialBalance;
            _purchasedBooks = new List<IBook>();
        }

        public IEnumerable<IBook> PurchasedBooks => _purchasedBooks.AsReadOnly();

        public void SetPurchasedBooks(IEnumerable<IBook> purchasedBooks)
        {
            _purchasedBooks = purchasedBooks.ToList();
        }

        public void AddPurchasedBook(IBook book)
        {
            if (book != null)
            {
                _purchasedBooks.Add(book);
            }
        }
    }
}
