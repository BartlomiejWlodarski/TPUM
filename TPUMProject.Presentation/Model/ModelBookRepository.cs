﻿using TPUMProject.Logic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public class ModelBookRepository : IModelBookRepository
    {
        private IBookService bookService;

        public ModelBookRepository(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public IEnumerable<IModelBook> GetAllBooks()
        {
            return bookService.GetAvailableBooks().Select(book => new ModelBook(book));
        }
    }
}
