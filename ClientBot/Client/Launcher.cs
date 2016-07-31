using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientBot.Client
{
    class Launcher
    {
        public enum E_MODE
        {
            STANDALONE = 0,
            BOT = 1,
        };

        private E_MODE m_mode;
        private int m_botCount;
        private Dictionary<int, Client> m_botClients;
        public static int connFailCount = 0;

        public Launcher(E_MODE mode, int botCount)
        {
            m_mode = mode;
            m_botCount = botCount;

            if (mode == E_MODE.BOT) // 봇 모드 일때에만 초기화
            {
                m_botClients = new Dictionary<int, Client>();
            }
        }

        // member methods
        public void Start()
        {
            createBot();
            helloMessage();

            while(true)
            {
                Do();
            }
        }

        private void createBot()
        {
            for (int i = 0; i < m_botCount; ++i)
            {
                try
                {
                    // Client 생성
                    Client client = new Client(m_botCount);
                    m_botClients.Add(i, client);
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
            Console.WriteLine("******Bot Count {0}******", m_botCount);
        }

        public void Do()
        {
            var clients = m_botClients;

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

        public void ClientsClear()
        {
            m_botClients.Clear();
        }
    }
}
