using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class DataAPI : AbstractDataAPI
    {
        private readonly List<IUser> _users;

        public override IEnumerable<IUser> Users => _users;

        private readonly IBookRepository _bookRepository = new BookRepository();

        public override IBookRepository BookRepository => _bookRepository;

        private int _nextBookId = 0;

        public DataAPI()
        {
            _users = new List<IUser>();
            _users.Add(new User("Marcin", 100));
            _users.Add(new User("Michał", 50));
            _users.Add(new User("Konrad", 10));
            _bookRepository = new BookRepository();
            _bookRepository.AddBook(CreateBook("Pan Tadeusz", "Adam Mickiewicz", 20));
            _bookRepository.AddBook(CreateBook("Romeo i Julia", "Szekspir", 25));
            _bookRepository.AddBook(CreateBook("Quo vadis", "Henryk Sienkiewicz", 27));
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
