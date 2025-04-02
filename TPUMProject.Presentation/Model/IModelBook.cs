using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

    public interface IModelBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public ModelGenre Genre { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; }
        public SolidColorBrush backcolor { get; set; }
    }
}
