using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUMProject.Logic.Abstract
{
    
    public interface ILogicUser
    {
        public string Name { get; }
        public decimal Balance { get; set; }
        public IEnumerable<ILogicBook> PurchasedBooks { get; }
    }
}
