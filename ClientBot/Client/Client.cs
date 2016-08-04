using ShareData;
using ShareData.CommonLogic.Network;

namespace ClientBot.Client
{
    class Client : Network
    {
        public Client( int idx ) : base()
        {
            m_idx = 0;
        }

        // member variables
        private int m_idx;
           
        public int GetIdx() { return m_idx; }

        public void SendPacket(Packet packet)
        {
          
        }

        public void Do( Launcher l )
        {
            if(socket == null)
            {
                BeginConnect(); // 연결이 안되어 있으면 연결 시작
                return;
            }
        }
    };
}
