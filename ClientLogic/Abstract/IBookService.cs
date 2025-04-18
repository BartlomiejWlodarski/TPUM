﻿using ClientData.Abstract;

namespace ClientLogic.Abstract
{
    public interface IBookService
    {
        public event EventHandler<LogicBookRepositoryChangedEventArgs> BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs> UserChanged;
        public event Action? LogicAllBooksUpdated;
        public event Action<int>? TransactionResult;
        public event Action<int>? NewsletterUpdate;

        public void RequestUpdate();

        public Task SellBook(int id);

        public List<ILogicBook> GetAllBooks();

        void GetUser(string userName);

        void Subscibe(bool value);

        public ILogicBook? GetBookByID(int bookID);
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
