using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
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

    public interface IBook
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
