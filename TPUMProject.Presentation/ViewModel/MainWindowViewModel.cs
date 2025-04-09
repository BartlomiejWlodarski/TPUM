using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TPUMProject.Presentation.Model;
using ViewModel;

namespace TPUMProject.Presentation.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Model.Model ModelLayer;
        private bool CatalogActive = true;
        private IModelUser _user;
        private AsyncObservableCollection<ViewModelBook> books;
        private AsyncObservableCollection<ViewModelBook> booksShow;
        private string _shoppingButtonContent = _userList;
        private string _connectionStateString;
        private Visibility _buttonVisibility = Visibility.Visible;
        private int selectedIndex = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            ModelLayer = new Model.Model(null);

            ModelLayer.ModelConnectionService.OnConnectionStateChanged += () => RunOnUI(OnConnectionStateChange);
            ModelLayer.ModelConnectionService.OnError += () => RunOnUI(OnConnectionStateChange);
            ModelLayer.ModelConnectionService.Logger += (message) => RunOnUI(() => Log(message));
            ModelLayer.ModelConnectionService.OnMessage += (message) => RunOnUI(() => OnMessage(message));

            ModelLayer.ModelBookRepository.ModelAllBooksUpdated += () => RunOnUI(HandleOnBooksUpdated);
            ModelLayer.ModelBookRepository.UserChanged += (sender, e) => RunOnUI(() => HandleUserChanged(sender, e));
            ModelLayer.ModelBookRepository.BookRepositoryChanged += (sender, e) => RunOnUI(() => HandleBookRepositoryChanged(sender, e));

            Books = new AsyncObservableCollection<ViewModelBook>();
            BooksShow = Books;

            OnConnectionStateChange();

            Buy = new RelayCommand(() => RelayBuy());
            ChangeList = new RelayCommand(() => RelayChangeList());
        }

        private void RunOnUI(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
                action();
            else
                Application.Current.Dispatcher.Invoke(action);
        }

        private void OnConnectionStateChange()
        {
            bool connectionState = ModelLayer.ModelConnectionService.IsConnected();
            ConnectionStateString = connectionState ? "Connected" : "Disconnected";

            if (connectionState)
            {
                ModelLayer.GetUser("Marcin");
                ModelLayer.ModelBookRepository.RequestUpdate();
            }
            else
            {
                Task.Run(() => ModelLayer.ModelConnectionService.Connect(new Uri("ws://localhost:42069/")));
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void OnMessage(string message)
        {
            Log($"New message: {message}");
        }

        private void HandleOnBooksUpdated()
        {
            RefreshBooks();
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
                    books.Add(new ViewModelBook(e.AffectedBook)); // Added
                    break;
                case 1:
                    books.Remove(Books.FirstOrDefault(book => book.Id == e.AffectedBook.Id)); // Removed
                    break;
                case 2:
                    var item = Books.FirstOrDefault(book => book.Id == e.AffectedBook.Id);
                    if (item != null)
                    {
                        int index = Books.IndexOf(item);
                        //if (index >= 0) books[index] = new ViewModelBook(e.AffectedBook); // Modified
                    }
                    break;
            }
            if (CatalogActive)
            {
                RunOnUI(() => BooksShow = new AsyncObservableCollection<ViewModelBook>(Books));
            }
        }

        private void RefreshBooks()
        {
            Books.Clear();
            Books.AddRange(ModelLayer.ModelBookRepository.GetAllBooks().Select(x => new ViewModelBook(x)));
            //BooksShow = Books;
        }

        public async Task CloseConnection()
        {
            if (ModelLayer.ModelConnectionService.IsConnected())
            {
                await ModelLayer.ModelConnectionService.Disconnect();
            }
        }

        private void RelayBuy()
        {
            if (selectedIndex >= 0 && selectedIndex < Books.Count)
            {
                Task.Run(async () => await ModelLayer.SellBook(Books[selectedIndex].Id));
            }
        }

        private void RelayChangeList()
        {
            if (CatalogActive)
            {
                CatalogActive = false;
                BooksShow = new AsyncObservableCollection<ViewModelBook>(User.PurchasedBooks.Select(x => new ViewModelBook(x)));
                ShoppingButtonContent = _shopList;
                ButtonVisibility = Visibility.Hidden;
            }
            else
            {
                CatalogActive = true;
                BooksShow = new AsyncObservableCollection<ViewModelBook>(Books);
                ShoppingButtonContent = _userList;
                ButtonVisibility = Visibility.Visible;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // === Public Properties ===

        public IModelUser User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public AsyncObservableCollection<ViewModelBook> BooksShow
        {
            get => booksShow;
            private set
            {
                if (booksShow != value)
                {
                    booksShow = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public string ConnectionStateString
        {
            get => _connectionStateString;
            set
            {
                if (value != _connectionStateString)
                {
                    _connectionStateString = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility ButtonVisibility
        {
            get => _buttonVisibility;
            set
            {
                _buttonVisibility = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged();
            }
        }

        private const string _shopList = "Show avaiable books list";
        private const string _userList = "Show user books list";
    }
}
