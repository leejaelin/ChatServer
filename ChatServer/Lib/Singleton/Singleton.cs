using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Lib.Singleton
{
    class Singleton<T> 
        where T : new()
    {
        private static T m_instance;

        public Singleton()
        {
        }

        public static T GetInstance()
        {
            if (m_instance == null)
                m_instance = new T();
            return m_instance;
        }
    }
}
