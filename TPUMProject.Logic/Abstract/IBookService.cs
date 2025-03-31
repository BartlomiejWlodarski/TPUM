using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Logic.Abstract
{
    public interface IBookService
    {
        IEnumerable<IBook> GetAvailableBooks();
        void AddNewBook(string title, string author, decimal price);
        bool BuyBook(int id);
        IBook GetRandomRecommendedBook(IBook currentRecommendedBook);
    }
}
