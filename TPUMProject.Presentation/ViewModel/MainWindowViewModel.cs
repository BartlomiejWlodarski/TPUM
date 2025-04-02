using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TPUMProject.Presentation.Model;

namespace TPUMProject.Presentation.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ModelAbstractAPI ModelLayer;
        private bool CatalogActive = true;
        private IModelUser _user;
        public IModelUser User
        {
            get => _user;
            set
            {
                if(value != _user)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }

        private const string _shopList = "Show avaiable books list";
        private const string _userList = "Show user books list";

        private string _shoppingButtonContent = _userList;
        public string ShoppingButtonContent
        {
            get => _shoppingButtonContent;
            set
            {
                _shoppingButtonContent = value;
                OnPropertyChanged();
            }
        }
        public ICommand Buy { get; set; }
        public ICommand ChangeList { get; set; }

        public MainWindowViewModel() : this(null)
        { }

        internal MainWindowViewModel(ModelAbstractAPI modelLayerAPI)
        {
            ModelLayer = modelLayerAPI == null ? ModelAbstractAPI.CreateModel() : modelLayerAPI;
            ModelLayer.Changed += HandleBookRepositoryChanged;
            ModelLayer.UserChanged += HandleUserChanged;

            Books = new ObservableCollection<IModelBook>(ModelLayer.ModelRepository.GetAllBooks());
            User = ModelLayer.User;
            BooksShow = Books;
            
            Buy = new RelayCommand(() => RelayBuy());
            ChangeList = new RelayCommand(() => RelayChangeList());
        }

        private void HandleUserChanged(object sender, ModelUserChangedEventArgs e)
        {
            User = e.user;
        }

        private void HandleBookRepositoryChanged(object sender, ModelBookRepositoryChangedEventArgs e)
        {
            switch (e.BookChangedEventType)
            {
                case 0:
                    Books.Add(e.AffectedBook); //Added
                        break;
                
                case 1:
                    Books.Remove(Books.Where(book => book.Id == e.AffectedBook.Id).Single()); //Removed
                    break;
                
                case 2:
                    Books[Books.IndexOf(Books.Where(book => book.Id == e.AffectedBook.Id).Single())] = e.AffectedBook; // Modified
                        break;
            }
        }

        private ObservableCollection<IModelBook> books;
        public ObservableCollection<IModelBook> Books 
        { 
            get => books;
            private set
            {
                if (books != value)
                {
                    books = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<IModelBook> _booksShow;
        public ObservableCollection<IModelBook> BooksShow
        {
            get => _booksShow;
            private set
            {
                if (_booksShow != value)
                {
                    _booksShow = value;
                    OnPropertyChanged();
                }
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
            if(selectedIndex >= 0 && selectedIndex < Books.Count)ModelLayer.BuyBook(Books[selectedIndex].Id);
        }

        private Visibility _buttonVisibility = Visibility.Visible;
        public Visibility ButtonVisibility
        {
            get => _buttonVisibility;
            set
            {
                _buttonVisibility = value;
                OnPropertyChanged();
            }
        }

        private void RelayChangeList()
        {
            if(CatalogActive)
            {
                CatalogActive = false;
                BooksShow = new ObservableCollection<IModelBook>(User.PurchasedBooks);
                ShoppingButtonContent = _shopList;
                ButtonVisibility = Visibility.Hidden;
            } 
            else
            {
                CatalogActive = true;
                BooksShow = Books;
                ShoppingButtonContent = _userList;
                ButtonVisibility = Visibility.Visible;
            }
        }
    }
}
