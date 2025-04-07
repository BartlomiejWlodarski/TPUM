namespace ClientLogic.Abstract
{
    
    public interface ILogicUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        public IEnumerable<ILogicBook> PurchasedBooks { get; }
    }
}
