namespace TPUMProject.Data.Abstract
{
    public abstract class AbstractDataAPI
    {
        public abstract IUser User { get; }
        public abstract IBookRepository BookRepository { get; }

        public abstract IBook CreateBook(string title, string author, decimal price);

        public abstract int CountBooks();

        public abstract void AddBook(IBook book);

        public static AbstractDataAPI Create(string userName, decimal initialBalance)
        {
            return new DataAPI(userName, initialBalance);
        }
    }
}
