using ShareData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotClient.BotClient.Packet
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

        private Dictionary<int, Func<ShareData.Packet, bool>> packetHandlerList;
        public Dictionary<int, Func<ShareData.Packet, bool>> PacketHandlerList
        {
            get { return this.packetHandlerList; }
        }

        public PacketHandler()
        {
            packetHandlerList = new Dictionary<int, Func<ShareData.Packet, bool>>();
            Init();
        }

        public void Init()
        {
            packetHandlerList.Add((int)PACKET_INDEX.SA_LOGIN, SA_LOGIN);
            packetHandlerList.Add((int)PACKET_INDEX.SN_CHAT, SN_CHAT);
        }

        public bool SA_LOGIN(ShareData.Packet packet)
        {
            SA_LOGIN pck = (SA_LOGIN)packet;
            return true;
        }

        public bool SN_CHAT(ShareData.Packet packet)
        {
            SN_CHAT noti = (SN_CHAT)packet;
            Form1.Instance.RefreshTextBox(noti.MsgStr);

            return true;
        }
    }
}
