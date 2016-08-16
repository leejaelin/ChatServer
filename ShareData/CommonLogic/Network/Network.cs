using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Collections.Generic;
using ShareData.Message;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Text;

namespace ShareData.CommonLogic.Network
{
    public class AsyncObject
    {
        public AsyncObject(Socket socket, uint idx = 0)
        {
            this.idx = idx;
            this.socket = socket;
            this.networkBuffer = new Buffer((int)Network.BUFFER_SIZE);
        }
        ~AsyncObject() { }

        private uint idx; // 서버에서 필요 하다. conn, receive시 유저 구분을 위한 용도
        public uint Idx { get { return idx; } }
        public Buffer networkBuffer { get; set; }
        
        private Socket socket;
        public Socket Socket { get { return socket; } }
    }

    public class Buffer
    {
        public Buffer(int buffSize)
        {
            SendBuffer = new byte[buffSize];
            RecvBuffer = new byte[buffSize];
        }
        ~Buffer()
        {
            SendBuffer = null;
            RecvBuffer = null;
        }
        public byte[] SendBuffer { get; set; }
        public byte[] RecvBuffer { get; set; }
    }

    public class Network
    {
        //System.Diagnostics.Trace trace; //로그 남기는 객체
        #region const variable
        public static string IP = "127.0.0.1";
        public static int PORT = 12345; 
        public static int BUFFER_SIZE = 4096;
        #endregion

        #region Constructor / Destructor
        public Network( E_SOCKET_MODE mode ) // constructor
        {
            socket = null;
            JobQueue = new JobQueue.JobQueue();
            socketMode = mode;

            m_connectHandle = new AsyncCallback(connProc);
            m_acceptHandle = new AsyncCallback(acceptProc);
            m_disconnectHandle = new AsyncCallback(disconnectProc);
            m_sendHandle = new AsyncCallback(sendProc);
            m_receiveHandle = new AsyncCallback(receiveProc);

            // server only
            userSocketList = new ConcurrentDictionary<uint, AsyncObject>();
            socketIdx = 0;
        }

        ~Network() // destructor
        {
            socket = null;
            //ReceiveBuffer = null;
            //NetworkBuffer = null;
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
        public JobQueue.JobQueue JobQueue { get; set; }

        public enum E_SOCKET_MODE
        {
            SERVER = 0,
            CLIENT,
        };
        private E_SOCKET_MODE socketMode;

        // server only variable
        private ConcurrentDictionary<uint, AsyncObject> userSocketList; // 서버에서 가지는 유저 소켓 리스트
        public uint socketIdx; // 서버에서 가지는 소켓의 개수

        #endregion

        // (client) 비동기 서버 접속을 시작 하는 함수
        public void BeginConnect()
        {
            try 
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.BeginConnect(IP, PORT, m_connectHandle, new AsyncObject(socket));
            }
            catch(SocketException /*e*/)
            { 
                return;
            }
        }
        
        #region AsyncCallback
        // method
        // (client) 비동기로 서버에 접속할때 호출될 콜백 함수
        public void connProc(IAsyncResult ar)
        {
            AsyncObject serverObject = (AsyncObject)ar.AsyncState;
            Buffer serverBuffer = serverObject.networkBuffer;
            Socket serverSocket = serverObject.Socket;
            try
            {
                IPEndPoint ipep = (IPEndPoint)serverSocket.RemoteEndPoint; // 서버 정보
                Debug.WriteLine("서버 접속 성공 IP:" + ipep.Address + " Port:" + ipep.Port);
                serverSocket.EndConnect(ar);
                socket = serverSocket;

                OnLogin();
                AwakeThread();

                // Begin Receive
                serverSocket.BeginReceive(serverBuffer.RecvBuffer, 0, serverBuffer.RecvBuffer.Length, SocketFlags.None, m_receiveHandle, serverObject); 
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
            Socket clientSocket = null;
            try
            {
                clientSocket = socket.EndAccept(ar);
                Console.WriteLine("유저 접속 : " + clientSocket.RemoteEndPoint.ToString());
            }
            catch(SocketException /*e*/)
            {
                //Console.WriteLine(e.ErrorCode + " " + e.Message);
                return;
            }

            uint userSocketIdx = socketIdx++;
            userSocketList.TryAdd(userSocketIdx, new AsyncObject(clientSocket, userSocketIdx) );

            JobQueue.TryPushBack(new Message.Message(userSocketIdx, MessageType.M_USER_IN_OUT, null, clientSocket));

            AwakeThread();

            // Begin Accept
            socket.BeginAccept(m_acceptHandle, null);

            try 
            {
                // Begin Receive
                AsyncObject newClientAsyncObject = new AsyncObject(clientSocket, userSocketIdx);
                Buffer clientBuffer = newClientAsyncObject.networkBuffer;
                clientSocket.BeginReceive(clientBuffer.RecvBuffer, 0, clientBuffer.RecvBuffer.Length, SocketFlags.None, m_receiveHandle, newClientAsyncObject);
            }
            catch( SocketException /*e*/)
            {
            }
        }

        // (server) 유저가 서버 연결 종료시 호출될 콜백함수
        private void disconnectProc(IAsyncResult ar)
        {
            AsyncObject clientObj = (AsyncObject)ar.AsyncState;
            AsyncObject disconnectClient;

            userSocketList.TryRemove(clientObj.Idx, out disconnectClient);
            JobQueue.TryPushBack(new Message.Message(disconnectClient.Idx, MessageType.M_USER_IN_OUT, null, disconnectClient.Socket));
            disconnectSocket(disconnectClient.Socket);
        }

        private void disconnectSocket( Socket TargetSocket )
        {
            try
            {
                TargetSocket.Shutdown(SocketShutdown.Both);
                TargetSocket.Close();
                TargetSocket.Dispose();
            }
            catch (SocketException /*e*/)
            {
            }
        }
        // (client/server) 비동기로 송신할때 호출될 콜백 함수
        public void sendProc(IAsyncResult ar)
        {
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
            catch(SocketException /*e*/)
            {
                return;
            }
            catch (ObjectDisposedException /*e*/)
            {
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
            AsyncObject receiveObj = null;// = (AsyncObject)ar.AsyncState;
            Socket receiveSocket = null;// = receiveObj.Socket;
            Buffer receiveBuffer = null;
            try
            {
                receiveObj = (AsyncObject)ar.AsyncState;
                receiveSocket = receiveObj.Socket;
                receiveBuffer = receiveObj.networkBuffer;

                if (!receiveSocket.Connected)
                    return;

                int recvBytes = receiveSocket.EndReceive(ar);
                if (recvBytes > 0)
                {
                    // receive Buffer Deserialize
                    MemoryStream stream = new MemoryStream(receiveBuffer.RecvBuffer);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    Object obj = (Object)binaryFormatter.Deserialize(stream);

                    Packet packet = (Packet)obj;
                    if( packet.GetPacketSize() <= recvBytes )
                    {
                        // Message Create
                        Message.Message msg = new Message.Message(receiveObj.Idx, Message.MessageType.M_PACKET, obj, receiveSocket);

                        // Message Queue insert
                        JobQueue.TryPushBack(msg);

                        // jobQueue를 위한 스레드를 살린다
                        AwakeThread();

                        // Clear Buffer
                        Array.Clear(receiveBuffer.RecvBuffer, 0, receiveBuffer.RecvBuffer.Length);
                    }

                    // Begin Receive
                    //receiveSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, m_receiveHandle, receiveObj);
                }
                else
                {
                }
            }
            catch (ObjectDisposedException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch(SerializationException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch(SocketException /*e*/)
            {
                // 상대방이 연결을 종료했다.(그냥 끊어 버림)
                UserDisconnect(receiveObj);
                return;
            }

            receiveSocket.BeginReceive(receiveBuffer.RecvBuffer, 0, receiveBuffer.RecvBuffer.Length, SocketFlags.None, m_receiveHandle, receiveObj);
        }
        #endregion

        // (client->server)
        protected void sendPacket(Packet packet)
        {
            // 클라이언트는 서버와 단일 연결 되어 있으므로 바로 보낸다.
            if (socket == null || !socket.Connected)
            {
                socket = null;
                return;
            }

            send(socket, packet);
        }

        // (server->client)
        protected void sendPacket(uint destSocketIdx, Packet packet)
        {
            // 서버는 목적지 클라이언트의 소켓을 찾아 전송 한다.
            try 
            {
                if (!userSocketList.ContainsKey(destSocketIdx))
                    return;
                
                AsyncObject destClientObj = userSocketList[destSocketIdx];
                send(destClientObj, packet);
            }
            catch(Exception /*e*/)
            {
            }            
        }

        private bool send(AsyncObject asyncObject, Packet packet)
        {
            Socket sendSocket = asyncObject.Socket;
            Buffer sendBuffer = asyncObject.networkBuffer;
            try
            {
                if (sendSocket == null || !sendSocket.Connected)
                    return false;

                MemoryStream mStream = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(mStream, packet);
                sendBuffer.SendBuffer = mStream.ToArray();
                sendSocket.BeginSend(sendBuffer.SendBuffer, 0, sendBuffer.SendBuffer.Length, SocketFlags.None, m_sendHandle, new AsyncObject(sendSocket));
            }
            catch (SerializationException /*e*/)
            {
                return false;
            }
            catch (SocketException /*e*/)
            {
                return false;
            }
            catch (ArgumentException /*e*/)
            {
                return false;
            }
            catch (Exception /*e*/ )
            {
                return false;
            }
            return true;
        }

        private bool send(Socket socket, Packet packet)
        {
            try
            {
                MemoryStream mStream = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(mStream, packet);

                AsyncObject sendAsyncObject = new AsyncObject(socket);
                Buffer NetworkBuffer = sendAsyncObject.networkBuffer;
                NetworkBuffer.SendBuffer = mStream.ToArray();
                socket.BeginSend(NetworkBuffer.SendBuffer, 0, NetworkBuffer.SendBuffer.Length, SocketFlags.None, m_sendHandle, sendAsyncObject);
            }
            catch(SerializationException /*e*/)
            {
                return false;
            }
            catch (SocketException /*e*/)
            {
                return false;
            }
            catch(ArgumentException /*e*/)
            {
                return false;
            }
            catch( Exception /*e*/ )
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
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Network.PORT);

            // Socket Create, Bind, Listen
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(ipEndPoint);
                socket.Listen(100);
            }
            catch (SocketException /*e*/)
            {
                return;
            }

            Console.WriteLine("[ Listen ]");
            Console.WriteLine("[ IP : {0}, PORT : {1} ]", ipEndPoint.Address.ToString(), ipEndPoint.Port);

            // Begin Accept
            socket.BeginAccept(m_acceptHandle, null);
        }

        // (client -> server) 연결 강제 종료
        public void UserDisconnect(AsyncObject asyncObj)
        {
            Socket targetSocket = asyncObj.Socket;
            try
            {
                disconnectSocket(targetSocket);
            }
            catch(SocketException /*e*/)
            {
            }
            catch(Exception /*e*/)
            {
            }
            finally
            {
                AsyncObject tmpAsyncObject;
                userSocketList.TryRemove(asyncObj.Idx, out tmpAsyncObject);
                JobQueue.TryPushBack(new Message.Message(asyncObj.Idx, MessageType.M_USER_IN_OUT, null, targetSocket));
                AwakeThread();
            }
        }

        public virtual void AwakeThread() { } // Job Thread 재개
        public virtual void OnLogin() { } // (client -> server 로그인 성공)
        public virtual void OnClose() { } // (server에서 유저 로그아웃 처리)
        #endregion
    }
}
