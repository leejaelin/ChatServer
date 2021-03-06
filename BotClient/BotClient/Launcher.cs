﻿using System.Collections.Generic;
using System.Threading;

namespace BotClient.BotClient
{
    class Launcher
    {
        #region Singleton
        private static Launcher m_instance;
        public static Launcher Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Launcher();
                return m_instance;
            }
        }
        #endregion

        private Thread workerThread;
        public Thread WorkerThread { get { return workerThread; } set { workerThread = value; } }

        private bool isWorkerThread;
        public bool IsWorkerThread { get {return isWorkerThread;} set{isWorkerThread = value;}}
        
        private int botClientMax;
        public int BotClientMax { get { return botClientMax; } set { botClientMax = value; } }
        private Dictionary<int, BotClient> botClients; // 봇 컨테이너

        private AutoResetEvent mainThreadEventHandler;
        public AutoResetEvent MainThreadEventHandler { get { return mainThreadEventHandler; } set { mainThreadEventHandler = value; } }

        public Launcher()
        {
            botClients = new Dictionary<int, BotClient>();
            MainThreadEventHandler = new AutoResetEvent(true);
        }

        public void Init( int botCount )
        {
            botClientMax = botCount;
        }

        public void JobLoop()
        {
            do
            {
                if( !Do() )
                {
                    MainThreadEventHandler.WaitOne();
                }
            } while (IsWorkerThread);

            WorkerThread = null;
        }

        public void Start()
        {
            if (WorkerThread != null)
                return;

            createBot();            
            createThread();
            botTimer();
        }

        private void createBot()
        {
            for (int i = 0; i < botClientMax; ++i)
            {
                // Client 생성
                BotClient botClient = new BotClient(i);
                botClients.Add(i, botClient);
            }
        }

        private void createThread()
        {
            IsWorkerThread = true;
            WorkerThread = new Thread(JobLoop);
            workerThread.Start();
        }

        public bool Do()
        {
            bool isRet = true;
            //Parallel.For(0, botClients.Count, (idx) =>
            //{
            //    isRet &= botClients[idx].Do();
            //});
            for (int idx = 0; idx < botClients.Count; ++idx )
            {
                isRet &= botClients[idx].Do();
            }
            return isRet;
        }

        public void SendPacket()
        {
            for (int i = 0; i < botClients.Count; ++i)
            {
                var client = botClients[i];
                ShareData.CQ_CHAT noti = new ShareData.CQ_CHAT();
                noti.MsgStr = "안녕.Hello I'm " + i + "th bot!!!";
                client.SendPacket(noti);
            }
        }

        public void BotClose()
        {
            for( int idx = 0; idx < botClients.Count; idx++ )
            {
                botClients[idx].socket.Close();
            }
            TerminateProcess();
        }

        public void TerminateProcess()
        {
            IsWorkerThread = false;
            MainThreadEventHandler.Set();
            botClients.Clear(); // 봇 리스트 정리
        }
        
        private void botTimer()
        {
            System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
            tm.Interval = 2000;
            tm.Tick += new System.EventHandler((sender, e) =>
            {
                SendPacket();
            });
            tm.Start();
        }
    }
}
