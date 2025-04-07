using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

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
            foreach (IUser user in _dataAPI.Users)
            {
                user.UserChanged += HandleOnUserChanged;
            }
            StartRecommending();
        }

        public IEnumerable<ILogicBook> GetAvailableBooks() => _bookRepository.GetAllBooks().Select(book => new LogicBook(book));

        public void AddNewBook(string title, string author, decimal price)
        {
            lock (booksLock)
            {
                IBook book = _dataAPI.CreateBook(title, author, price);
                _bookRepository.AddBook(book);
            }
        }

        public int BuyBook(int id, string username)
        {
            lock (booksLock)
            {
                IBook book = _dataAPI.BookRepository.GetAllBooks().FirstOrDefault(b => b.Id == id);
                IUser? user = _dataAPI.Users.Where(x => x.Name == username).FirstOrDefault();
                if (book == null) return 2; // Book not found

                if(user == null) return 3; // User not found

                if(user.Balance < book.Price) return 1; // Not enought money

                if(book.Recommended)
                {
                    _bookRepository.ChangeBookRecommended(book, false);
                }

                user.Balance -= book.Price;
                user.AddPurchasedBook(book);
                return _dataAPI.BookRepository.RemoveBook(id) ? 0 : 4; //0 - success 4 - unknown error;
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
            return;
        }

        private async void StartRecommending()
        {
            while(true)
            {
                await Task.Delay(recommendationInterval * 1000);

                GetRandomRecommendedBook();
            }
        }
    }
}
