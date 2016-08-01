namespace ChatServer
{
    class Launcher
    {
        public Launcher()
        {
        }
        ~Launcher() { }

        
        public void Start()
        {
            ChatServer chatServer = ChatServer.Instance;
            chatServer.StartServer();
            chatServer.JobLoop();
        }
    }
}
