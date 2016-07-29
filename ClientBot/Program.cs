using ClientBot.Client;
using System.Threading;

namespace ClientBot
{
    class Program
    {
        const int BOT_COUNT = 8;

        static void Main(string[] args)
        {
            // Launcher 를 통해 Client 생성 및 소켓 연결 수행
            Launcher launcher = new Launcher(Launcher.E_MODE.BOT, BOT_COUNT);
            launcher.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
