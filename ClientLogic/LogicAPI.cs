using ClientData.Abstract;
using ClientLogic.Abstract;

namespace ClientLogic
{
    internal class LogicAPI : AbstractLogicAPI
    {
        private readonly AbstractDataAPI _dataAPI;
        private readonly IBookService _bookService;
        private readonly ILogicConnectionService _logicConnectionService;

        public LogicAPI(AbstractDataAPI dataAPI)
        {
            _dataAPI = dataAPI;
            _bookService = new BookService(dataAPI);
            _logicConnectionService = new LogicConnectionService(_dataAPI.GetConnectionService());
        }

        public override IUser User => _dataAPI.User.user;

        public override IBookService BookService => _bookService;

        public override ILogicConnectionService GetConnectionService()
        {
            return _logicConnectionService;
        }

        public override ILogicUser GetUser()
        {
            return new LogicUser(User);
        }
    }
}
