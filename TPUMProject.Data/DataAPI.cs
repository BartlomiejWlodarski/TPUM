using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Data
{
    internal class DataAPI : AbstractDataAPI
    {
        private readonly IBookRepository _bookRepository = new BookRepository();

        public override IBookRepository BookRepository => _bookRepository;
    }
}
