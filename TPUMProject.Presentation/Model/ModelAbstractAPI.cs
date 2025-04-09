using ClientLogic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public class ModelBookRepositoryReplacedEventArgs : EventArgs
    {
        public IEnumerable<ModelBook> modelBooks;

        public ModelBookRepositoryReplacedEventArgs(IEnumerable<ModelBook> modelBooks)
        {
            this.modelBooks = modelBooks;
        }

        public ModelBookRepositoryReplacedEventArgs(LogicBookRepositoryReplacedEventArgs e) { modelBooks = e.BooksReplaced.Select(x =>new ModelBook(x)); }

    }


    public class ModelBookRepositoryChangedEventArgs : EventArgs
    {
        public ModelBook AffectedBook;
        public int BookChangedEventType;

        public ModelBookRepositoryChangedEventArgs(ModelBook affectedBook, int bookChangedEventType)
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

    public class Model
    {
        private AbstractLogicAPI abstractLogic;
        public ModelConnectionService ModelConnectionService { get; private set; }
        public ModelBookRepository ModelBookRepository { get; private set; }

        public Action? ModelAllBooksUpdated;

        public Model(AbstractLogicAPI? abstractLogic)
        {
            this.abstractLogic = abstractLogic ?? AbstractLogicAPI.Create();

            ModelBookRepository = new ModelBookRepository(this.abstractLogic.GetBookService());
            ModelConnectionService = new ModelConnectionService(this.abstractLogic.GetConnectionService());
        }


        public async Task SellBook(int id)
        {
            await abstractLogic.GetBookService().SellBook(id);
        }

        public void GetUser(string userName)
        {
            abstractLogic.GetBookService().GetUser(userName);
        }
    }
}
