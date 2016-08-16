using Client;
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
            init();
        }
        ~PacketHandler() { packetHandlerList = null; }
        private Dictionary<int, Func<ShareData.Packet, bool>> packetHandlerList;
        public Dictionary<int, Func<ShareData.Packet, bool>> PacketHandlerList
        {
            get{return this.packetHandlerList;}
        }
        
        public void init()
        {
            packetHandlerList.Add((int)PACKET_INDEX.SA_LOGIN, SA_LOGIN);
            packetHandlerList.Add((int)PACKET_INDEX.SN_CHAT, SN_CHAT);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CHANGENICKNAME, SA_CHANGENICKNAME);
        }

        public bool SA_LOGIN(ShareData.Packet packet)
        {
            SA_LOGIN ack = (SA_LOGIN)packet;
            Client.Scene.SceneManager.Instance.
            return true;
        }

        public bool SN_CHAT(ShareData.Packet packet)
        {
            SN_CHAT ack = (SN_CHAT)packet;
            
            LobbyForm.Instance.RefreshTextBox("[ "+ack.SenderNickname+" ] " + ack.MsgStr);

            return true;
        }

        public bool SA_CHANGENICKNAME(ShareData.Packet packet)
        {
            SA_CHANGENICKNAME ack = (SA_CHANGENICKNAME)packet;
            if (ack.result == ShareData.SA_CHANGENICKNAME.E_RESULT.FAIL)
                return false;

            Launcher.Instance.GetClient().Nickname = ack.nickname;
            return true;
        }
    }
}
