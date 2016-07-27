using ChatServer.Data.User;
using ShareData;
using ShareData.Message;
using System;
using System.Diagnostics;
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
            if (ar == null || ar.AsyncState == null)
                return;

            User user = (User)ar.AsyncState;

            try
            {
                int recvBytes = user.clientSocket.EndReceive(ar);
                if (recvBytes > 0)
                {
                    MemoryStream stream = new MemoryStream(user.buffer);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    Object obj = binaryFormatter.Deserialize(stream);

                    // Message Create
                    Message msg = new Message(MessageType.M_PACKET, obj);

                    // Message Queue insert
                    JobQueue.GetInstance().TryPushBack(msg);

                    Array.Clear(user.buffer, 0, user.buffer.Length);
                    user.clientSocket.BeginReceive(user.buffer, 0, user.buffer.Length, SocketFlags.None, m_receiveHandle, user);
                }
                else
                {
                    disconnectProc(user);
                }
            }
            catch(Exception /*e*/) 
            {
                disconnectProc(user);
            }
        }

        public void acceptProc(IAsyncResult ar)
        {
            Socket client = m_listener.EndAccept(ar);
            User user = new User(0, UserCnt++, client);

            client.BeginReceive(user.buffer, 0, user.buffer.Length, SocketFlags.None, m_receiveHandle, user);
            m_listener.BeginAccept(m_acceptHandle, null);
        }

        private void disconnectProc( User user )
        {
            UserContainer.Instance.Pop(user.Index);
            user.clientSocket.Shutdown(SocketShutdown.Both);
            user.clientSocket.Close();
        }

        public void StartServer()
        {
            m_receiveHandle = new AsyncCallback(receiveProc);
            m_acceptHandle = new AsyncCallback(acceptProc);

            // Socket
            const int port = 12345;
            IPAddress ipAddr = (IPAddress)Dns.GetHostEntry(Dns.GetHostName()).AddressList.GetValue(1);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            // Listen
            m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                m_listener.Bind(ipEndPoint);
                m_listener.Listen(10);
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket Error : {0}", e.Message.ToString());
                return;
            }

            // Accept
            var UserContainer = Data.User.UserContainer.Instance;
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
            ShareData.TestPacket packet = (ShareData.TestPacket)e.UserToken;
        }
    }
}

