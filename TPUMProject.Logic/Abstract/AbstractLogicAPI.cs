using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;

namespace TPUMProject.Logic.Abstract
{
    public abstract class AbstractLogicAPI
    {
        public abstract IBookService BookService { get; }

        public static AbstractLogicAPI Create()
        {
            AbstractDataAPI dataAPI = AbstractDataAPI.Create();
            return new LogicAPI(dataAPI);
        }

        public abstract void GetRandomRecommendedBook();
    }
}
