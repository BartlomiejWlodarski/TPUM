namespace ClientData.Abstract
{
    public abstract class AbstractDataAPI
    {
        public event EventHandler<UserChangedEventArgs> UserChanged;
        public event Action<int>? TransactionResult;

        public abstract IUser User { get; }
        public abstract IBookRepository BookRepository { get; }

        public abstract int CountBooks();

        public abstract IConnectionService GetConnectionService();

        public abstract void RequestBooks();

        public abstract void GetUser(string username);

        public abstract void BuyBook(int bookID);

        public static AbstractDataAPI Create(IConnectionService connectionService)
        {
            return new DataAPI(connectionService??new ConnectionService());
        }
    }
}
