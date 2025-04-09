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
        private object itemLock = new object();

        private Model.Model ModelLayer;
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


        private AsyncObservableCollection<ViewModelBook> booksShow;
        public AsyncObservableCollection<ViewModelBook> BooksShow
        {
            get => booksShow;
            private set
            {
                if (booksShow != value)
                {
                    booksShow = value;
                    OnPropertyChanged("BooksShow");
                }
            }
        }

        public MainWindowViewModel()
        {
            ModelLayer = new Model.Model(null);
            ModelLayer.ModelConnectionService.OnConnectionStateChanged += OnConnectionStateChange;
            ModelLayer.ModelConnectionService.OnError += OnConnectionStateChange;
            ModelLayer.ModelConnectionService.Logger += Log;
            ModelLayer.ModelConnectionService.OnMessage += OnMessage;

            ModelLayer.ModelBookRepository.ModelAllBooksUpdated += HandleOnBooksUpdated;
            ModelLayer.ModelBookRepository.UserChanged += HandleUserChanged;
            ModelLayer.ModelBookRepository.BookRepositoryChanged += HandleBookRepositoryChanged;

            OnConnectionStateChange();

            booksShow = new AsyncObservableCollection<ViewModelBook>(ModelLayer.ModelBookRepository.GetAllBooks().Select(book => new ViewModelBook(book)));


            booksShow.Add(new ViewModelBook(new ModelBook(1, "kek", "kek", ModelGenre.Uncategorized, 20, false)));
            booksShow.Add(new ViewModelBook(new ModelBook(2, "lol", "lol", ModelGenre.Uncategorized, 25, false)));
            booksShow.Add(new ViewModelBook(new ModelBook(3, "uwu", "uwu", ModelGenre.Uncategorized, 30, false)));

            //BooksShow = Books;

            Buy = new RelayCommand(() => RelayBuy());
            ChangeList = new RelayCommand(() => RelayChangeList());
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
                    books.Add(new ViewModelBook(e.AffectedBook)); //Added
                    break;

                case 1:
                    books.Remove(Books.Where(book => book.Id == e.AffectedBook.Id).Single()); //Removed
                    break;

                case 2:
                    if (booksShow.Count == 0) return;
                    ViewModelBook? viewModelBook = booksShow.Where(book => book.Id == e.AffectedBook.Id).Single();
                    if(viewModelBook == null) return;
                    int index = booksShow.IndexOf(viewModelBook);
                    if (index < 0 || index >= booksShow.Count) return;
                    booksShow[index] = new ViewModelBook(e.AffectedBook); // Modified
                    break;
            }
        }

        private void RefreshBooks()
        {
            //booksShow.Clear();
            if (CatalogActive)
            {
                //booksShow.AddRange(ModelLayer.ModelBookRepository.GetAllBooks().Select(x => new ViewModelBook(x)));
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
            if (ModelLayer.ModelConnectionService.IsConnected())
            {
                await ModelLayer.ModelConnectionService.Disconnect();
            }
        }

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
            if(selectedIndex >= 0 && selectedIndex < Books.Count)
            {
                Task.Run(async () => await ModelLayer.SellBook(Books[selectedIndex].Id));
            }
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



        private const string _shopList = "Show avaiable books list";
        private const string _userList = "Show user books list";
    }
}
