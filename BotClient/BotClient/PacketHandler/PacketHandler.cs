using ShareData;
using System;
using System.Collections.Generic;

namespace BotClient.BotClient.PacketHandler
{
    class PacketHandler
    {
        #region Singleton
        private static PacketHandler m_instance;
        public static PacketHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PacketHandler();
                return m_instance;
            }
        }
        #endregion

        private Dictionary<int, Func<Packet, bool>> packetHandlerList;
        public Dictionary<int, Func<Packet, bool>> PacketHandlerList
        {
            get { return this.packetHandlerList; }
        }

        public PacketHandler()
        {
            packetHandlerList = new Dictionary<int, Func<Packet, bool>>();
            Init();
        }

        public void Init()
        {
            packetHandlerList.Add((int)PACKET_INDEX.SA_LOGIN, SA_LOGIN);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CHAT, SA_CHAT);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CHANGENICKNAME, SA_CHANGENICKNAME);
            packetHandlerList.Add((int)PACKET_INDEX.SA_ENTERCHATROOM, SA_ENTERCHATROOM);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CHATROOMLIST, SA_CHATROOMLIST);
        }

        public bool SA_LOGIN(Packet packet)
        {
            SA_LOGIN pck = (SA_LOGIN)packet;
            return true;
        }

        public bool SA_CHAT(Packet packet)
        {
            SA_CHAT noti = (SA_CHAT)packet;
            Form1.Instance.RefreshTextBox(noti.MsgStr);

            return true;
        }

        public bool SA_CHANGENICKNAME(Packet packet)
        {
            return true;
        }

        public bool SA_ENTERCHATROOM(Packet packet)
        {
            SA_ENTERCHATROOM ack = (SA_ENTERCHATROOM)packet;
            if (ack.Result == ShareData.SA_ENTERCHATROOM.E_RESULT.FAIL)
                return false;



            return true;
        }

        public bool SA_CHATROOMLIST(Packet packet)
        {
            return true;
        }
    }
}
