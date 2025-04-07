using TPUMProject.Data.Abstract;

namespace TPUMProject.Logic.Abstract
{
    public abstract class AbstractLogicAPI
    {
        public abstract IBookService BookService { get; }
        public abstract IEnumerable<IUser> Users { get; }

        public static AbstractLogicAPI Create()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create();
            return new LogicAPI(dataAPI);
        }

        public static AbstractLogicAPI Create(AbstractDataAPI dataAPI)
        {
            return new LogicAPI(dataAPI);
        }

        public abstract ILogicUser GetUser(string username);

        public abstract void GetRandomRecommendedBook();
    }
}
