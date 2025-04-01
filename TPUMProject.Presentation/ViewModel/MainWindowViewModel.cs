using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TPUMProject.Data.Abstract;
using TPUMProject.Presentation.Model;

namespace TPUMProject.Presentation.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ModelAbstractAPI ModelLayer;
        private bool Disposed = false;
        private string _testString = string.Empty;

        //public ICommand Previous { get; set; }
        public ICommand Buy { get; set; }

        public string TestString
        {
            get { return _testString; }
            set
            {
                _testString = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel() : this(null)
        { }

        internal MainWindowViewModel(ModelAbstractAPI modelLayerAPI)
        {
            ModelLayer = modelLayerAPI == null ? ModelAbstractAPI.CreateModel() : modelLayerAPI;
            //Test = new RelayCommand(() => ChangeString());
            Buy = new RelayCommand(() => RelayBuy());
        }

        public ObservableCollection<IBook> Books { get; } = new ObservableCollection<IBook>();

        private int selectedIndex = 0;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged();
            }
        }

        private void RelayBuy()
        {
            ModelLayer.BuyBook(Books[selectedIndex]);
        }
    }
}
