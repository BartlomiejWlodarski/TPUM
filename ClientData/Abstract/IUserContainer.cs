using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData.Abstract
{
    public interface IUserContainer
    {
        IUser user { get; }
        public event EventHandler<UserChangedEventArgs>? UserChanged;

        void ChangeUser(IUser user);


    }
}
