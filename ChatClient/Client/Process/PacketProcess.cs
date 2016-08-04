using ChatClient.Client.Packet;
using Clinet.Process;
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
        }

        public void MsgProcess(Message message)
        {
            Packet packet = (Packet)message.GetValue();
            if (packet == null)
                return;
            PacketHandler.Instance.PacketHandlerList[packet.GetPacketIndex()](packet);
        }
    }
}