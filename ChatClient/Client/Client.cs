using ChatServer.Process;
using ShareData;
using ShareData.CommonLogic.Network;
using ShareData.Message;

namespace Client
{
    class Client : Network
    {
        public Client( int idx ) : base()
        {
            m_idx = idx;
        }

        // member variables
        private int m_idx;
   
        public int GetIdx() { return m_idx; }

        public void SendPacket(Packet packet)
        {
            sendPacket(packet);
        }

        public bool Do( Launcher l )
        {
            if(socket == null)
            {
                BeginConnect(); // 연결이 안되어 있으면 연결 시작
                return true;
            }

            //if(socket.Connected) // 소켓이 연결 되어 있을때
            //{
            //    if(GetConnectedToServer()) // 소켓연결+서버연결 완료 되었다면
            //    {
            //        Close();
            //    }
            //}

            int loopCount = JobQueue.GetTryGetQueueCount();
            if (loopCount == 0)
            {
                return false;
            } 

            for (int idx = 0; idx < loopCount; ++idx )
            {
                Message message = JobQueue.TryPopFront();
                if (message == null)
                    return false;

                PacketProcess.Instance.MsgProcess(message);
            }
            return true;
        }

        public override void Alive()
        {
            Launcher.Instance.ReleaseJobLoop();
        }
    };
}
