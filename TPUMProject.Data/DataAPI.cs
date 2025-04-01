using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class DataAPI : AbstractDataAPI
    {
        private readonly IUser _user;

        public override IUser User => _user;

        private readonly IBookRepository _bookRepository = new BookRepository();

        public override IBookRepository BookRepository => _bookRepository;

        private int _nextBookId = 0;

        public DataAPI(string userName, decimal initialBalance)
        {
            _user = new User(userName, initialBalance);
            _bookRepository = new BookRepository();
        }

        public override IBook CreateBook(string title, string author, decimal price)
        {
            return new Book
            {
                Id = _nextBookId++,
                Title = title,
                Author = author,
                Price = price
            };
        }

        public override int CountBooks()
        {
            return _bookRepository.CountBooks();
        }

        public override void AddBook(IBook book)
        {
            _bookRepository.AddBook(book);
        }
    }
}
