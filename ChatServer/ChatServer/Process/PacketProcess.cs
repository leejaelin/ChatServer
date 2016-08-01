using ChatServer.Data.User;
using ChatServer.Data.User.UserState;
using ShareData;
using ShareData.Message;
using System;
using System.Collections.Generic;

namespace ChatServer.Process
{
    class PacketProcess : IProcess
    {
        #region Singleton
        private static PacketProcess m_instance;
        public static PacketProcess Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PacketProcess();
                return m_instance;
            }
        }
        #endregion

        public PacketProcess()
        {
            packetHandlerList = new Dictionary<int, Func<User, Packet, bool>>();
            initPacketHandlerList();
        }
        ~PacketProcess() { }

        private Dictionary<int, Func<User, Packet, bool>> packetHandlerList;
        private void initPacketHandlerList()
        {
            // (int)Enum값을 키로 사용하는 패킷 핸들러들을 등록 한다.
            packetHandlerList.Add((int)PACKET_INDEX.TESTPACKET, TESTPACKET);
            packetHandlerList.Add((int)PACKET_INDEX.CQ_LOGIN, CQ_LOGIN );
        }

        public void MsgProcess(User user, Message message)
        {
            Packet pck = (Packet)message.GetValue();
            
            PacketDispatcher(user, pck);
        }

        private void PacketDispatcher(User user, Packet packet)
        {
            packetHandlerList[packet.GetPacketIndex()](user, packet);
        }

        /// ////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////

        private bool TESTPACKET(User user, Packet packet)
        {
            TestPacket req = (TestPacket)packet;
            Console.WriteLine(req.testString);
            
            SA_LOGIN ack = new SA_LOGIN();
            ack.result = 0;
            user.DoSend(ack);

            return true;
        }

        private bool CQ_LOGIN(User user, Packet packet)
        {
            CQ_LOGIN req = (CQ_LOGIN)packet;
            Console.WriteLine( "ID : " + req.id);

            user.State = LoginedState.Instance;

            SA_LOGIN ack = new SA_LOGIN();
            ack.result = 0;
            user.DoSend(ack);
            return true;
        }
    }
}
