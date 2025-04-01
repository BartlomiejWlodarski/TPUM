using TPUMProject.Data.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void BookTest()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create("test", 10);

            IBook book = dataAPI.CreateBook("Pan Tadeusz", "Adam Mickiewicz", 20);
            IBook book2 = dataAPI.CreateBook("Quo vadis", "Henryk Sienkiewicz", 25);

            Assert.AreEqual("Pan Tadeusz", book.Title);

            Assert.AreEqual("Adam Mickiewicz", book.Author);

            Assert.AreEqual(20, book.Price);

            Assert.AreNotEqual(book.Id, book2.Id);
        }

        [TestMethod]
        public void CreateBookTest()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create("test", 10);

            Assert.AreEqual(0, dataAPI.CountBooks());

            IBook book = dataAPI.CreateBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            dataAPI.AddBook(book);

            Assert.AreEqual(1, dataAPI.CountBooks());
        }
    }
}