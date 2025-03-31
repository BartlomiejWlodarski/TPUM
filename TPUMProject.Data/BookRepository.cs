using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class BookRepository : IBookRepository
    {
        private readonly List<IBook> _books = new();

        public IEnumerable<IBook> GetAllBooks() => _books;

        public void AddBook(IBook book)
        {
            book = new Book { Id = _books.Count + 1, Title = book.Title, Author = book.Author, Price = book.Price };
            _books.Add(book);
        }

        public int CountBooks()
        {
            return _books.Count;
        }

        public bool RemoveBook(int id)
        {
            IBook bookToRemove = _books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                _books.Remove(bookToRemove);
                return true;
            }
            return false;
        }
    }
}
