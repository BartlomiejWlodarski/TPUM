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
            AbstractDataAPI dataAPI = AbstractDataAPI.Create();
            AbstractLogicAPI logicAPI = AbstractLogicAPI.Create(dataAPI);

            Assert.AreEqual(3, logicAPI.BookService.GetAvailableBooks().Count());

            Assert.AreEqual(0, logicAPI.BookService.BuyBook(1, "Marcin"));

            Assert.AreEqual(logicAPI.GetUser("Marcin").Balance, dataAPI.Users.First().Balance);

            Assert.AreEqual(2, logicAPI.BookService.BuyBook(1, "Marcin"));
        }
    }
}
