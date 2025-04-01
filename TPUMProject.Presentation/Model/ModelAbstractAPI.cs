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

    public class ModelBookRepositoryChangedEventArgs : EventArgs
    {
        public ModelBook AffectedBook;
        public BookRepositoryChangedEventType BookChangedEventType;

        public ModelBookRepositoryChangedEventArgs(ModelBook affectedBook, BookRepositoryChangedEventType bookChangedEventType)
        {
            AffectedBook = affectedBook;
            BookChangedEventType = bookChangedEventType;
        }

        public ModelBookRepositoryChangedEventArgs(LogicBookRepositoryChangedEventArgs e) 
        {
            AffectedBook = new ModelBook(e.AffectedBook);
            BookChangedEventType = e.ChangedEventType;
        }
    }

    public abstract class ModelAbstractAPI
    {
        public event EventHandler<ModelBookRepositoryChangedEventArgs>? Changed;

        public static ModelAbstractAPI CreateModel(AbstractLogicAPI? logicAPI = default)
        {
            return new ModelLayer(logicAPI ?? AbstractLogicAPI.Create("Marcin",100));
        }

        public abstract bool BuyBook(int id);

        public ModelBookRepository ModelRepository;

        internal class ModelLayer : ModelAbstractAPI {

            private readonly AbstractLogicAPI _logicLayer;

            public ModelLayer(AbstractLogicAPI logicLayer)
            {
                _logicLayer = logicLayer;
                ModelRepository = new ModelBookRepository(_logicLayer.BookService);
            }

            public override bool BuyBook(int id)
            {
                //return _logicLayer. Attempt to buy a book here
                return _logicLayer.BookService.BuyBook(id);
            }

            public void HandleBookRepositoryChanged(object sender, LogicBookRepositoryChangedEventArgs e)
            {
                Changed?.Invoke(this, new ModelBookRepositoryChangedEventArgs(e));
            }
        }
    }
}
