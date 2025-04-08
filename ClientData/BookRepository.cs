using ClientData.Abstract;
using System.Diagnostics;

namespace ClientData
{
    internal class BookRepository : IBookRepository
    {
        private readonly List<IBook> _books = new();

        public event EventHandler<BookRepositoryChangedEventArgs>? BookRepositoryChangedHandler;
        public event Action? AllBooksUpdated;
        public event EventHandler<BookRepositoryReplacedEventArgs>? BookRepositoryReplacedHandler;


        private HashSet<IObserver<BookRepositoryChangedEventArgs>> observers = new HashSet<IObserver<BookRepositoryChangedEventArgs>>();

        private object bookLock = new object();

        public IEnumerable<IBook> GetAllBooks() => _books;

        public int CountBooks()
        {
            return _books.Count;
        }

        public bool RemoveBook(int id)
        {
            lock (bookLock)
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
        }

        public bool ChangeBook(IBook book)  
        {
            lock (bookLock)
            {
                int result = _books.IndexOf(_books.Find(x => x.Id == book.Id));
                if (result == -1) return false;
                _books[result] = book;
                //BookRepositoryChangedHandler?.Invoke(this, new BookRepositoryChangedEventArgs(book, BookRepositoryChangedEventType.Modified));
                foreach (IObserver<BookRepositoryChangedEventArgs> observer in observers)
                {
                    observer.OnNext(new BookRepositoryChangedEventArgs(book, BookRepositoryChangedEventType.Modified));
                }
                return true;
            }
        }

        public bool AddBook(IBook book)
        {
            lock (bookLock)
            {
                IBook? result = _books.Find(x => x.Id == book.Id);
                if (result == null)
                {
                    _books.Add(book);
                    BookRepositoryChangedHandler?.Invoke(this, new BookRepositoryChangedEventArgs(book, BookRepositoryChangedEventType.Added));
                    return true;
                }
                return false;
            }
        }

        public void LoadAllBooks(IEnumerable<IBook> books)
        {
            lock (bookLock)
            {
                _books.Clear();
                _books.AddRange(books);
                BookRepositoryReplacedHandler?.Invoke(this,new BookRepositoryReplacedEventArgs(books));
                //AllBooksUpdated?.Invoke();
            }
        }

        public IDisposable Subscribe(IObserver<BookRepositoryChangedEventArgs> observer)
        {
            observers.Add(observer);
            return new BookRepositoryDisposable(this, observer);
        }

        private void UnSubscribe(IObserver<BookRepositoryChangedEventArgs> observer)
        {
            observers.Remove(observer);
        }

        private class BookRepositoryDisposable : IDisposable
        {
            private readonly BookRepository _bookRepository;
            private readonly IObserver<BookRepositoryChangedEventArgs> _observer;

            public BookRepositoryDisposable(BookRepository bookRepository, IObserver<BookRepositoryChangedEventArgs> observer)
            {
                _bookRepository = bookRepository;
                _observer = observer;
            }
            public void Dispose()
            {
                _bookRepository.UnSubscribe(_observer);
            }
        }
    }
}
