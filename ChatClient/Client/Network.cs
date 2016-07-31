using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientBot.Client
{
    class Network
    {
        // const variable
        const int THREAD_COUNT = 4;

        // client class
        class Client
        {
            public Client(Socket socket, int idx)
            {
                m_socket = socket;
                m_idx = idx;
                m_buffer = null;
            }

            private Socket m_socket;
            private int m_idx;
            private byte[] m_buffer;

            public Socket GetSocket() { return m_socket; }
            public int GetIdx() { return m_idx; }
            public byte[] GetBuffer() { return m_buffer; }
        };

        // member variables
        private Thread[] m_threads;

        // member methods
        private void createThread()
        {
            m_threads = new Thread[THREAD_COUNT];
            for( int idx = 0; idx < THREAD_COUNT; ++idx )
            {
                m_threads[idx] = new Thread(threadFunc);
            }
        }
        private void beginThread()
        {
            foreach (var t in m_threads)
            {
                t.Start();
            }
        }
        public void Start()
        {
            createThread();
            beginThread();
        }

        private void threadFunc()
        {
            while( true)
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                try
                {
                    socket.Connect(ipEndPoint);
                    Client client = new Client(socket, ClientBot.Program.botCnt++);

                    AsyncCallback HandleSend = new AsyncCallback(sendProc);
                    sendPacket(client, HandleSend);

                    socket.Disconnect(false);
                }
                catch (Exception /*e*/)
                {

                }
                finally
                {
                    socket.Dispose();
                }
            }
        }
   
        private void sendPacket( Client client, AsyncCallback callbackFunc )
        {
            ShareData.TestPacket noti = new ShareData.TestPacket();
            Socket socket = client.GetSocket();
            int botCnt = client.GetIdx();
            noti.testString = "테스트봇" + botCnt + "번 입니다.";

            byte[] buffer = client.GetBuffer();
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(mStream, noti);
            buffer = mStream.ToArray();

            Console.WriteLine("I'm " + botCnt + "th bot!");
            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, callbackFunc, (object)client);
        }
        private void sendProc(IAsyncResult ar)
        {
            Client client;
            int sendBytes;
            try
            {
                client = (Client)ar.AsyncState; ;
                if (!client.GetSocket().Connected)
                    return;

                sendBytes = client.GetSocket().EndSend(ar);
            }
            catch(Exception e)
            {
                Console.WriteLine("EndSend ERROR!!"+ e.Message);
                return;
            }

            if( sendBytes > 0 )
            {
                // 메시지 전송 성공
            }
        }
    }
}
