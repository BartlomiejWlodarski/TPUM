using ClientData.Abstract;
using ClientLogic.Abstract;

namespace ClientLogic
{
    internal class LogicBook : ILogicBook
    {
        public int Id { get ; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public LogicGenre Genre { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; }

        public LogicBook(IBook book)
        {
            this.Id = book.Id;
            this.Title = book.Title;
            this.Author = book.Author;
            this.Genre = (LogicGenre)book.Genre;
            this.Price = book.Price;
            this.Recommended = book.Recommended;
        }
    }
}
