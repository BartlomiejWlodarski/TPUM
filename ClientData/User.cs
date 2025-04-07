using ClientData.Abstract;

namespace ClientData
{
    internal class User : IUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        private List<IBook> _purchasedBooks;

        public event EventHandler<UserChangedEventArgs>? UserChanged;

        public User(string name, decimal initialBalance)
        {
            Name = name;
            Balance = initialBalance;
            _purchasedBooks = new List<IBook>();
        }

        public IEnumerable<IBook> PurchasedBooks => _purchasedBooks.AsReadOnly();

        public void SetPurchasedBooks(IEnumerable<IBook> purchasedBooks)
        {
            _purchasedBooks = purchasedBooks.ToList();
        }

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
