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

        public UserContainer()
        {
            userContainer = new Dictionary<uint, User>();
            userContainer.Clear();
        }

        public void Insert( User user )
        {
            if (userContainer.ContainsKey(user.Index))
                userContainer.Remove(user.Index);
            userContainer.Add(user.Index, user);
        }
        public User Find( uint idx )
        {
            User user;
            userContainer.TryGetValue(idx, out user);
            return user != null ? user : null;
        }
        public void Pop( uint idx )
        {
            try
            {
                userContainer.Remove(idx);
            }
            catch(Exception /*e*/)
            {
                return;
            }
        }

        private Dictionary<uint, User> userContainer;
    }
}
