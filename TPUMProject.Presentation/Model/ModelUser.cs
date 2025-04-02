using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public class ModelUser : IModelUser,INotifyPropertyChanged
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
