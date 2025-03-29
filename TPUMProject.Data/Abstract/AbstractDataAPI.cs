using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Data.Abstract
{
    public abstract class AbstractDataAPI
    {
        public abstract IBookRepository BookRepository { get; }

        public static AbstractDataAPI Create()
        {
            return new DataAPI();
        }
    }
}
