using BotClient.BotClient.Process;
using ShareData;
using ShareData.Message;

namespace BotClient.BotClient
{
    class BotClient : ShareData.CommonLogic.Network.Network
    {
        public BotClient( int idx ) : base()
        {
            m_botIdx = idx;
        }

        private int m_botIdx;
        public int BotIdx { get { return m_botIdx; } set { m_botIdx = value; } }

        public void SendPacket( ShareData.Packet packet )
        {
            sendPacket(packet);
        }

        public bool Do()
        {
            if( socket == null )
            {
                BeginConnect();
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
