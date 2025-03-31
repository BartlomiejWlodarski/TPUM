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
    public class MainWindowViewModel : IDisposable, INotifyPropertyChanged
    {
        private ModelAbstractAPI ModelLayer;
        private bool Disposed = false;
        private string _testString = string.Empty;

        //public ICommand Previous { get; set; }
        //public ICommand Next { get; set; }

        public string TestString
        {
            get { return _testString; }
            set
            {
                _testString = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel() : this(null)
        { }

        internal MainWindowViewModel(ModelAbstractAPI modelLayerAPI)
        {
            ModelLayer = modelLayerAPI == null ? ModelAbstractAPI.CreateModel() : modelLayerAPI;
            //Test = new RelayCommand(() => ChangeString());
        }

        public ObservableCollection<IBook> Books { get; } = new ObservableCollection<IBook>();

        private int selectedIndex = 0;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    
                }
                Disposed = true;
            }
        }
        public void Dispose()
        {
            if(Disposed) throw new ObjectDisposedException(nameof(MainWindowViewModel));
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
