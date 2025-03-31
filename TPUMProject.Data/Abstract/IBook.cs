using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public interface IBook
    {
        int Id { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        decimal Price { get; set; }

        public abstract string ToString();
    }
}
