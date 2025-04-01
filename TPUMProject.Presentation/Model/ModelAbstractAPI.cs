using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Data.Abstract;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public abstract class ModelAbstractAPI
    {

        public static ModelAbstractAPI CreateModel(AbstractLogicAPI? logicAPI = default)
        {
            return new ModelLayer(logicAPI ?? AbstractLogicAPI.Create());
        }

        public abstract bool BuyBook(IBook book);


        internal class ModelLayer : ModelAbstractAPI {
            private readonly AbstractLogicAPI _logicLayer;
            public ModelLayer(AbstractLogicAPI logicLayer)
            {
                _logicLayer = logicLayer;
            }

            public override bool BuyBook(IBook book)
            {
                //return _logicLayer. Attempt to buy a book here
                return true;
            }
        }
    }
}
