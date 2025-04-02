using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Logic
{
    internal class LogicUser : ILogicUser
    {
        public string Name { get; }

        public decimal Balance { get; set; }

        public IEnumerable<ILogicBook> PurchasedBooks { get; }

        public LogicUser(IUser user)
        {
            Name = user.Name;
            Balance = user.Balance;
            PurchasedBooks = user.PurchasedBooks.Select(book => new LogicBook(book));
        }
    }
}
