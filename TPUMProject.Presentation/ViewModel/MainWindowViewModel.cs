using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TPUMProject.Presentation.Model;
using ViewModel;

namespace TPUMProject.Presentation.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
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

        public MainWindowViewModel(ModelAbstractAPI modelLayerAPI)
        {
            ModelLayer = modelLayerAPI == null ? ModelAbstractAPI.CreateModel() : modelLayerAPI;
            ModelLayer.ConnectionService.OnConnectionStateChanged += OnConnectionStateChange;
            ModelLayer.ConnectionService.OnError += OnConnectionStateChange;

            ModelLayer.Changed += HandleBookRepositoryChanged;
            ModelLayer.UserChanged += HandleUserChanged;
            ModelLayer.ModelBookRepositoryReplaced += HandleBookRepositoryReplaced;
            //ModelLayer.ModelAllBooksUpdated += GetAllBooks;
            ModelLayer.ModelRepository.ModelAllBooksUpdated += GetAllBooks;

            OnConnectionStateChange();

            Books = new AsyncObservableCollection<ViewModelBook>(ModelLayer.ModelRepository.GetAllBooks().Select(x => new ViewModelBook(x)));
            BooksShow = Books;
            //User = ModelLayer.User;

            Buy = new RelayCommand(() => RelayBuy());
            ChangeList = new RelayCommand(() => RelayChangeList());
        }

        private void GetAllBooks()
        {
            RefreshBooks();
            //_booksShow.Clear();
            //_booksShow.AddRange(ModelLayer.ModelRepository.GetAllBooks());
        }

        private void RefreshBooks()
        {
            _booksShow.Clear();
            if (CatalogActive)
            {
                _booksShow.AddRange(ModelLayer.ModelRepository.GetAllBooks().Select(x => new ViewModelBook(x)));
                //ShoppingButtonContent = _shopList;
                //ButtonVisibility = Visibility.Hidden;
            }
            else
            {
                //BooksShow = Books;
                //ShoppingButtonContent = _userList;
               // ButtonVisibility = Visibility.Visible;
            }

        }
        public async Task CloseConnection()
        {
            if (ModelLayer.ConnectionService.IsConnected())
            {
                await ModelLayer.ConnectionService.Disconnect();
            }
        }

        private void OnConnectionStateChange()
        {
            bool connectionState = ModelLayer.ConnectionService.IsConnected();
            ConnectionStateString = connectionState ? "Connected" : "Disconnected";

            if (connectionState)
            {
                ModelLayer.GetUser("Marcin");
                ModelLayer.GetBooks();
            }
            else
            {
                Task.Run(() => ModelLayer.ConnectionService.Connect(new Uri("ws://localhost:42069/")));
            }
        }

        private void HandleBookRepositoryReplaced(object sender, ModelBookRepositoryReplacedEventArgs e)
        {
            books.Clear();
            //books.AddRange(e.modelBooks);
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
                    books.Add(new ViewModelBook(e.AffectedBook)); //Added
                        break;
                
                case 1:
                    books.Remove(Books.Where(book => book.Id == e.AffectedBook.Id).Single()); //Removed
                    break;
                
                case 2:
                    if (books.Count == 0) return;
                    int index = books.IndexOf(books.Where(book => book.Id == e.AffectedBook.Id).Single());
                    if (index < 0 || index >= books.Count) return;
                    books[index] = new ViewModelBook(e.AffectedBook); // Modified
                        break;
            }
        }

        private AsyncObservableCollection<ViewModelBook> books;
        public AsyncObservableCollection<ViewModelBook> Books 
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
        private AsyncObservableCollection<ViewModelBook> _booksShow;
        public AsyncObservableCollection<ViewModelBook> BooksShow
        {
            get => _booksShow;
            private set
            {
                if (_booksShow != value)
                {
                    _booksShow = value;
                    OnPropertyChanged("BooksShow");
                }
            }
        }

        private string _connectionStateString;

        public string ConnectionStateString
        {
            get => _connectionStateString;
            set
            {
                if(value != _connectionStateString)
                {
                    _connectionStateString = value;
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

        public event PropertyChangedEventHandler? PropertyChanged;

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
                //BooksShow = new AsyncObservableCollection<IModelBook>(User.PurchasedBooks);
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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
