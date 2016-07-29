using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace ShareData.CommonLogic.Network
{
    public class Network
    {
        System.Diagnostics.Trace trace; //로그 남기는 객체
        const string IP = "127.0.0.1";
        const int PORT = 12345;
        public Network()
        {
            socket = null;
            Buffer = new byte[4096];
            m_connectHandle = new AsyncCallback(connProc);
            m_acceptHandle = new AsyncCallback(acceptProc);
            m_disconnectHandle = new AsyncCallback(disconnectProc);
            m_sendHandle = new AsyncCallback(sendProc);
            m_receiveHandle = new AsyncCallback(receiveProc);
        }

        public Socket socket { get; private set; }
        public byte[] Buffer { get; set; }

        // (client) 동기 서버 접속을 시작 하는 함수
        public void TryConnect()
        {
        }
        // (client) 비동기 서버 접속을 시작 하는 함수
        public void BeginConnect()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(ipep, m_connectHandle, socket);
        }

        #region
        /// <summary>
        /// AsyncCallback
        /// </summary>

        // variable
        private AsyncCallback m_connectHandle;
        private AsyncCallback m_acceptHandle;
        private AsyncCallback m_disconnectHandle;
        private AsyncCallback m_sendHandle;
        private AsyncCallback m_receiveHandle;
        private int serverInitCnt = 0;
        // method
        // (client) 비동기로 서버에 접속할때 호출될 콜백 함수
        public void connProc(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            try
            {
                IPEndPoint ipep = (IPEndPoint)clientSocket.RemoteEndPoint; // 서버 정보

                Debug.WriteLine("서버 접속 성공 IP:" + ipep.Address + " Port:" + ipep.Port);
                clientSocket.EndConnect(ar);
                socket = clientSocket;
            }
            catch (Exception e)
            {
                return;
            }
            socket.Disconnect(true);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close(0);
            // Begin Receive
            //clientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, m_receiveHandle, clientSocket);
        }

        // (server) 유저가 서버 접근할때 호출될 콜백함수
        public void acceptProc(IAsyncResult ar)
        {
            Socket client = socket.EndAccept(ar);

            // 유저가 들어 왔으므로 유저 컨테이너에 넣어주거나 메시지 큐를 만들어 주어야 한다.

            // Begin Accept
            socket.BeginAccept(m_acceptHandle, null);
            serverInitCnt++;

            // Begin Receive
            client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, m_receiveHandle, socket);
        }

        // (server) 유저가 서버 연결 종료시 호출될 콜백함수
        private void disconnectProc(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            //client.EndDisconnect(ar);
            // 유저가 나갔으므로 유저컨테이너에서 빼주기 위한 메시지를 큐에 넣어 주어야 한다.

            //client.Shutdown(SocketShutdown.Both);
            client.Close();
            client.Dispose();
        }

        // (client/server) 비동기로 송신할때 호출될 콜백 함수
        public void sendProc(IAsyncResult ar)
        {
            int sendBytes;
            try
            {
                if (!socket.Connected)
                    return;

                // 메시지 전송
                sendBytes = socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine("EndSend ERROR!!" + e.Message);
                return;
            }

            if (sendBytes > 0)
            {
                // 메시지 전송 성공
            }
        }

        // (client/server) 비동기로 수신할때 호출될 콜백 함수
        public void receiveProc(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                int recvBytes = client.EndReceive(ar);
                if (recvBytes > 0)
                {
                    // receive Buffer Deserialize
                    MemoryStream stream = new MemoryStream(Buffer);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    Object obj = binaryFormatter.Deserialize(stream);

                    // Message Create
                    Message.Message msg = new Message.Message(Message.MessageType.M_PACKET, obj);

                    // Message Queue insert
                    JobQueue.JobQueue.Instance.TryPushBack(msg);

                    // Clear Buffer
                    Array.Clear(Buffer, 0, Buffer.Length);

                    // Begin Receive
                    client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, m_receiveHandle, client);
                }
                else
                {
                    // Begin Disconnect
                    //UserDisconnect(client);
                }
            }
            catch (Exception /*e*/)
            {
                // Begin Disconnect
                //UserDisconnect(client);                
            }
        }
        #endregion

        private void sendPacket(Packet packet)
        {
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(mStream, packet);
            Buffer = mStream.ToArray();
            socket.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, m_sendHandle, (object)this);
        }
        #region Server Side
        /// <summary>
        /// 서버에서만 사용 되는 Network 모듈 입니다.
        /// </summary>
        public void StartServer()
        {
            serverInit();  
        }

        private void serverInit()
        {
            // Socket
            const int port = 12345;
            //IPAddress ipAddr = (IPAddress)Dns.GetHostEntry(Dns.GetHostName()).AddressList.GetValue(1);
            IPAddress ipAddr = IPAddress.Parse(IP);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            // Listen
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(ipEndPoint);
                socket.Listen(100);
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket Error : {0}", e.Message.ToString());
                return;
            }

            // Begin Accept
            socket.BeginAccept(m_acceptHandle, null);
        }

        public void UserDisconnect(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch(Exception e)
            {
                socket.Close(0);
                socket.Dispose();
            }
        }
        #endregion
    }
}
