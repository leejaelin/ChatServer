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
            m_botIdx = idx;
            m_userIdx = 0;
        }

        // member variables
        private int m_botIdx; // 클라이언트 봇 번호
        private uint m_userIdx; // 서버에서 전달 받은 유저 인덱스
   
        public int GetIdx() { return m_botIdx; }

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

        public override void OnLogin()
        {
            CQ_LOGIN req = new CQ_LOGIN();
            req.id = "User"+m_botIdx;
            req.pw = "1234";
            SendPacket(req);

            //if (Launcher.Instance.Mode == Launcher.E_MODE.BOT)
            //{
            //    System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
            //    tm.Interval = 2000;
            //    tm.Tick += new System.EventHandler((sender, e) =>
            //    {
            //        TestPacket botNoti = new TestPacket();
            //        botNoti.testString = "안녕.Hello I'm " + m_botIdx + "th bot!!!";
            //        SendPacket(botNoti);
            //    });
            //    tm.Start();
            //}
        }

        public override void Alive()
        {
            Launcher.Instance.ReleaseJobLoop();
        }
    };
}
