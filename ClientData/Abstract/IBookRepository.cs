namespace ClientData.Abstract
{
    public interface IBookRepository : IObservable<BookRepositoryChangedEventArgs>
    {
        public event EventHandler<BookRepositoryChangedEventArgs> BookRepositoryChangedHandler;
        public event Action? AllBooksUpdated;

        public void RequestUpdate();

        public Task SellBook(int id, string username);

        public List<IBook> GetAllBooks();

        public IBook? GetBookByID(int bookID);
    }

    public enum BookRepositoryChangedEventType
    {
        Added,
        Removed,
        Modified
    }
    public class BookRepositoryChangedEventArgs : EventArgs
    {
        public IBook bookAffected;
        public BookRepositoryChangedEventType EventType;

        public BookRepositoryChangedEventArgs(IBook bookAffected, BookRepositoryChangedEventType changeType)
        {
            this.bookAffected = bookAffected;
            this.EventType = changeType;
        }
    }

    public class BookRepositoryReplacedEventArgs : EventArgs
    {
        public IEnumerable<IBook> booksAffected;

        public BookRepositoryReplacedEventArgs(IEnumerable<IBook> books)
        {
            this.booksAffected = books;
        }
    }
}
