using ChatServer.Data.User.UserState;
using System.Net.Sockets;

namespace ChatServer.Data.User
{
    class User
    {
        // constructor
        public User( int ip, int ConnIdx, Socket socket )
        {
            ipAddr = ip;
            Index = ConnIdx;
            clientSocket = socket;
            buffer = new byte[4096];
            userState = null;
        }

        // public property
        public int ipAddr
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public Socket clientSocket
        {
            get;
            set;
        }

        public byte[] buffer
        {
            get;
            set;
        }

        public IUserState userState
        {
            get;
            set;
        }

        // public method
        public void OnClose() { UserContainer.Instance.Pop(this.Index); }
        
    }
}
