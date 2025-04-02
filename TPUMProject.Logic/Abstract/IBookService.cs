using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Logic.Abstract
{
    public interface IBookService
    {
        public event EventHandler<LogicBookRepositoryChangedEventArgs> BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs> UserChanged;

        IEnumerable<IBook> GetAvailableBooks();
        void AddNewBook(string title, string author, decimal price);
        bool BuyBook(int id);
        void GetRandomRecommendedBook();
    }

    public class LogicBookRepositoryChangedEventArgs : EventArgs
    {
        public ILogicBook AffectedBook;
        public BookRepositoryChangedEventType ChangedEventType;

        public LogicBookRepositoryChangedEventArgs(IBook affectedBoook, BookRepositoryChangedEventType changedEventType)
        {
            //AffectedBook = affectedBoook;
            AffectedBook = new LogicBook(affectedBoook);
            ChangedEventType = changedEventType;
        }

        public LogicBookRepositoryChangedEventArgs(BookRepositoryChangedEventArgs args)
        {
            //AffectedBook = args.bookAffected;
            AffectedBook = new LogicBook(args.bookAffected);
            ChangedEventType = args.EventType;
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
}
