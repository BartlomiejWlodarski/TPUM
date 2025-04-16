using ClientData.Abstract;
using ConnectionAPI;
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
                new FakeBookDTO{Id = 7, Title = "TestTitle", Author = "TestAuthor", Price = 20, Recommended = false, Genre = 1 },
                new FakeBookDTO{Id = 8, Title = "TestTitle2", Author = "TestAuthor2", Price = 40, Recommended = false, Genre = 2 }
            ]);
        }

        [TestMethod]
        public void UpdateAllTest()
        {
            FakeBookDTO[] bookDTOs = [
                new FakeBookDTO{Id = 7, Title = "TestTitle", Author = "TestAuthor", Price = 20, Recommended = false, Genre = 1 },
                new FakeBookDTO{Id = 8, Title = "TestTitle2", Author = "TestAuthor2", Price = 40, Recommended = false, Genre = 2 } 
            ];

            connectionService.MockUpdateAllBooks(bookDTOs);
            List<IBook> books = (List<IBook>)data.GetBookRepository().GetAllBooks();

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
        public void GetItemsTest()
        {
            Assert.AreEqual(data.GetBookRepository().GetAllBooks().Count(), 0);

            connectionService.MockUpdateAllBooks([
                new FakeBookDTO{Id = 7, Title = "TestTitle", Author = "TestAuthor", Price = 20, Recommended = false, Genre = 1 },
                new FakeBookDTO{Id = 8, Title = "TestTitle2", Author = "TestAuthor2", Price = 40, Recommended = false, Genre = 2 }
            ]);

            Assert.AreEqual(data.GetBookRepository().GetAllBooks().Count(), 2);
        }

        [TestMethod]
        public async Task BuyBookTest()
        {
            connectionService.MockUpdateAllBooks([
                new FakeBookDTO{ Id = 7, Title = "TestTitle", Author = "TestAuthor", Price = 20, Recommended = false, Genre = 1 },
                new FakeBookDTO{ Id = 8, Title = "TestTitle2", Author = "TestAuthor2", Price = 40, Recommended = false, Genre = 2 }
            ]);

            BookDTO[] books;

            connectionService.MockUpdateUser(
                new FakeUserDTO { Username = "Marcin", Balance = 50, 
                    Books = [new FakeBookDTO { Id = 6, Title = "TestTitle3", Author = "TestAuthor3", Price = 10, Recommended = false, Genre = 1 }] 
                });

            await data.GetBookRepository().SellBook(7, "Marcin");

            Assert.AreEqual(7, connectionService.lastBoughtId);
        }

        
    }
}
