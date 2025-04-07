namespace ClientData.Abstract
{
    public abstract class AbstractDataAPI
    {
        public abstract IUser User { get; }
        public abstract IBookRepository BookRepository { get; }

        public abstract int CountBooks();

        public static AbstractDataAPI Create(string userName, decimal initialBalance)
        {
            return new DataAPI(userName, initialBalance);
        }
    }
}
