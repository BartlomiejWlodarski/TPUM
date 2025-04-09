using ClientData.Abstract;
using ClinetAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPUMProject.ClientDataTests
{
    [TestClass]
    public class ClientDataTest
    {
        static FakeConnectionService connectionService = new FakeConnectionService();
        AbstractDataAPI data = AbstractDataAPI.Create(connectionService);
        int nextId = 7;

        public void PrepareData()
        {
            connectionService.MockUpdateAllBooks([
                new BookDTO(7, "TestTitle", "TestAuthor", 20, false, 1),
            new BookDTO(8, "TestTitle2", "TestAuthor2", 40, false, 2)
            ]);
        }

        [TestMethod]
        public void UpdateAllTest()
        {
            BookDTO[] bookDTOs = [
                new BookDTO(7, "TestTitle", "TestAuthor", 20, false, 1),
            new BookDTO(8, "TestTitle2", "TestAuthor2", 40, false, 2)
            ];

            connectionService.MockUpdateAllBooks(bookDTOs);
            List<IBook> books = (List<IBook>)data.BookRepository.GetAllBooks();

            for (int i = 0; i < bookDTOs.Length; i++)
            {
                Assert.IsTrue(
                    bookDTOs[i].Id == books[i].Id
                    && bookDTOs[i].Author == books[i].Author
                    && bookDTOs[i].Title == books[i].Title
                    && bookDTOs[i].Price == books[i].Price
                    && bookDTOs[i].Recommended == books[i].Recommended);
            }
        }

        [TestMethod]
        public void BuyBookTest()
        {
            connectionService.MockUpdateAllBooks([
                new BookDTO(7, "TestTitle", "TestAuthor", 20, false, 1),
            new BookDTO(8, "TestTitle2", "TestAuthor2", 40, false, 2)
            ]);

            BookDTO[] books;

            connectionService.MockUpdateUser(
                new UserDTO("Marcin", 50, [new BookDTO(6, "TestTitle3", "TestAuthor3", 10, false, 1)])
                );
            data.BuyBook(7);
            //Task.Delay(1000).Wait();

            //Assert.AreEqual(7, connectionService.lastBoughtId);
        }

        [TestMethod]
        public void GetItemsTest()
        {
            PrepareData();

            Assert.AreEqual(data.BookRepository.CountBooks(), 2);
            Assert.AreEqual(data.BookRepository.GetAllBooks().Count(), 2);
        }
    }
}
