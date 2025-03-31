using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Data;
using TPUMProject.Logic.Abstract;
using static System.Reflection.Metadata.BlobBuilder;

namespace TPUMProject.Logic
{
    internal class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AbstractDataAPI _dataAPI;

        public BookService(AbstractDataAPI dataAPI)
        {
            _dataAPI = dataAPI;
            _bookRepository = dataAPI.BookRepository;
        }

        public IEnumerable<IBook> GetAvailableBooks() => _bookRepository.GetAllBooks();

        public void AddNewBook(string title, string author, decimal price)
        {
            IBook book = _dataAPI.CreateBook(title, author, price);
            _bookRepository.AddBook(book);
        }

        public bool BuyBook(int id)
        {
            return _bookRepository.RemoveBook(id);
        }

        public IBook GetRandomRecommendedBook(IBook currentRecommendedBook)
        {
            if (_bookRepository.CountBooks() == 0)
                return null;

            var books = _bookRepository.GetAllBooks().ToList();
            Random random = new();
            IBook newBook;
            int maxAttempts = 10;

            do
            {
                int index = random.Next(books.Count);
                newBook = books[index];
            }
            while (newBook == currentRecommendedBook && --maxAttempts > 0); // TODO: maybe change the check to id or title + author

            return newBook;
        }
    }
}
