using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.ServerLogicTests
{
    [TestClass]
    public class ServerLogicTest
    {
        private AbstractDataAPI _dataAPI;
        private AbstractLogicAPI _logicAPI;

        [TestInitialize]
        public void Setup()
        {
            _dataAPI = new FakeDataAPI();
            _logicAPI = AbstractLogicAPI.Create(_dataAPI);
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
            Assert.AreEqual(2, _logicAPI.BookService.BuyBook(0, "Marcin"));

            _logicAPI.BookService.AddNewBook("Pan Tadeusz", "Adam Mickiewicz", 20);
            _logicAPI.BookService.AddNewBook("Quo vadis", "Henryk Sienkiewicz", 100);

            Assert.AreEqual(2, _logicAPI.BookService.BuyBook(0, "Marcin"));

            Assert.AreEqual(1, _logicAPI.BookService.BuyBook(2, "Marcin"));

            Assert.AreEqual(3, _logicAPI.BookService.BuyBook(1, "Jakub"));

            Assert.AreEqual(0, _logicAPI.BookService.BuyBook(1, "Marcin"));

            Assert.AreEqual(30, _logicAPI.GetUser("Marcin").Balance);

            Assert.AreEqual(1, _logicAPI.BookService.GetAvailableBooks().Count());

            Assert.AreEqual(2, _logicAPI.BookService.BuyBook(1, "Marcin"));
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
