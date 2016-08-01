using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    class Launcher
    {
        public enum E_MODE
        {
            STANDALONE = 0,
            BOT = 1,
        };

        private E_MODE m_mode;

        private Dictionary<int, Client> m_Clients; // 봇, 싱글 전부 다 쓰인다

        // Bot Mode
        private int m_clientCount;

        public static int connFailCount = 0;

        public Launcher(E_MODE mode, int clientCount)
        {
            m_mode = mode;
            m_Clients = new Dictionary<int, Client>();

            m_clientCount = 1;
            if (mode == E_MODE.BOT) // 봇 모드 일때에만 초기화
            {
                m_clientCount = clientCount;
            }
            
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

            while (true)
            {
                Do();
            }
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
                    Client client = new Client(m_clientCount);
                    m_Clients.Add(i, client);
                }
                catch (Exception /*e*/)
                {
                }
            }
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

        public void Do()
        {
            var clients = m_Clients;

            if (clients.Count > 1000000)
            {
                Parallel.For(0, clients.Count, (idx) =>
                {
                    var client = clients[idx];
                    client.Do(this);
                });
            }
            else
            {
                for (int i = 0; i < clients.Count; ++i)
                {
                    var client = clients[i];
                    client.Do(this);
                }
            }
        }

        public void SendPacket()
        {
            var clients = m_Clients;
            for (int i = 0; i < clients.Count; ++i)
            {
                var client = clients[i];
                ShareData.TestPacket noti = new ShareData.TestPacket();
                noti.testString = "HELLO WORLD";
                client.SendPacket(noti);
            }
        }
        public void ClientsClear()
        {
            m_Clients.Clear();
        }
    }
}
