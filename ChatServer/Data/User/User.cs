using ChatServer.Data.User.UserState;
using ShareData;
using System.Net.Sockets;

namespace ChatServer.Data.User
{
    class User
    {
        // constructor
        public User( uint connIdx, Socket socket )
        {
            Index = connIdx;
            ClientSocket = socket;
            State = LoginingState.Instance;
        }

        // member variable

        private uint index;
        public uint Index
        {
            get { return index; }
            set { index = value; }
        }

        private Socket clientSocket;
        public Socket ClientSocket
        {
            get { return clientSocket; }
            set { clientSocket = value; }
        }

        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        private IUserState state;
        public IUserState State
        {
            get { return state; }
            set
            {
                state = value;
                state.OnEntry();
            }
        }

        // public method
        public void DoSend(Packet packet)
        {
            ChatServer.Instance.SendPacket(Index, packet);
        }
        public void OnClose() { UserContainer.Instance.Pop(this.Index); }
    }
}
