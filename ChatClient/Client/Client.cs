using ChatServer.Process;
using ShareData;
using ShareData.CommonLogic.Network;
using ShareData.Message;

namespace ChatClient.Client
{
    class Client : Network
    {
        public Client(string IP) : base(Network.E_SOCKET_MODE.CLIENT)
        {
            UserIdx = 0;
            this.IP = IP;
        }

        // member variables
        public uint UserIdx { get; set; } // 서버에서 전달 받은 유저 인덱스
        private string m_nickname; // 유저 닉네임
        public string Nickname { get { return m_nickname; } set{m_nickname = value;} }
        public new string IP;
        //public new string PORT;

        public void SendPacket(Packet packet)
        {
            sendPacket(packet);
        }

        public bool Do()
        {
            if(socket == null)
            {
                BeginConnect(IP); // 연결이 안되어 있으면 연결 시작
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
            req.id = "User";
            req.pw = "1234";
            SendPacket(req);
        }

        public override void AwakeThread()
        {
            Launcher.Instance.ReleaseJobLoop();
        }
    };
}
