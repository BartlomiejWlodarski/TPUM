using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public interface IUser
    {
        public event EventHandler<UserChangedEventArgs> UserChanged;

        string Name { get; }
        decimal Balance { get; set; }
        IEnumerable<IBook> PurchasedBooks { get; }

        void AddPurchasedBook(IBook book);
    }

    public class UserChangedEventArgs : EventArgs
    {
        public IUser user;

        public UserChangedEventArgs(IUser user)
        {
            this.user = user;
        }
    } 
}
