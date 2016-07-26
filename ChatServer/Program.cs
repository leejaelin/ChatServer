using ChatServer.ChatServer;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Network ntw = new Network();
            ntw.StartServer();

            // 메인 스레드 정지
            //Thread.Sleep(Timeout.Infinite);
            while(true)
            {
                JobQueue.GetInstance().itQueue();
            }
        }
    }
}
