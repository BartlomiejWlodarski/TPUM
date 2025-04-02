using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class BookRepository : IBookRepository
    {
        private readonly List<IBook> _books = new();

        public event EventHandler<BookRepositoryChangedEventArgs>? BookRepositoryChangedHandler;

        public IEnumerable<IBook> GetAllBooks() => _books;

        public void AddBook(IBook book)
        {
            book = new Book { Id = _books.Count + 1, Title = book.Title, Author = book.Author, Price = book.Price };
            _books.Add(book);
            BookRepositoryChangedHandler?.Invoke(this, new BookRepositoryChangedEventArgs(book,BookRepositoryChangedEventType.Added));
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
}
