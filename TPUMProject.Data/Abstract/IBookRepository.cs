using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public interface IBookRepository
    {
        public event EventHandler<BookAddedEventArgs> BookAddedHandler;
        IEnumerable<IBook> GetAllBooks();
        void AddBook(IBook book);
        public abstract int CountBooks();
        bool RemoveBook(int id);
    }

    public class BookAddedEventArgs : EventArgs
    {
           public IBook bookAdded;

        public BookAddedEventArgs(IBook bookAdded)
        {
            this.bookAdded = bookAdded;
        }
    }
}
