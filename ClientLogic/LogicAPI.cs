using ClientData.Abstract;
using ClientLogic.Abstract;

namespace ClientLogic
{
    internal class LogicAPI : AbstractLogicAPI
    {
        private readonly IUser user;
        private readonly IBookService _bookService;
        private readonly ILogicConnectionService _logicConnectionService;

        public LogicAPI(AbstractDataAPI dataAPI) : base(dataAPI)
        {
            _bookService = new BookService(dataAPI);
            user = dataAPI.GetUserContainer().user;
            _logicConnectionService = new LogicConnectionService(dataAPI.GetConnectionService());
        }
        public override IBookService GetBookService()
        {
            return _bookService;
        }

        public override ILogicConnectionService GetConnectionService()
        {
            return _logicConnectionService;
        }

        public override ILogicUser GetUser()
        {
            return new LogicUser(user);
        }
    }
}
