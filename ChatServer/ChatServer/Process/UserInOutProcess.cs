using ChatServer.Data.User;
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
                UserContainer.Instance.Insert(new User(message.GetCallerIdx(), message.GetSocket()));
            }
            else // 유저가 서버 접속을 끊었을때
            {
                UserContainer.Instance.Pop(message.GetCallerIdx());
            }
        }
    }
}