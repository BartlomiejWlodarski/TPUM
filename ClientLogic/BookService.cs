using ClientData.Abstract;
using ClientLogic.Abstract;
using System.Diagnostics;

namespace ClientLogic
{
    internal class BookService : IBookService, IObserver<BookRepositoryChangedEventArgs>
    {
        private readonly IBookRepository _bookRepository;
        private readonly AbstractDataAPI _dataAPI;

        public event EventHandler<LogicBookRepositoryChangedEventArgs>? BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs>? UserChanged;
        public event Action? LogicAllBooksUpdated;
        public event Action<int>? TransactionResult;

        private IDisposable BookRepoHandle;

        public BookService(AbstractDataAPI dataAPI)
        {
            _bookRepository = dataAPI.GetBookRepository();
            _dataAPI = dataAPI;

            BookRepoHandle = _bookRepository.Subscribe(this);

            _bookRepository.AllBooksUpdated += () => LogicAllBooksUpdated?.Invoke();
            _bookRepository.TransactionResult += (int code) => TransactionResult?.Invoke(code);
            _bookRepository.BookRepositoryChangedHandler += HandleOnBookRepositoryChanged;
            _dataAPI.GetUserContainer().UserChanged += HandleOnUserChanged;
        }

        private void HandleOnBookRepositoryChanged(object sender, BookRepositoryChangedEventArgs e)
        {
            BookRepositoryChanged?.Invoke(this, new LogicBookRepositoryChangedEventArgs(e));
        }

        private void HandleOnUserChanged(object sender, UserChangedEventArgs e)
        {
            UserChanged?.Invoke(this, new LogicUserChangedEventArgs(e));
        }

        public void GetUser(string userName)
        {
            _dataAPI.GetUser(userName);
        }

        public void RequestUpdate()
        {
            _bookRepository.RequestUpdate();
        }

        public async Task SellBook(int id)
        {
            List<ILogicBook> books = _bookRepository.GetAllBooks().Select(book => new LogicBook(book)).Cast<ILogicBook>().ToList();
            ILogicBook? foundBook = books.Find((book) => book.Id == id);
            if(foundBook != null)
            {
                await _bookRepository.SellBook(foundBook.Id,_dataAPI.GetUserContainer().user.Name);
            } 
            else
            {

            }
        }

        public List<ILogicBook> GetAllBooks()
        {
            return _bookRepository.GetAllBooks().Select(book => new LogicBook(book)).Cast<ILogicBook>().ToList();
        }

        public ILogicBook? GetBookByID(int bookID)
        {
            IBook? result = _bookRepository.GetBookByID(bookID);
            return result == null? null : new LogicBook(result);
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
            BookRepositoryChanged?.Invoke(this, new LogicBookRepositoryChangedEventArgs(value));
        }
    }
}
