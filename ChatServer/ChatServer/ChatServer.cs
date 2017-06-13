using ChatServer.Process;
using ChatServer.Data.User;
using ShareData.CommonLogic.Network;
using ShareData.Message;
using System.Collections.Generic;
using ShareData;
using System.Threading;
using ShareData.CommonLogic.Log;

namespace ChatServer
{    
    class ChatServer : Network
    {
        #region Singleton
        private static ChatServer m_instance;
        public static ChatServer Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ChatServer();
                return m_instance;
            }
        }
        #endregion

        public ChatServer() : base(Network.E_SOCKET_MODE.SERVER)
        {
            processList = new Dictionary<MessageType, IProcess>();
            init();
            mainThreadEventHandler = new AutoResetEvent(true);
            logMaker = new LogMaker();
        }
        ~ChatServer() {}

        // member variable
        private Dictionary<MessageType, IProcess> processList;
        private AutoResetEvent mainThreadEventHandler;
        public static LogMaker logMaker;

        private void init()
        {
            processList.Add(MessageType.M_PACKET, PacketProcess.Instance);
            processList.Add(MessageType.M_PROCEDURE, ProcedureProcess.Instance);
            processList.Add(MessageType.M_USER_IN_OUT, UserInOutProcess.Instance);
        }

        public void JobLoop()
        {
            logMaker.File("SERVER START UP!!!!");
            System.Net.IPHostEntry host = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName());
            string myIp = host.AddressList[0].ToString();
            logMaker.File("[ IP : " + myIp + " / PORT : " + Network.PORT + " ]");

            while (true)
            {
                Message message = JobQueue.TryPopFront();
                if (message == null)
                {
                    mainThreadEventHandler.WaitOne();
                    continue;
                }
                MessageProc(message);
            }
        }

        public void MessageProc( Message message )
        {
            User user = UserContainer.Instance.Find(message.GetCallerIdx());
            processList[message.GetMessageType()].MsgProcess(user, message);
        }

        public void SendPacket( uint destIdx, Packet packet )
        {
            sendPacket(destIdx, packet);
        }

        public override void AwakeThread()
        {
            mainThreadEventHandler.Set();
        }
    }
}
