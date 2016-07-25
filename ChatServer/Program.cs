
using ChatServer.ChatServer;

namespace ChatServer
{
    class Program
    {
        static int UserCnt = 0;

        static void Main(string[] args)
        {
            Network ntw = new Network();
            ntw.StartServer();
            while(true)
            {

            }
        }
    }
}
