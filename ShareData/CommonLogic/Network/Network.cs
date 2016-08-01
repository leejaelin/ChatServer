using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Collections.Generic;
using ShareData.Message;

namespace ShareData.CommonLogic.Network
{
    public class AsyncObject
    {
        public AsyncObject(Socket socket, uint idx = 0)
        {
            this.idx = idx;
            this.socket = socket;
        }
        ~AsyncObject() { }

        private uint idx; // 서버에서 필요 하다. conn, receive시 유저 구분을 위한 용도
        public uint Idx { get { return idx; } }
        private Socket socket;
        public Socket Socket { get { return socket; } }
    }

    public class Network
    {
        //System.Diagnostics.Trace trace; //로그 남기는 객체
        #region const variable
        const string IP = "10.61.201.17";
        const int PORT = 12345;
        const int BUFFER_SIZE = 4096;
        #endregion

        #region Constructor / Destructor
        // constructor
        public Network()
        {
            socket = null;
            SendBuffer = new byte[BUFFER_SIZE];
            ReceiveBuffer = new byte[BUFFER_SIZE];
            JobQueue = new JobQueue.JobQueue();

            m_connectHandle = new AsyncCallback(connProc);
            m_acceptHandle = new AsyncCallback(acceptProc);
            m_disconnectHandle = new AsyncCallback(disconnectProc);
            m_sendHandle = new AsyncCallback(sendProc);
            m_receiveHandle = new AsyncCallback(receiveProc);

            // server only
            userSocketList = new Dictionary<uint, Socket>();
            socketIdx = 0;

            // client only
            m_connected = false;
        }
        // destructor
        ~Network()
        {
            socket = null;
            SendBuffer = null;
            ReceiveBuffer = null;
            JobQueue = null;

            m_connectHandle = null;
            m_acceptHandle = null;
            m_disconnectHandle = null;
            m_sendHandle = null;
            m_receiveHandle = null;

            // server only
            userSocketList = null;
        }
        #endregion

        #region variable
        // member variable
        private AsyncCallback m_connectHandle;
        private AsyncCallback m_acceptHandle;
        private AsyncCallback m_disconnectHandle;
        private AsyncCallback m_sendHandle;
        private AsyncCallback m_receiveHandle;

        public Socket socket { get; set; }
        public byte[] SendBuffer { get; set; }
        public byte[] ReceiveBuffer { get; set; }
        public JobQueue.JobQueue JobQueue { get; set; }

        public virtual void Alive() { }

        // server only variable
        private Dictionary<uint, Socket> userSocketList; // 서버에서 가지는 유저 소켓 리스트
        public uint socketIdx; // 서버에서 가지는 소켓의 개수

        // Client only variable
        private bool m_connected; // socket의 connect를 비교하면 멀티스레드에서의 처리 순서때문에 문제가 발생하여, 따로 관리해준다.
        public bool GetConnectedToServer()
        {
            return m_connected;
        }
        #endregion

        // (client) 동기 서버 접속을 시작 하는 함수
        public void TryConnect()
        {
        }
        // (client) 비동기 서버 접속을 시작 하는 함수
        public void BeginConnect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //**socket.BeginConnect(IP, PORT, m_connectHandle, socket);
            socket.BeginConnect(IP, PORT, m_connectHandle, new AsyncObject(socket));
        }
        
        #region AsyncCallback
        // method
        // (client) 비동기로 서버에 접속할때 호출될 콜백 함수
        public void connProc(IAsyncResult ar)
        {
            //**Socket serverSocket = (Socket)ar.AsyncState;
            AsyncObject serverObject = (AsyncObject)ar.AsyncState;
            Socket serverSocket = serverObject.Socket;
            if (!serverSocket.Connected)
            {
                BeginConnect();
                return;
            }
            try
            {
                IPEndPoint ipep = (IPEndPoint)serverSocket.RemoteEndPoint; // 서버 정보
                Debug.WriteLine("서버 접속 성공 IP:" + ipep.Address + " Port:" + ipep.Port);
                serverSocket.EndConnect(ar);
                socket = serverSocket;

                // Begin Receive
                serverSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, m_receiveHandle, serverObject); //**
                m_connected = true;
            }
            catch( SocketException /*e*/ ) // 연결중에 서버가 다운되거나 해서 소켓이 끊어지는 케이스
            {
                BeginConnect();
                return;
            }
        }

        // (server) 유저가 서버 접근할때 호출될 콜백함수
        public void acceptProc(IAsyncResult ar)
        {
            Socket client = null;
            try
            {
                client = socket.EndAccept(ar);
                Console.WriteLine("유저 접속 : " + client.RemoteEndPoint.ToString());
            }
            catch(SocketException e)
            {
                Console.WriteLine(e.ErrorCode + " " + e.Message);
                return;
            }

            uint userSocketIdx = socketIdx++;
            userSocketList.Add(userSocketIdx, client);

            JobQueue.TryPushBack(new Message.Message(userSocketIdx, MessageType.M_USER_IN_OUT, new object(), client));

            Alive();

            // Begin Accept
            socket.BeginAccept(m_acceptHandle, null);
                        
            // Begin Receive
            client.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, m_receiveHandle, new AsyncObject(client, userSocketIdx)); //**
        }

        // (server) 유저가 서버 연결 종료시 호출될 콜백함수
        private void disconnectProc(IAsyncResult ar)
        {
            //**Socket clientSocket = (Socket)ar.AsyncState;
            AsyncObject clientObj = (AsyncObject)ar.AsyncState;
            Socket clientSocket = clientObj.Socket;

            userSocketList.Remove(clientObj.Idx);
            JobQueue.TryPushBack(new Message.Message(clientObj.Idx, MessageType.M_USER_IN_OUT, null, clientSocket));

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket.Dispose();
        }

        // (client/server) 비동기로 송신할때 호출될 콜백 함수
        public void sendProc(IAsyncResult ar)
        {
            //**Socket sendSocket = (Socket)ar.AsyncState;
            AsyncObject sendObj = (AsyncObject)ar.AsyncState;
            Socket sendSocket = sendObj.Socket;

            if (sendSocket == null || !sendSocket.Connected)
                return;

            int sendBytes;
            try
            {
                // 메시지 전송;
                sendBytes = sendSocket.EndSend(ar);
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
            //**Socket receiveSocket = (Socket)ar.AsyncState;
            AsyncObject receiveObj = (AsyncObject)ar.AsyncState;
            Socket receiveSocket = receiveObj.Socket;

            if (!receiveSocket.Connected)
                return;

            try
            {
                int recvBytes = receiveSocket.EndReceive(ar);
                if (recvBytes > 0)
                {
                    // receive Buffer Deserialize
                    MemoryStream stream = new MemoryStream(ReceiveBuffer);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    Object obj = binaryFormatter.Deserialize(stream);

                    // Message Create
                    Message.Message msg = new Message.Message(receiveObj.Idx, Message.MessageType.M_PACKET, obj, receiveSocket);

                    // Message Queue insert
                    JobQueue.TryPushBack(msg);

                    // Clear Buffer
                    Array.Clear(ReceiveBuffer, 0, ReceiveBuffer.Length);

                    // Begin Receive
                    receiveSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, m_receiveHandle, receiveObj); //**

                    return;
                }
                else
                {
                }
            }
            catch (ObjectDisposedException /*e*/)
            {
            }
            catch (Exception /*e*/)
            {
            }
            UserDisconnect(receiveObj);
        }
        #endregion

        // (client->server) 일방적으로 연결 종료 처리
        public void Close()
        {
            if(socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close(0);
                socket.Dispose();
            }
            socket = null;
            m_connected = false;
        }

        // (client->server)
        protected void sendPacket(Packet packet)
        {
            // 클라이언트는 서버와 단일 연결 되어 있으므로 바로 보낸다.
            if (!socket.Connected)
                return;

            send(this.socket, packet);
        }

        // (server->client)
        protected void sendPacket(uint destSocketIdx, Packet packet)
        {
            // 서버는 목적지 클라이언트의 소켓을 찾아 전송 한다.
            if ( !userSocketList.ContainsKey(destSocketIdx) )
                return;

            Socket destSocket = userSocketList[destSocketIdx];
            send(destSocket, packet);
        }

        private bool send(Socket socket, Packet packet)
        {
            try
            {
                MemoryStream mStream = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(mStream, packet);
                SendBuffer = mStream.ToArray();
                socket.BeginSend(SendBuffer, 0, SendBuffer.Length, SocketFlags.None, m_sendHandle, new AsyncObject(socket));
            }
            catch (SocketException /*e*/)
            {
                return false;
            }
            return true;
        }
        #region Server Side
        public void StartServer()
        {
            serverInit();  
        }

        private void serverInit()
        {
            // Address, EndPoint
            const int port = 12345;
            IPAddress ipAddr = IPAddress.Parse(IP);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            // Socket Create, Bind, Listen
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

        public void UserDisconnect(AsyncObject asyncObj)
        {
            Socket socket = asyncObj.Socket;
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close(0);
                socket.Dispose();
            }
            userSocketList.Remove(asyncObj.Idx);
            JobQueue.TryPushBack(new Message.Message(asyncObj.Idx, MessageType.M_USER_IN_OUT, null, socket));
            socket = null;          
        }
        #endregion
    }
}
