using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void BuyIntegrationTest()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create("testUser", 40);
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create(dataAPI);

            Assert.AreEqual(3, logicAPI.BookService.GetAvailableBooks().Count());

            Assert.AreEqual(true, logicAPI.BookService.BuyBook(1));

            Assert.AreEqual(logicAPI.User.Balance, dataAPI.User.Balance);

            Assert.AreEqual(false, logicAPI.BookService.BuyBook(2));
        }
    }
}
