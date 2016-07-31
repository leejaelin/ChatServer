using ChatServer.ChatServer.Process;
using ChatServer.Data.User;
using ShareData.CommonLogic.Network;
using ShareData.Message;
using System.Collections.Generic;

namespace ChatServer.ChatServer
{
    class ChatServer : Network
    {
        public ChatServer()
        {
            processList = new Dictionary<MessageType, IProcess>();
            init();
        }
        ~ChatServer() {}

        // member variable
        private Dictionary<MessageType, IProcess> processList;

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
                    continue;

                MessageProc(message);
            }
        }

        public void MessageProc( Message message )
        {
            User user = UserContainer.Instance.Find(message.GetCallerIdx());
            processList[message.GetMessageType()].MsgProcess(user, message);
        }
    }
}
