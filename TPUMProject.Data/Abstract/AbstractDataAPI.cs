using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public abstract class AbstractDataAPI
    {
        public abstract IBookRepository BookRepository { get; }

        public abstract IBook CreateBook(string title, string author, decimal price);

        public abstract int CountBooks();

        public abstract void AddBook(IBook book);

        public static AbstractDataAPI Create()
        {
            return new DataAPI();
        }
    }
}
