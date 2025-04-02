namespace TPUMProject.Presentation.Model
{
    public interface IModelBookRepository
    {
        IEnumerable<IModelBook> GetAllBooks();
    }
}
