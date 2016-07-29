using ChatServer.ChatServer;
using ChatServer.Data.User;
using System;
using System.Diagnostics;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Launcher launcher = new Launcher();
            launcher.Start();
            // 메인 스레드 정지
            //Thread.Sleep(Timeout.Infinite);
            while (true)
            {
                //ShareData.CommonLogic.JobQueue.Instance.PopQueue();
            }
        }
    }
}
