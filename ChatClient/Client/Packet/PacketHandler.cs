using ShareData;
using System;
using System.Collections.Generic;

namespace ChatClient.Client.Packet
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

        public PacketHandler()
        {
            packetHandlerList = new Dictionary<int, Func<ShareData.Packet, bool>>();
            inIt();
        }
        ~PacketHandler() { packetHandlerList = null; }
        private Dictionary<int, Func<ShareData.Packet, bool>> packetHandlerList;
        public Dictionary<int, Func<ShareData.Packet, bool>> PacketHandlerList
        {
            get{return this.packetHandlerList;}
        }

            
        
        private void inIt()
        {
            packetHandlerList.Add((int)PACKET_INDEX.SA_LOGIN, SA_LOGIN);
        }

        private bool SA_LOGIN(ShareData.Packet packet)
        {
            return true;
        }
    }
}
