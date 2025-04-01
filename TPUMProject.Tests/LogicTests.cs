using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class LogicTests
    {
        private readonly AbstractLogicAPI _logicAPI;

        public LogicTests()
        {
            _logicAPI = AbstractLogicAPI.Create("testUser", 50);
        }

        [TestMethod]
        public void AddNewBookTest()
        {
            Assert.AreEqual(0, _logicAPI.BookService.GetAvailableBooks().Count());

            _logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            Assert.AreEqual(1, _logicAPI.BookService.GetAvailableBooks().Count());
        }

        [TestMethod]
        public void BuyBookTest()
        {
            Assert.AreEqual(false, _logicAPI.BookService.BuyBook(0));

            _logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);
            _logicAPI.BookService.AddNewBook("Quo vadis", "Henryk Sienkiewicz", 100);

            Assert.AreEqual(false, _logicAPI.BookService.BuyBook(0));

            Assert.AreEqual(false, _logicAPI.BookService.BuyBook(2));

            Assert.AreEqual(true, _logicAPI.BookService.BuyBook(1));

            Assert.AreEqual(30, _logicAPI.User.Balance);

            Assert.AreEqual(1, _logicAPI.BookService.GetAvailableBooks().Count());

            Assert.AreEqual(false, _logicAPI.BookService.BuyBook(1));
        }

        [TestMethod]
        public void RecommendedBookTest()
        {
            _logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);
            _logicAPI.BookService.AddNewBook("Quo vadis", "Henryk Sienkiewicz", 25);

            Assert.AreEqual(_logicAPI.BookService.GetAvailableBooks().ToList()[0].Recommended, _logicAPI.BookService.GetAvailableBooks().ToList()[1].Recommended);

            _logicAPI.BookService.GetRandomRecommendedBook();

            Assert.AreNotEqual(_logicAPI.BookService.GetAvailableBooks().ToList()[0].Recommended, _logicAPI.BookService.GetAvailableBooks().ToList()[1].Recommended);
        }
    }
}
