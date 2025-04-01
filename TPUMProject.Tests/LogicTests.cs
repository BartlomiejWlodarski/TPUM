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
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create("test", 10);

            Assert.AreEqual(0, logicAPI.BookService.GetAvailableBooks().Count());

            logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            Assert.AreEqual(1, logicAPI.BookService.GetAvailableBooks().Count());
        }

        [TestMethod]
        public void BuyBookTest()
        {
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create("test", 50);

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(0));

            logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(0));

            Assert.AreEqual(true, logicAPI.BookService.BuyBook(1));

            Assert.AreEqual(30, logicAPI.User.Balance);

            Assert.AreEqual(0, logicAPI.BookService.GetAvailableBooks().Count());

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(1));

            // TODO: more buy tests
        }

        [TestMethod]
        public void RecommendedBookTest()
        {
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create("test", 10);

            logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);
            logicAPI.BookService.AddNewBook("Quo vadis", "Henryk Sienkiewicz", 25);

            Assert.AreEqual(logicAPI.BookService.GetAvailableBooks().ToList()[0].Recommended, logicAPI.BookService.GetAvailableBooks().ToList()[1].Recommended);

            logicAPI.BookService.GetRandomRecommendedBook();

            Assert.AreNotEqual(logicAPI.BookService.GetAvailableBooks().ToList()[0].Recommended, logicAPI.BookService.GetAvailableBooks().ToList()[1].Recommended);
        }
    }
}
