using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public ModelGenre Genre { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; }
        public SolidColorBrush backcolor { get; set; }

        public ModelBook(IBook book)
        {
            Id = book.Id;
            Title = book.Title;
            Author = book.Author;
            Genre = (ModelGenre)book.Genre;
            Price = book.Price;
            Recommended = book.Recommended;
            if (Recommended)
            {
                backcolor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255,215,0));
            }
            else
            {
                backcolor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 240, 240));
            }
        }
        public ModelBook(int id, string title, string author, ModelGenre genre, decimal price, bool recommended)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Price = price;
            Recommended = recommended;
            if (Recommended)
            {
                backcolor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 215, 0));
            }
            else
            {
                backcolor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 210, 220));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
