using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Lib.Singleton;
using User = ChatServer.Data.User.User;

namespace ChatServer.Data.User
{
    class UserContainer : Singleton< UserContainer >
    {
        public UserContainer()
        {
            this.userContainer = new Dictionary<int, User>();
            this.userContainer.Clear();
        }
        public void Insert( User user )
        {
            this.userContainer.Add(user.GetIndex(), user);
        }
        public User Find( int idx )
        {
            User user;
            this.userContainer.TryGetValue(idx, out user);
            return user != null ? user : null;

        }

        private Dictionary<int, User> userContainer;

    }
}
