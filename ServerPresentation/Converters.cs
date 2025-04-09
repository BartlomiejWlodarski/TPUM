using ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Logic.Abstract;

namespace ServerPresentation
{
    internal static class Converters
    {
        public static BookDTO ConvertToDTO(this ILogicBook book)
        {
            return new BookDTO(book.Id, book.Title, book.Author, book.Price, book.Recommended, (int)book.Genre);
        }

        public static UserDTO ConvertToDTO(this ILogicUser user)
        {
            return new UserDTO(user.Name, user.Balance, user.PurchasedBooks.Select(x => x.ConvertToDTO()).ToArray());
        }
    }
}
