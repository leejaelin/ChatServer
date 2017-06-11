using ChatServer.Data.Room;
using ChatServer.Data.User;
using ShareData;
using ShareData.Message;

namespace ChatServer.Process
{
    class UserInOutProcess : IProcess
    {
        #region Singleton
        private static UserInOutProcess m_instance;
        public static UserInOutProcess Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UserInOutProcess();
                return m_instance;
            }
        }
        #endregion

        public UserInOutProcess() { } // 유저가 서버에서 연결/종료 시 최대한 늦은 시점에 유저 컨테이너에서 뺴주기 위한 작업
        ~UserInOutProcess() { }

        public void MsgProcess(User user, Message message)
        {
            if (null == user) // 유저가 서버에 접속 하였을때
            {
                LoginProcess(message);
            }
            else // 유저가 서버 접속을 끊었을때
            {
                LogoutProcess(user, message);
            }
        }

        void LoginProcess(Message message)
        {
            User user = new User(message.GetCallerIdx(), message.GetSocket());
            UserContainer.Instance.Insert(user);

            RoomContainer roomContainer = RoomContainer.Instance;
            if( 0 >= roomContainer.ChatRoomList.Count ) { return; }
            SN_CHATROOMLIST noti = new SN_CHATROOMLIST();
            noti.ChatRoomList = roomContainer.ChatRoomList;
            noti.Type = SN_CHATROOMLIST.E_TYPE.ADD_LIST;
            user.DoSend(noti);
        }

        void LogoutProcess(User user, Message message)
        {
            UserContainer.Instance.Pop(message.GetCallerIdx());

        }
    }
}