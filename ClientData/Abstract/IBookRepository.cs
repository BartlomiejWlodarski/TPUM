namespace ClientData.Abstract
{
    public interface IBookRepository : IObservable<BookRepositoryChangedEventArgs>
    {
        public event EventHandler<BookRepositoryChangedEventArgs> BookRepositoryChangedHandler;
        public event Action? AllBooksUpdated;
        public event EventHandler<BookRepositoryReplacedEventArgs> BookRepositoryReplacedHandler;

        IEnumerable<IBook> GetAllBooks();
        public abstract int CountBooks();
        bool RemoveBook(int id);
        bool ChangeBook(IBook book);

        bool AddBook(IBook book);

        void LoadAllBooks(IEnumerable<IBook> books);


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
