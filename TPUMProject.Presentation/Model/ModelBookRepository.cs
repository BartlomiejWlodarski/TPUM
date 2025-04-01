using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public class ModelBookRepository
    {
        private IBookService bookService;

        public ModelBookRepository(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public List<ModelBook> GetAllBooks()
        {
            return bookService.GetAvailableBooks().Select(book => new ModelBook(book)).ToList();
        }
    }
}
