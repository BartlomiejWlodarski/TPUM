using ClientData.Abstract;
using ClinetAPI;
using System.Diagnostics;

namespace ClientData
{
    internal class BookRepository : IBookRepository
    {
        private readonly List<IBook> _books = new();
        private object bookLock = new object();

        private HashSet<IObserver<BookRepositoryChangedEventArgs>> observers;

        public event Action? AllBooksUpdated;
        public event EventHandler<BookRepositoryChangedEventArgs>? BookRepositoryChangedHandler;

        IConnectionService connectionService;

        public BookRepository(IConnectionService connectionService)
        {
            observers = new HashSet<IObserver<BookRepositoryChangedEventArgs>>();
            this.connectionService = connectionService;
            this.connectionService.OnMessage += OnMessage;
        }

        ~BookRepository()
        {
            List<IObserver<BookRepositoryChangedEventArgs>> cachedObservers = observers.ToList();
            foreach (IObserver<BookRepositoryChangedEventArgs>? observer in cachedObservers)
            {
                observer?.OnCompleted();
            }
        }

        private void OnMessage(string message)
        {
            Serializer serializer = Serializer.Create();

            if (serializer.GetCommandHeader(message) == BookChangedResponse.StaticHeader)
            {
                BookChangedResponse response = serializer.Deserialize<BookChangedResponse>(message);
                switch (response.changeType)
                {
                    case 0:
                        AddBook(response.book.ToBook());
                        break;
                    case 1:
                        RemoveBook(response.book.Id);
                        break;
                    case 2:
                        ReplaceBook(response.book.ToBook());
                        break;
                    default:
                        break;
                }
            }
            else if (serializer.GetCommandHeader(message) == AllBooksUpdateResponse.StaticHeader)
            {
                AllBooksUpdateResponse response = serializer.Deserialize<AllBooksUpdateResponse>(message);
                UpdateAllBooks(response);
            }
            else if (serializer.GetCommandHeader(message) == TransactionResultResponse.StaticHeader)
            {
                TransactionResultResponse response = serializer.Deserialize<TransactionResultResponse>(message);
                //TransactionResult?.Invoke(response.ResultCode);
            }
        }

        public async Task RequestBooks()
        {
            Serializer serializer = Serializer.Create();
            await connectionService.SendAsync(serializer.Serialize(new GetBooksCommand()));
        }

        public async Task SellBook(int id, string username)
        {
            if (connectionService.IsConnected())
            {
                Serializer serializer = Serializer.Create();
                SellBookCommand command = new SellBookCommand(id, username);
                await connectionService.SendAsync(serializer.Serialize<SellBookCommand>(command));
            }
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
            List<IBook> books = new List<IBook> ();
            lock (bookLock)
            {
                books.AddRange (_books.Select(book => (IBook)book.Clone()));
            }
            return books;
        }

        public void ReplaceBook(IBook replacement)
        {
            lock (bookLock)
            {
                int index = _books.FindIndex(book => book.Id == replacement.Id);
                if(index > -1 && index < _books.Count)
                {
                    _books[index] = replacement;
                }
            }

            foreach(IObserver<BookRepositoryChangedEventArgs>? observer in observers)
            {
                observer.OnNext(new BookRepositoryChangedEventArgs(replacement, BookRepositoryChangedEventType.Modified));
            }
        }

        public void AddBook(IBook book)
        {
            lock (bookLock)
            {
                _books.Add(book);
            }
        }

        public void RemoveBook(int bookID)
        {
            lock (bookLock)
            {
                _books.RemoveAll(x => x.Id == bookID);
            }
        }

        public IBook? GetBookByID(int bookID)
        {
            IBook? result = null;
            lock (bookLock)
            {
                result = _books.Find(x => x.Id == bookID);
            }
            return result;
        }

        private void UpdateAllBooks(AllBooksUpdateResponse response)
        {
            if (response.Books == null) return;
            lock (bookLock)
            {
                _books.Clear();
                foreach (BookDTO book in response.Books)
                {
                    _books.Add(book.ToBook());
                }
            }
            AllBooksUpdated?.Invoke();
        }

        public IDisposable Subscribe(IObserver<BookRepositoryChangedEventArgs> observer)
        {
            observers.Add(observer);
            return new BookRepositoryDisposable(this, observer);
        }

        private void UnSubscribe(IObserver<BookRepositoryChangedEventArgs> observer)
        {
            observers.Remove(observer);
        }

        private class BookRepositoryDisposable : IDisposable
        {
            private readonly BookRepository _bookRepository;
            private readonly IObserver<BookRepositoryChangedEventArgs> _observer;

            public BookRepositoryDisposable(BookRepository bookRepository, IObserver<BookRepositoryChangedEventArgs> observer)
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
}
