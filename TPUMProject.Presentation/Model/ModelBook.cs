using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Presentation.Model
{
    public enum ModelGenre
    {
        Uncategorized,
        Action,
        Drama,
        Romance,
        Mystery,
        Science_Fiction
    }
    public class ModelBook : INotifyPropertyChanged
    {
        

        int Id { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        ModelGenre Genre { get; set; }
        decimal Price { get; set; }
        bool Recommended { get; set; }

        public ModelBook(IBook book)
        {
            Id = book.Id;
            Title = book.Title;
            Author = book.Author;
            Genre = (ModelGenre)book.Genre;
            Price = book.Price;
            Recommended = book.Recommended;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
