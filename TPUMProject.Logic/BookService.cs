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

        public void GetRandomRecommendedBook()
        {
            if (_bookRepository.CountBooks() == 0)
                return;

            var books = _bookRepository.GetAllBooks().ToList();
            int recommendedID = 0;
            
            for (int i = 0; i < books.Count(); i++)
            {
                if (books[i].Id == recommendedID)
                {
                    recommendedID = books[i].Id;
                    books[i].Recommended = false;
                    break;
                }
            }

            Random random = new();
            IBook newRecommended;
            int maxAttempts = 10;

            do
            {
                int index = random.Next(books.Count);
                newRecommended = books[index];
            }
            while (newRecommended.Id == recommendedID && --maxAttempts > 0); // TODO: maybe change the check to id or title + author

            for (int i = 0; i < books.Count(); i++)
            {
                if (books[i].Id == newRecommended.Id)
                {
                    books[i].Recommended = true;
                    break;
                }
            }

            return;
        }
    }
}
