namespace ClientLogic.Abstract
{
    public enum LogicGenre
    {
        Uncategorized,
        Action,
        Drama,
        Romance,
        Mystery,
        Science_Fiction
    }
    public interface ILogicBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public LogicGenre Genre { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; }
    }
}
