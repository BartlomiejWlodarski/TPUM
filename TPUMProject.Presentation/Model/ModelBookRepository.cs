using ClientLogic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public class ModelBookRepository
    {
        private IBookService bookService;

        public Action? ModelAllBooksUpdated;

        public ModelBookRepository(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public List<ModelBook> GetAllBooks()
        {
            return bookService.GetAvailableBooks().Select(book => new ModelBook(book)).ToList();
        }
    }
}
