using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClientLogic.Abstract;

namespace TPUMProject.Presentation.Model
{
    internal class ModelUser : IModelUser,INotifyPropertyChanged
    {
        public string Name { get; }
        public decimal Balance {get; set;}
        public IEnumerable<IModelBook> PurchasedBooks { get; }

        public ModelUser(ILogicUser user)
        {
            this.Name = user.Name;
            this.Balance = user.Balance;
            this.PurchasedBooks = user.PurchasedBooks.Select(book => new ModelBook(book));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
