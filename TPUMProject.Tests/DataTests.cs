using TPUMProject.Data.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void TestCreateBook()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create();

            Assert.AreEqual(0, dataAPI.CountBooks());

            IBook book = dataAPI.CreateBook("Pan Tadeusz", "Adam Mickiewicz", 20);

            dataAPI.AddBook(book);

            Assert.AreEqual(1, dataAPI.CountBooks());
        }
    }
}