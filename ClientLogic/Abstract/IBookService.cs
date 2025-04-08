using ClientData.Abstract;

namespace ClientLogic.Abstract
{
    public interface IBookService
    {
        public event EventHandler<LogicBookRepositoryChangedEventArgs> BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs> UserChanged;
        public event EventHandler<LogicBookRepositoryReplacedEventArgs> BookRepositoryReplaced;
        public event Action<int>? LogicTransactionResult;
        public event Action? LogicAllBooksUpdated;

        IEnumerable<ILogicBook> GetAvailableBooks();
        void BuyBook(int id);
        void GetUser(string userName);
        void GetBooks();
    }

    public class LogicBookRepositoryChangedEventArgs : EventArgs
    {
        public ILogicBook AffectedBook;
        public int ChangedEventType;

        public LogicBookRepositoryChangedEventArgs(IBook affectedBoook, BookRepositoryChangedEventType changedEventType)
        {
            //AffectedBook = affectedBoook;
            AffectedBook = new LogicBook(affectedBoook);
            ChangedEventType = ((int)changedEventType);
        }

        public LogicBookRepositoryChangedEventArgs(BookRepositoryChangedEventArgs args)
        {
            //AffectedBook = args.bookAffected;
            AffectedBook = new LogicBook(args.bookAffected);
            ChangedEventType = ((int)args.EventType);
        }
    }

    public class LogicUserChangedEventArgs : EventArgs
    {
        public ILogicUser user;
        
        public LogicUserChangedEventArgs(IUser user)
        {
            this.user = new LogicUser(user);
        }

        public LogicUserChangedEventArgs(UserChangedEventArgs e)
        {
            this.user = new LogicUser(e.user);
        }
    }

    public class LogicBookRepositoryReplacedEventArgs : EventArgs
    {
        public IEnumerable<ILogicBook> BooksReplaced;

        public LogicBookRepositoryReplacedEventArgs(IEnumerable<ILogicBook> booksReplaced) { BooksReplaced = booksReplaced; }

        public LogicBookRepositoryReplacedEventArgs(BookRepositoryReplacedEventArgs bookRepositoryReplacedEventArgs) { BooksReplaced = bookRepositoryReplacedEventArgs.booksAffected.Select(x => new LogicBook(x)); }
    }
}
