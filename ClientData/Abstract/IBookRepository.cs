namespace ClientData.Abstract
{
    public interface IBookRepository
    {
        public event EventHandler<BookRepositoryChangedEventArgs> BookRepositoryChangedHandler;
        IEnumerable<IBook> GetAllBooks();
        public abstract int CountBooks();
        bool RemoveBook(int id);
        void ChangeBookRecommended(IBook book, bool recommended);
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
}
