using ChatServer.Data.User;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ChatServer.ChatServer
{
    class Network
    {
        private Socket m_listener;
        private AsyncCallback m_receiveHandle;
        private AsyncCallback m_acceptHandle;
        
        private static int UserCnt = 0;

        public Network() { }
        ~Network() { }

        public void receiveProc(IAsyncResult ar)
        {
            IAsyncResult iar = ar;
            User user = (User)iar.AsyncState;
            int recvBytes = user.GetBuffer().Length;
            if (recvBytes > 0)
            {
                MemoryStream stream = new MemoryStream(user.GetBuffer());
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Object obj = null;
                try
                {
                    obj = binaryFormatter.Deserialize(stream);
                }
                catch( Exception e )
                {
                    string vstring = e.Message;
                }
                ShareData.TestPacket req = (ShareData.TestPacket)obj;

                Console.WriteLine(req.testString);
                Array.Clear(user.GetBuffer(), 0, user.GetBuffer().Length);
            }
            user.GetSocket().BeginReceive(user.GetBuffer(), 0, user.GetBuffer().Length, SocketFlags.None, m_receiveHandle, user);
        }

        public void acceptProc(IAsyncResult ar)
        {
            Socket client = m_listener.EndAccept(ar);
            var tmp = client.AddressFamily;
            User user = new User(0, UserCnt++, client);
            Data.User.UserContainer.GetInstance().Insert(user);

            client.BeginReceive(user.GetBuffer(), 0, user.GetBuffer().Length, SocketFlags.None, m_receiveHandle, user);
        }

        public void StartServer()
        {
            m_receiveHandle = new AsyncCallback(receiveProc);
            m_acceptHandle = new AsyncCallback(acceptProc);

            // Socket
            const int port = 12345;
            IPAddress ipAddr = Dns.Resolve("localhost").AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            // Listen
            m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                m_listener.Bind(ipEndPoint);
                m_listener.Listen(100);
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket Error : {0}", e.Message.ToString());
                return;
            }

            // Accept
            var UserContainer = Data.User.UserContainer.GetInstance();
            Thread AcceptThread = new Thread(new ThreadStart(
                () =>
                {
                    m_listener.BeginAccept(m_acceptHandle, null);
                }));
            AcceptThread.Start();
        }

        private static void RecvPacket(object sender, SocketAsyncEventArgs e)
        {
            Socket client = (Socket)sender;
            ShareData.TestPacket packet =
                (ShareData.TestPacket)e.UserToken;
        }
    }
}
