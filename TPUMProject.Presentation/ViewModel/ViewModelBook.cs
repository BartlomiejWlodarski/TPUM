using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TPUMProject.Presentation.Model;

namespace TPUMProject.Presentation.ViewModel
{
    public class ViewModelBook : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public ModelGenre Genre { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; }
        public SolidColorBrush backcolor { get; set; }

        public void ChangeBackcolor()
        {
            if (Recommended)
            {
                backcolor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 215, 0));
            }
            else
            {
                backcolor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 240, 240));
            }
        }

        public ViewModelBook(ModelBook book)
        {
            Id = book.Id;
            Title = book.Title;
            Author = book.Author;
            Genre = book.Genre;
            Price = book.Price;
            Recommended = book.Recommended;
            ChangeBackcolor();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
