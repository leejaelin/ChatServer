using ShareData.CommonLogic.JobQueue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client
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

        public enum E_MODE
        {
            STANDALONE = 0,
            BOT = 1,
        };

        private E_MODE m_mode;
        public E_MODE Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }
        private Dictionary<int, Client> m_Clients; // 봇, 싱글 전부 다 쓰인다
        private Thread workerThread;
        private bool isWorkerThreadRunning;
        // Bot Mode
        private int m_clientCount;

        public Launcher()
        {
            m_mode = E_MODE.STANDALONE;
            m_Clients = new Dictionary<int, Client>();
            m_clientCount = 1;
            workerThread = new Thread(Jobloop);
            isWorkerThreadRunning = false;
        }

        public void Init(E_MODE mode, int clientCount)
        {
            if (mode == E_MODE.STANDALONE)
                return;

            // 봇 모드 일때에만 설정
            m_mode = mode;
            m_clientCount = clientCount;
        }

        // member methods
        public void Start()
        {   
            helloMessage();
            switch (m_mode)
            {
                case E_MODE.STANDALONE:
                    startStandAlone();
                    break;
                case E_MODE.BOT:
                    startBot();
                    break;
            }

            workerThread.Start();            
        }

        public void Jobloop()
        {
            if (isWorkerThreadRunning)
                return;
            do 
            {
                if (!Do())
                {
                    Thread.Sleep(100);
                }
            }
            while(true);
        }

        private void startStandAlone()
        {
            createClient();
            if (m_Clients[0] != null)
            {
                m_Clients[0].BeginConnect(); // 연결이 안되어 있으면 연결 시작
                return;
            }
        }
        private void startBot()
        {
            createClient();
        }

        private void createClient()
        {
            for (int i = 0; i < m_clientCount; ++i)
            {
                try
                {
                    // Client 생성
                    Client client = new Client(i);
                    m_Clients.Add(i, client);
                }
                catch (Exception /*e*/)
                {
                }
            }
            System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
            tm.Interval = 1000;
            tm.Tick += new System.EventHandler((sender, e) =>
            {
                SendPacket();
            });
            tm.Start();
        }

        private void helloMessage()
        {
            Console.WriteLine("******Client Start******");
            switch (m_mode)
            {
                case E_MODE.BOT:
                    Console.WriteLine("******Bot MODE******");
                    break;
                case E_MODE.STANDALONE:
                    Console.WriteLine("******StandAlone MODE******");
                    break;
            }
            Console.WriteLine("******Bot Count {0}******", m_clientCount);
        }

        public bool Do()
        {
            var clients = m_Clients;
            bool isRet = false;

            //Parallel.For(0, clients.Count, (idx) =>
            //{
            //    var client = clients[idx];
            //    isRet &= client.Do(this);
            //});

            for (int i = 0; i < clients.Count; ++i)
            {
                var client = clients[i];
                isRet &= client.Do(this);
            }

            return isRet;

            //if (clients.Count > 1000000)
            //{
            //    Parallel.For(0, clients.Count, (idx) =>
            //    {
            //        var client = clients[idx];
            //        client.Do(this);
            //    });
            //}
            //else
            //{
            //    for (int i = 0; i < clients.Count; ++i)
            //    {
            //        var client = clients[i];
            //        client.Do(this);
            //    }
            //}
        }

        public void SendPacket()
        {
            var clients = m_Clients;
            for (int i = 0; i < clients.Count; ++i)
            {
                var client = clients[i];
                ShareData.TestPacket noti = new ShareData.TestPacket();
                noti.testString = "안녕.Hello I'm " + i + "th bot!!!";
                client.SendPacket(noti);
            }
        }
        public void ClientsClear()
        {
            m_Clients.Clear();
        }

        public void ReleaseJobLoop()
        {
        }
    }
}
