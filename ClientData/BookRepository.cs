using ClientData.Abstract;
using ClinetAPI;
using System.Diagnostics;

namespace ClientData
{
    internal class BookRepository : IBookRepository
    {
        private readonly List<IBook> _books = new();

        public event EventHandler<BookRepositoryChangedEventArgs>? BookRepositoryChangedHandler;
        public event Action? AllBooksUpdated;
        public event EventHandler<BookRepositoryReplacedEventArgs>? BookRepositoryReplacedHandler;

        private HashSet<IObserver<BookRepositoryChangedEventArgs>> observers;

        private object bookLock = new object();

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
                        ChangeBook(response.book.ToBook());
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
