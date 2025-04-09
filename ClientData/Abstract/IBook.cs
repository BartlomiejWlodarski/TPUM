namespace ClientData.Abstract
{
    public enum Genre
    {
        Uncategorized,
        Action,
        Drama,
        Romance,
        Mystery,
        Science_Fiction
    }

    public interface IBook : ICloneable
    {
        int Id { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        Genre Genre { get; set; }
        decimal Price { get; set; }
        bool Recommended { get; set; }

        public abstract string ToString();
    }
}
