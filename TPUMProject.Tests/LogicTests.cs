using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void AddNewBookTest()
        {
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create();

            Assert.AreEqual(0, logicAPI.BookService.GetAvailableBooks().Count());

            logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            Assert.AreEqual(1, logicAPI.BookService.GetAvailableBooks().Count());
        }

        [TestMethod]
        public void BuyBookTest()
        {
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create();

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(0));

            logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(0));

            Assert.AreEqual(true, logicAPI.BookService.BuyBook(1));

            Assert.AreEqual(0, logicAPI.BookService.GetAvailableBooks().Count());

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(1));
        }
    }
}
