using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class User : IUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        private readonly List<IBook> _purchasedBooks;

        public event EventHandler<UserChangedEventArgs>? UserChanged;

        public User(string name, decimal initialBalance)
        {
            Name = name;
            Balance = initialBalance;
            _purchasedBooks = new List<IBook>();
        }

        public IEnumerable<IBook> PurchasedBooks => _purchasedBooks.AsReadOnly();

        public void AddPurchasedBook(IBook book)
        {
            if (book != null)
            {
                _purchasedBooks.Add(book);
                UserChanged?.Invoke(this, new UserChangedEventArgs(this));
            }
        }
    }
}
