using ClientData.Abstract;
using ClientLogic.Abstract;

namespace ClientLogic
{
    internal class BookService : IBookService, IObserver<BookRepositoryChangedEventArgs>
    {
        private readonly IBookRepository _bookRepository;
        private readonly AbstractDataAPI _dataAPI;

        public event EventHandler<LogicBookRepositoryChangedEventArgs>? BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs> UserChanged;
        public event Action<int>? LogicTransactionResult;
        public event Action? LogicAllBooksUpdated;

        private IDisposable BookRepoHandle;

        private object booksLock = new object();

        private void HandleOnBookRepositoryChanged(object sender, BookRepositoryChangedEventArgs e)
        {
            BookRepositoryChanged?.Invoke(this, new LogicBookRepositoryChangedEventArgs(e));
        }

        private void HandleOnUserChanged(object sender, UserChangedEventArgs e)
        {
            UserChanged?.Invoke(this, new LogicUserChangedEventArgs(e));
        }

        public BookService(AbstractDataAPI dataAPI)
        {
            _dataAPI = dataAPI;
            _bookRepository = dataAPI.BookRepository;
            _bookRepository.BookRepositoryChangedHandler += HandleOnBookRepositoryChanged;
            _dataAPI.UserChanged += HandleOnUserChanged;
            _dataAPI.TransactionResult += (int code) => LogicTransactionResult?.Invoke(code);
            _bookRepository.AllBooksUpdated += () => LogicAllBooksUpdated?.Invoke();

            BookRepoHandle = _bookRepository.Subscribe(this);
        }

        public IEnumerable<ILogicBook> GetAvailableBooks() => _bookRepository.GetAllBooks().Select(book => new LogicBook(book));

        public void BuyBook(int id)
        {
            lock (booksLock)
            {
                _dataAPI?.BuyBook(id);
            }
        }

        public void OnCompleted()
        {
            BookRepoHandle.Dispose();
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(BookRepositoryChangedEventArgs value)
        {
            BookRepositoryChanged?.Invoke(this,new LogicBookRepositoryChangedEventArgs(value));
        }

        public void GetUser(string userName)
        {
            _dataAPI.GetUser(userName);
        }

        public void GetBooks()
        {
            _dataAPI.RequestBooks();
        }
    }
}
