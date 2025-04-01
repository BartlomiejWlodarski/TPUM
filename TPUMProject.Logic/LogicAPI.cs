using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Logic
{
    internal class LogicAPI : AbstractLogicAPI
    {
        private readonly IBookService _bookService;

        public LogicAPI(AbstractDataAPI dataAPI)
        {
            _bookService = new BookService(dataAPI);
        }

        public override IBookService BookService => _bookService;

        public override void GetRandomRecommendedBook()
        {
            _bookService.GetRandomRecommendedBook();
        }
    }
}
