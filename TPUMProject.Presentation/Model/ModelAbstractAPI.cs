using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.Presentation.Model
{
    public abstract class ModelAbstractAPI
    {
        public static ModelAbstractAPI CreateModel(AbstractLogicAPI? logicAPI = default)
        {
            return new ModelLayer(logicAPI ?? AbstractLogicAPI.Create());
        }

        internal class ModelLayer : ModelAbstractAPI {
            private readonly AbstractLogicAPI _logicLayer;
            public ModelLayer(AbstractLogicAPI logicLayer)
            {
                _logicLayer = logicLayer;
            }
        }
    }
}
