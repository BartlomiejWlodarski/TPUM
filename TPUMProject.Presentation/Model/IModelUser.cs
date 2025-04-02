using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Presentation.Model
{
    public interface IModelUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        public IEnumerable<IModelBook> PurchasedBooks { get; }
    }
}
