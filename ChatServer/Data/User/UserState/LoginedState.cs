
namespace ChatServer.Data.User.UserState
{
    class LoginedState : IUserState
    {
        public LoginedState() { }
        ~LoginedState() { }

        #region Singleton
        private static LoginedState m_instance;
        public static LoginedState Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new LoginedState();
                return m_instance;
            }
        }
        #endregion

        public void OnEntry()
        {

        }
    }
}
