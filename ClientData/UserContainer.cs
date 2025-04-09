using ClientData.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData
{
    internal class UserContainer : IUserContainer
    {
        public IUser user { get; private set; }

        public event EventHandler<UserChangedEventArgs>? UserChanged;

        public void ChangeUser(IUser user)
        {
            this.user = user;
            UserChanged?.Invoke(this, new UserChangedEventArgs(user));
        }
    }
}
