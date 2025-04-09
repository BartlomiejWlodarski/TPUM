using ClientLogic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public class ModelBookRepository
    {
        private IBookService bookService {  get; set; }

        public event EventHandler<ModelBookRepositoryChangedEventArgs>? BookRepositoryChanged;
        public event EventHandler<ModelUserChangedEventArgs>? UserChanged;
        public event Action? ModelAllBooksUpdated;

        public ModelBookRepository(IBookService bookService)
        {
            this.bookService = bookService;

            bookService.LogicAllBooksUpdated += () => ModelAllBooksUpdated?.Invoke();
        }

        private void HandleBookRepositoryChanged(LogicBookRepositoryChangedEventArgs e)
        {
            BookRepositoryChanged?.Invoke(this, new ModelBookRepositoryChangedEventArgs(e));
        }

        private void HandleUserChanged(LogicUserChangedEventArgs e)
        {
            UserChanged?.Invoke(this,new ModelUserChangedEventArgs(e));
        }

        public void RequestUpdate()
        {
            bookService.RequestUpdate();
        }

        public List<ModelBook> GetAllBooks()
        {
            return bookService.GetAllBooks().Select(book => new ModelBook(book)).ToList();
        }

        public ModelBook? GetBookById(int id)
        {
            ILogicBook? result = bookService.GetBookByID(id);
            return result == null ? null : new ModelBook(result);
        }
    }
}
