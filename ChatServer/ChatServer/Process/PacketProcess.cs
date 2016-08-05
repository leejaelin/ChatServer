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
            packetHandlerList.Add((int)PACKET_INDEX.CQ_CHAT, CQ_CHAT);
            packetHandlerList.Add((int)PACKET_INDEX.CQ_CHANGENICKNAME, CQ_CHANGENICKNAME);
        }

        public void MsgProcess(User user, Message message)
        {
            Packet packet = (Packet)message.GetValue();

            PacketDispatcher(user, packet);
        }

        private void PacketDispatcher(User user, Packet packet)
        {
            if (!packetHandlerList.ContainsKey(packet.GetPacketIndex()))
                return;
            packetHandlerList[packet.GetPacketIndex()](user, packet);
        }

        private void broadCast(Packet packet)
        {
            ChatServer.Instance.Broadcast(packet);
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////

        private bool TESTPACKET(User user, Packet packet)
        {
            if (null == user)
                return false;

            TestPacket req = (TestPacket)packet;
            //Console.WriteLine(req.testString);
            
            SA_LOGIN ack = new SA_LOGIN();
            ack.result = 0;
            user.DoSend(ack);

            return true;
        }

        private bool CQ_LOGIN(User user, Packet packet)
        {
            CQ_LOGIN req = (CQ_LOGIN)packet;

            user.State = LoginedState.Instance;
            user.NickName = req.id;

            SA_LOGIN ack = new SA_LOGIN();
            ack.result = SA_LOGIN.E_RESULT.SUCCESS;
            ack.userIndex = user.Index;

            user.DoSend(ack);
            return true;
        }

        private bool CQ_CHAT(User user, Packet packet)
        {
            if (null == user)
                return false;

            CQ_CHAT req = (CQ_CHAT)packet;
            
            SN_CHAT ack = new SN_CHAT();
            ack.SenderIdx = user.Index;
            ack.SenderNickname = user.NickName;
            ack.MsgStr = req.MsgStr;
            broadCast(ack);

            return true;
        }

        private bool CQ_CHANGENICKNAME(User user, Packet packet) 
        {
            if( user == null )
                return false;

            CQ_CHANGENICKNAME req = (CQ_CHANGENICKNAME)packet;
            user.NickName = req.nickname;
            
            SA_CHANGENICKNAME ack = new SA_CHANGENICKNAME();
            ack.nickname = req.nickname;
            ack.result = SA_CHANGENICKNAME.E_RESULT.SUCCESS;
            user.DoSend(ack);

            return true;
        }
    }
}
