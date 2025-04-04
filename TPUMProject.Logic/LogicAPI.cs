﻿using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Logic
{
    internal class LogicAPI : AbstractLogicAPI
    {
        private readonly AbstractDataAPI _dataAPI;
        private readonly IBookService _bookService;

        public LogicAPI(AbstractDataAPI dataAPI)
        {
            _dataAPI = dataAPI;
            _bookService = new BookService(dataAPI);
        }

        public override IUser User => _dataAPI.User;

        public override IBookService BookService => _bookService;

        public override void GetRandomRecommendedBook()
        {
            _bookService.GetRandomRecommendedBook();
        }

        public override ILogicUser GetUser()
        {
            return new LogicUser(User);
        }
    }
}
