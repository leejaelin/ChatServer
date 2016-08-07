using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Lib.Singleton;
using User = ChatServer.Data.User.User;
using System.Collections.Concurrent;

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

        private ConcurrentDictionary<uint, User> conUserContainer;
        public ConcurrentDictionary<uint, User> ConUserContainer
        {
            get { return conUserContainer; }
        }

        public UserContainer()
        {
            conUserContainer = new ConcurrentDictionary<uint, User>();
            conUserContainer.Clear();
        }

        public void Insert( User user )
        {
            if (conUserContainer.ContainsKey(user.Index))
            {
                User tmpUser;
                conUserContainer.TryRemove(user.Index, out tmpUser);
            }
            conUserContainer.TryAdd(user.Index, user);
        }

        public User Find( uint idx )
        {
            User user;

            conUserContainer.TryGetValue(idx, out user);
            return user != null ? user : null;

        }

        public void Pop( uint idx )
        {
            User user;
            if( conUserContainer.ContainsKey(idx) )
                conUserContainer.TryRemove(idx, out user);
        }
    }
}
