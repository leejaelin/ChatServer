using ShareData;
using ShareData.CommonLogic.Network;
using ShareData.Message;
using BotClient.BotClient.Process;
using BotClient.BotClient.Room;

namespace BotClient.BotClient
{
    class BotClient : Network
    {
        public BotClient( int idx ) : base(Network.E_SOCKET_MODE.CLIENT)
        {
            m_botIdx = idx;
        }

        private int m_botIdx;
        public int BotIdx { get { return m_botIdx; } set { m_botIdx = value; } }

        public RoomContainer RoomList { get; set; }

        public void SendPacket( ShareData.Packet packet )
        {
            sendPacket(packet);
        }

        public bool Do()
        {
            if( socket == null )
            {
                BeginConnect("");
                return true;
            }

            int loopCount = JobQueue.GetTryGetQueueCount();
            if (loopCount == 0)
                return false;

            for( int idx = 0; idx < loopCount; ++idx )
            {
                Message message = JobQueue.TryPopFront();
                if (message == null)
                    return false;

                PacketProcess.Instance.MsgProcess(message);
            }
            return true;
        }

        public override void OnLogin()
        {
            CQ_LOGIN req = new CQ_LOGIN();
            req.id = "User" + m_botIdx;
            req.pw = "1234";
            SendPacket(req);
        }

        public override void AwakeThread()
        {
            Launcher.Instance.MainThreadEventHandler.Set();
        }
    }
}
