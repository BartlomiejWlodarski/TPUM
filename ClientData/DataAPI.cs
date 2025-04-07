using ClientData.Abstract;

namespace ClientData
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

        public override int CountBooks()
        {
            return _bookRepository.CountBooks();
        }
    }
}
