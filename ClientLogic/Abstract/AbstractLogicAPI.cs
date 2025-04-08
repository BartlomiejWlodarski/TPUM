using ClientData.Abstract;

namespace ClientLogic.Abstract
{
    public abstract class AbstractLogicAPI
    {
        public abstract IBookService BookService { get; }
        public abstract IUser User { get; }

        public static AbstractLogicAPI Create()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create(null);
            return new LogicAPI(dataAPI);
        }

        public static AbstractLogicAPI Create(AbstractDataAPI dataAPI)
        {
            return new LogicAPI(dataAPI);
        }

        public abstract ILogicUser GetUser();

        public abstract ILogicConnectionService GetConnectionService();
    }
}
