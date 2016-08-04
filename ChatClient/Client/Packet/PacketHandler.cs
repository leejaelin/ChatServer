﻿using ShareData;
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
        }

        public bool SA_LOGIN(ShareData.Packet packet)
        {
            SA_LOGIN pck = (SA_LOGIN)packet;
            return true;
        }

        public bool SN_CHAT(ShareData.Packet packet)
        {
            SN_CHAT noti = (SN_CHAT)packet;
            //Form1.Instance.GetTextMain().Text.Insert(Form1.Instance.GetTextMain().Text.Length, noti.MsgStr);
            Form1.Instance.RefreshTextBox(noti.MsgStr);

            return true;
        }
    }
}
