using ClientLogic.Abstract;

namespace TPUMProject.Presentation.Model
{

    public class ModelBookRepositoryChangedEventArgs : EventArgs
    {
        public IModelBook AffectedBook;
        public int BookChangedEventType;

        public ModelBookRepositoryChangedEventArgs(IModelBook affectedBook, int bookChangedEventType)
        {
            AffectedBook = affectedBook;
            BookChangedEventType = bookChangedEventType;
        }

        public ModelBookRepositoryChangedEventArgs(LogicBookRepositoryChangedEventArgs e) 
        {
            AffectedBook = new ModelBook(e.AffectedBook);
            BookChangedEventType = e.ChangedEventType;
        }
    }

    public class ModelUserChangedEventArgs : EventArgs
    {
        public IModelUser user;

        public ModelUserChangedEventArgs(ILogicUser user)
        {
            this.user = new ModelUser(user);
        }

        public ModelUserChangedEventArgs(LogicUserChangedEventArgs e)
        {
            this.user = new ModelUser(e.user);
        }
    }

    public abstract class ModelAbstractAPI
    {
        public event EventHandler<ModelBookRepositoryChangedEventArgs>? Changed;
        public event EventHandler<ModelUserChangedEventArgs>? UserChanged;
        public Action? ModelAllBooksUpdated;

        public static ModelAbstractAPI CreateModel(AbstractLogicAPI? logicAPI = default)
        {
            return new ModelLayer(logicAPI ?? AbstractLogicAPI.Create());
        }

        public abstract void BuyBook(int id);
        public abstract void GetUser(string username);
        public abstract void GetBooks();

        public IModelBookRepository ModelRepository;
        public IModelConnectionService ConnectionService;

        internal class ModelLayer : ModelAbstractAPI {

            private readonly AbstractLogicAPI _logicLayer;

            public ModelLayer(AbstractLogicAPI logicLayer)
            {
                _logicLayer = logicLayer;
                _logicLayer.BookService.BookRepositoryChanged += HandleBookRepositoryChanged;
                _logicLayer.BookService.UserChanged += HandleUserChanged;
                ModelRepository = new ModelBookRepository(_logicLayer.BookService);
                ConnectionService = new ModelConnectionService(_logicLayer.GetConnectionService());
                _logicLayer.BookService.LogicAllBooksUpdated += () => ModelAllBooksUpdated?.Invoke();
            }

            public override void BuyBook(int id)
            {
                _logicLayer.BookService.BuyBook(id);
            }

            public override void GetBooks()
            {
                _logicLayer.BookService.GetBooks();
            }

            public override void GetUser(string username)
            {
                _logicLayer.BookService.GetUser(username);
            }

            private void HandleBookRepositoryChanged(object sender, LogicBookRepositoryChangedEventArgs e)
            {
                Changed?.Invoke(this, new ModelBookRepositoryChangedEventArgs(e));
            }

            private void HandleUserChanged(object sender, LogicUserChangedEventArgs e)
            {
                UserChanged?.Invoke(this, new ModelUserChangedEventArgs(e));
            }
        }
    }
}
