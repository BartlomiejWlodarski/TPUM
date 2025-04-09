using ClientData.Abstract;

namespace ClientLogic.Abstract
{
    public abstract class AbstractLogicAPI
    {
        public AbstractDataAPI DataAPI { get;private set; }

        public AbstractLogicAPI(AbstractDataAPI abstractDataAPI) 
        { 
            DataAPI = abstractDataAPI;
        }
        public static AbstractLogicAPI Create(AbstractDataAPI? dataAPI = null)
        {
            AbstractDataAPI abstractData = dataAPI ?? AbstractDataAPI.Create(null);
            return new LogicAPI(abstractData);
        }

        public abstract IBookService GetBookService();
        public abstract ILogicUser GetUser();

        public abstract ILogicConnectionService GetConnectionService();
    }
}
