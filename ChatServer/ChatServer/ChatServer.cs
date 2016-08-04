using ChatServer.Process;
using ChatServer.Data.User;
using ShareData.CommonLogic.Network;
using ShareData.Message;
using System.Collections.Generic;
using ShareData;
using System.Threading;

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

        public ChatServer()
        {
            processList = new Dictionary<MessageType, IProcess>();
            mainThreadEventHandler = new AutoResetEvent(true);
            init();
        }
        ~ChatServer() {}

        // member variable
        private Dictionary<MessageType, IProcess> processList;
        private AutoResetEvent mainThreadEventHandler;

        private void init()
        {
            processList.Add(MessageType.M_PACKET, PacketProcess.Instance);
            processList.Add(MessageType.M_PROCEDURE, ProcedureProcess.Instance);
            processList.Add(MessageType.M_USER_IN_OUT, UserInOutProcess.Instance);
        }

        public void JobLoop()
        {
            while(true)
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

        public void Broadcast(Packet packet) 
        { 
            foreach( var user in UserContainer.Instance.ConUserContainer.Values )
                user.DoSend(packet);
        }

        public void SendPacket( uint destIdx, Packet packet )
        {
            sendPacket(destIdx, packet);
        }

        public override void Alive()
        {
            mainThreadEventHandler.Set();
        }
    }
}
