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

        IEnumerable<IBook> GetAvailableBooks();
        void AddNewBook(string title, string author, decimal price);
        bool BuyBook(int id);
        void GetRandomRecommendedBook();
    }

    public class LogicBookRepositoryChangedEventArgs : EventArgs
    {
        public IBook AffectedBook;
        public BookRepositoryChangedEventType ChangedEventType;

        public LogicBookRepositoryChangedEventArgs(IBook affectedBoook, BookRepositoryChangedEventType changedEventType)
        {
            AffectedBook = affectedBoook;
            ChangedEventType = changedEventType;
        }

        public LogicBookRepositoryChangedEventArgs(BookRepositoryChangedEventArgs args)
        {
            AffectedBook = args.bookAffected;
            ChangedEventType = args.EventType;
        }
    }
}
