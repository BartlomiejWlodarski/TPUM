using System.Collections.ObjectModel;
using System.Diagnostics;
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
            ModelLayer.Changed += HandleBookRepositoryChanged;

            Books = new ObservableCollection<ModelBook>(ModelLayer.ModelRepository.GetAllBooks());
            
            Buy = new RelayCommand(() => RelayBuy());
        }

        private void HandleBookRepositoryChanged(object sender, ModelBookRepositoryChangedEventArgs e)
        {
            switch (e.BookChangedEventType)
            {
                case BookRepositoryChangedEventType.Added:

                    Books.Add(e.AffectedBook);
                        break;
                
                case BookRepositoryChangedEventType.Removed:
                    Books.Remove(Books.Where(book => book.Id == e.AffectedBook.Id).Single());
                        break;
                
                case BookRepositoryChangedEventType.Modified:
                    Books[Books.IndexOf(Books.Where(book => book.Id == e.AffectedBook.Id).Single())] = e.AffectedBook;
                        break;
            }
        }

        private ObservableCollection<ModelBook> books;
        public ObservableCollection<ModelBook> Books 
        { 
            get => books;
            private set
            {
                if (books != value)
                {
                    books = value;
                }
                OnPropertyChanged();
            }
        }

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
            ModelLayer.BuyBook(Books[selectedIndex].Id);
        }
    }
}
