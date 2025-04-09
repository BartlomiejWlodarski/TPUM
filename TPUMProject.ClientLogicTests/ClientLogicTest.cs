using ClientLogic.Abstract;

namespace TPUMProject.ClientLogicTests
{
    [TestClass]
    public class ClientLogicTest
    {
        private AbstractLogicAPI logicAPI = AbstractLogicAPI.Create(new FakeDataAPI());

        [TestMethod]
        public void GetBooksTest()
        {
            Assert.AreEqual(2, logicAPI.GetBookService().GetAllBooks().Count());
        }

        [TestMethod]
        public void BuyBookTest()
        {
            logicAPI.GetBookService().SellBook(0);

            Assert.AreEqual(2, logicAPI.GetBookService().GetAllBooks().Count());
        }
    }
}
