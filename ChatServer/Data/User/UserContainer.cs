using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Lib.Singleton;
using User = ChatServer.Data.User.User;

namespace ChatServer.Data.User
{
    class UserContainer
    {
        public UserContainer()
        {
            this.userContainer = new Dictionary<int, User>();
            this.userContainer.Clear();
        }

        #region Singleton
        private static UserContainer m_instance;
        public static UserContainer Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UserContainer();
                return m_instance;
            }
        }
        #endregion

        public void Insert( User user )
        {
            this.userContainer.Add(user.Index, user);
        }
        public User Find( int idx )
        {
            User user;
            this.userContainer.TryGetValue(idx, out user);
            return user != null ? user : null;
        }
        public void Pop( int idx )
        {
            this.userContainer.Remove(idx);
        }

        private Dictionary<int, User> userContainer;

    }
}
