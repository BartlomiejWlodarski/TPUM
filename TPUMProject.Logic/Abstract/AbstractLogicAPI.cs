using TPUMProject.Data.Abstract;

namespace TPUMProject.Logic.Abstract
{
    public abstract class AbstractLogicAPI
    {
        public abstract IBookService BookService { get; }
        public abstract IUser User { get; }

        public static AbstractLogicAPI Create(string userName, decimal initialBalance)
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create(userName, initialBalance);
            return new LogicAPI(dataAPI);
        }

        public static AbstractLogicAPI Create(AbstractDataAPI dataAPI)
        {
            return new LogicAPI(dataAPI);
        }

        public abstract ILogicUser GetUser();

        public abstract void GetRandomRecommendedBook();
    }
}
