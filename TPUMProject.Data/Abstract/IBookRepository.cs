using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public interface IBookRepository
    {
        IEnumerable<IBook> GetAllBooks();
        void AddBook(IBook book);
        public abstract int CountBooks();
    }
}
