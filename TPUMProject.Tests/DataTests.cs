using TPUMProject.Data.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class DataTests
    {
        private AbstractDataAPI _dataAPI;

        [TestInitialize]
        public void Setup()
        {
            _dataAPI = AbstractDataAPI.Create("testUser", 50);
        }

        [TestMethod]
        public void BookTest()
        {
            IBook book = _dataAPI.CreateBook("Pan Tadeusz", "Adam Mickiewicz", 20);
            IBook book2 = _dataAPI.CreateBook("Quo vadis", "Henryk Sienkiewicz", 25);

            Assert.AreEqual("Pan Tadeusz", book.Title);

            Assert.AreEqual("Adam Mickiewicz", book.Author);

            Assert.AreEqual(20, book.Price);

            Assert.AreNotEqual(book.Id, book2.Id);
        }

        [TestMethod]
        public void CreateBookTest()
        {
            Assert.AreEqual(3, _dataAPI.CountBooks());

            IBook book = _dataAPI.CreateBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            _dataAPI.AddBook(book);

            Assert.AreEqual(4, _dataAPI.CountBooks());
        }
    }
}