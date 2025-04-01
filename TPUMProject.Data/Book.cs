using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class Book : IBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public bool Recommended { get; set; } = false;
        public Genre Genre { get; set ; }

        public override string ToString()
        {
            return $"{Title} - {Author} ({Price:C})";
        }
    }
}
