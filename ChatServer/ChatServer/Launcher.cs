namespace ChatServer.ChatServer
{
    class Launcher
    {
        public Launcher()
        {
            m_chatServer = new ChatServer();
        }
        ~Launcher() { }

        private ChatServer m_chatServer;
        
        public void Start()
        {
            m_chatServer.StartServer();
            m_chatServer.JobLoop();
        }
    }
}
