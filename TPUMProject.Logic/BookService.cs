using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Data;
using TPUMProject.Logic.Abstract;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net;

namespace TPUMProject.Logic
{
    internal class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AbstractDataAPI _dataAPI;

        public event EventHandler<LogicBookRepositoryChangedEventArgs>? BookRepositoryChanged;
        public event EventHandler<LogicUserChangedEventArgs> UserChanged;

        private object booksLock = new object();

        private readonly int recommendationInterval = 5;

        private void HandleOnBookRepositoryChanged(object sender, BookRepositoryChangedEventArgs e)
        {
            BookRepositoryChanged?.Invoke(this, new LogicBookRepositoryChangedEventArgs(e));
        }

        private void HandleOnUserChanged(object sender, UserChangedEventArgs e)
        {
            UserChanged?.Invoke(this, new LogicUserChangedEventArgs(e));
        }

        public BookService(AbstractDataAPI dataAPI)
        {
            _dataAPI = dataAPI;
            _bookRepository = dataAPI.BookRepository;
            _bookRepository.BookRepositoryChangedHandler += HandleOnBookRepositoryChanged;
            _dataAPI.User.UserChanged += HandleOnUserChanged;

            StartRecommending();
        }

        public IEnumerable<IBook> GetAvailableBooks() => _bookRepository.GetAllBooks();

        public void AddNewBook(string title, string author, decimal price)
        {
            lock (booksLock)
            {
                IBook book = _dataAPI.CreateBook(title, author, price);
                _bookRepository.AddBook(book);
            }
        }

        public bool BuyBook(int id)
        {
            lock (booksLock)
            {
                IBook book = _dataAPI.BookRepository.GetAllBooks().FirstOrDefault(b => b.Id == id);
                if (book == null || _dataAPI.User.Balance < book.Price)
                    return false;

                if(book.Recommended)
                {
                    _bookRepository.ChangeBookRecommended(book, false);
                }

                _dataAPI.User.Balance -= book.Price;
                _dataAPI.User.AddPurchasedBook(book);
                return _dataAPI.BookRepository.RemoveBook(id);
            }
        }

        public void GetRandomRecommendedBook()
        {
            lock (booksLock)
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
                        _bookRepository.ChangeBookRecommended(books[i], false);

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
                        _bookRepository.ChangeBookRecommended(books[i], true);
                        break;
                    }
                }
            }
            return;
        }

        private async void StartRecommending()
        {
            while(true)
            {
                await Task.Delay(recommendationInterval * 1000);

                lock(booksLock)
                {
                    if (_bookRepository.CountBooks() == 0)
                        return;

                    var books = _bookRepository.GetAllBooks().ToList();
                    int recommendedID = 0;

                    for (int i = 0; i < books.Count(); i++)
                    {
                        if (books[i].Recommended)
                        {
                            recommendedID = books[i].Id;
                            _bookRepository.ChangeBookRecommended(books[i], false);
                            BookRepositoryChanged?.Invoke(this, new LogicBookRepositoryChangedEventArgs(books[i], BookRepositoryChangedEventType.Modified));
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
                    while (newRecommended.Id == recommendedID && --maxAttempts > 0);

                    for (int i = 0; i < books.Count(); i++)
                    {
                        if (books[i].Id == newRecommended.Id)
                        {
                            _bookRepository.ChangeBookRecommended(books[i], true);
                            BookRepositoryChanged?.Invoke(this, new LogicBookRepositoryChangedEventArgs(books[i], BookRepositoryChangedEventType.Modified));
                            break;
                        }
                    }
                }
            }
        }
    }
}
