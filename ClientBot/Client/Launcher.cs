using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        }

        private void createBot()
        {
            for (int i = 0; i < m_botCount; ++i)
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                try
                {
                    // Client 생성
                    Client client = new Client(m_botCount);
                    client.BeginConnect();
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
            if (m_botClients.Count > 1000000)
            {
                Parallel.For(0, m_botClients.Count, (idx) =>
                {
                    var client = m_botClients[idx];
                    client.Do();
                });
            }
            else
            {
                for (int i = 0; i < m_botClients.Count; ++i)
                {
                    var client = m_botClients[i];
                    client.Do();
                }
            }
        }
    }
}
