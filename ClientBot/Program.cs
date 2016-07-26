using ClientBot.Client;
using System.Threading;

namespace ClientBot
{
    class Program
    {
        public static int botCnt = 0;

        static void Main(string[] args)
        {
            Network ntw = new Network();
            ntw.Start();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
