

namespace ClinetAPI
{
    [Serializable]
    public abstract class ServerCommand
    {
        public string Header;

        protected ServerCommand(string header)
        {
            Header = header;
        }
    }

    [Serializable]
    public struct BookDTO
    {
        public int Id;
        public string Title;
        public string Author;
        public decimal Price;
        public bool Recommended;
        public int Genre;

        public BookDTO(int Id, string title, string author, decimal Price, bool Recommended, int Genre)
        {
            this.Id = Id;
            this.Title = title;
            this.Author = author;
            this.Price = Price;
            this.Recommended = Recommended;
            this.Genre = Genre;
        }
    }
}
