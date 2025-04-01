using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public interface IUser
    {
        string Name { get; }
        decimal Balance { get; set; }
        IEnumerable<IBook> PurchasedBooks { get; }

        void AddPurchasedBook(IBook book);
    }
}
