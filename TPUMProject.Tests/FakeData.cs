using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Tests
{
    internal class FakeDataAPI : AbstractDataAPI
    {
        private readonly IUser _user;

        public override IUser User => _user;

        private readonly IBookRepository _bookRepository = new FakeBookRepository();

        public override IBookRepository BookRepository => _bookRepository;

        private int _nextBookId = 0;

        public FakeDataAPI(string userName, decimal initialBalance)
        {
            _user = new FakeUser(userName, initialBalance);
            _bookRepository = new FakeBookRepository();
        }

        public override IBook CreateBook(string title, string author, decimal price)
        {
            return new FakeBook
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

    internal class FakeUser : IUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        private readonly List<IBook> _purchasedBooks;

        public event EventHandler<UserChangedEventArgs>? UserChanged;

        public FakeUser(string name, decimal initialBalance)
        {
            Name = name;
            Balance = initialBalance;
            _purchasedBooks = new List<IBook>();
        }

        public IEnumerable<IBook> PurchasedBooks => _purchasedBooks.AsReadOnly();

        public void AddPurchasedBook(IBook book)
        {
            if (book != null)
            {
                _purchasedBooks.Add(book);
                UserChanged?.Invoke(this, new UserChangedEventArgs(this));
            }
        }
    }

    internal class FakeBookRepository : IBookRepository
    {
        private readonly List<IBook> _books = new();

        public event EventHandler<BookRepositoryChangedEventArgs>? BookRepositoryChangedHandler;

        public IEnumerable<IBook> GetAllBooks() => _books;

        public void AddBook(IBook book)
        {
            book = new FakeBook { Id = _books.Count + 1, Title = book.Title, Author = book.Author, Price = book.Price };
            _books.Add(book);
            BookRepositoryChangedHandler?.Invoke(this, new BookRepositoryChangedEventArgs(book, BookRepositoryChangedEventType.Added));
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
                BookRepositoryChangedHandler?.Invoke(this, new BookRepositoryChangedEventArgs(bookToRemove, BookRepositoryChangedEventType.Removed));
                return true;
            }
            return false;
        }

        public void ChangeBookRecommended(IBook book, bool recommended)
        {
            IBook foundBook = _books.Find(bookToFind => bookToFind.Id == book.Id);
            if (foundBook != null)
            {
                foundBook.Recommended = recommended;
                BookRepositoryChangedHandler?.Invoke(this, new BookRepositoryChangedEventArgs(foundBook, BookRepositoryChangedEventType.Modified));
            }
        }
    }

    internal class FakeBook : IBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; } = false;
        public Genre Genre { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Author} ({Price:C})";
        }
    }
}
