namespace ClientData.Abstract
{
    public abstract class AbstractDataAPI
    {
        public static AbstractDataAPI Create(IConnectionService connectionService)
        {
            return new DataAPI(connectionService);
        }
        public abstract void GetUser(string username);

        public abstract void SubscibeToNewsLetterUpdates(bool value);

        public abstract IConnectionService GetConnectionService();

        public abstract IBookRepository GetBookRepository();

        public abstract IUserContainer GetUserContainer();
    }
}
