using ClientData.Abstract;
using ConnectionAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData
{
    internal static class Converters
    {
        public static IBook ToBook(this BookDTO book)
        {
            return new Book() { Id = book.Id, Title = book.Title,Author = book.Author,Price = book.Price,Recommended = book.Recommended,Genre = (Genre)book.Genre};
        }

        public static IUser ToUser(this UserDTO user)
        {
            IUser resultUser = new User(user.Username, user.Balance);
            resultUser.SetPurchasedBooks(user.Books.Select(x => x.ToBook()));
            return resultUser;
        }
    }
}
