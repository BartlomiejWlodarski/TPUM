
namespace TPUMProject.Presentation.Model
{
    public interface IModelUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        public IEnumerable<IModelBook> PurchasedBooks { get; }
    }
}
