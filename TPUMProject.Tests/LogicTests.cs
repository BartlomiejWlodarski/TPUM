using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void TestAddNewBook()
        {
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create();

            Assert.AreEqual(0, logicAPI.BookService.GetAvailableBooks().Count());

            logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            Assert.AreEqual(1, logicAPI.BookService.GetAvailableBooks().Count());
        }
    }
}
