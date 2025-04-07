using ClientData.Abstract;
using ClientLogic.Abstract;

namespace ClientLogic
{
    internal class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AbstractDataAPI _dataAPI;

        public event EventHandler<LogicBookRepositoryChangedEventArgs>? BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs> UserChanged;

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
            _dataAPI.User.UserChanged += HandleOnUserChanged;
        }

        public IEnumerable<ILogicBook> GetAvailableBooks() => _bookRepository.GetAllBooks().Select(book => new LogicBook(book));

        public bool BuyBook(int id)
        {
            lock (booksLock)
            {
                IBook book = _dataAPI.BookRepository.GetAllBooks().FirstOrDefault(b => b.Id == id);
                if (book == null || _dataAPI.User.Balance < book.Price)
                    return false;

                if(book.Recommended)
                {
                    _bookRepository.ChangeBookRecommended(book, false);
                }

                _dataAPI.User.Balance -= book.Price;
                _dataAPI.User.AddPurchasedBook(book);
                return _dataAPI.BookRepository.RemoveBook(id);
            }
        }
    }
}
