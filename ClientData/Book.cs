using ClientData.Abstract;

namespace ClientData
{
    internal class Book : IBook , ICloneable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; } = false;
        public Genre Genre { get; set ; }

        public object Clone()
        {
            Book clone = (Book)MemberwiseClone();
            clone.Title = string.Copy(Title);
            clone.Author = string.Copy(Author);
            return clone;
        }

        public override string ToString()
        {
            return $"{Title} - {Author} ({Price:C})";
        }
    }
}
