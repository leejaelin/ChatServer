using ChatClient.Client.PacketHandler;
using ChatClient.Client;
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

        private PacketHandler packetHandler;
        public PacketProcess()
        {
            packetHandler = PacketHandler.Instance;
        }

        public void MsgProcess(Message message)
        {
            if ( message.GetMessageType() == MessageType.M_USER_IN_OUT )
            {
                serverDisconnected();
                return;
            }

            Packet packet = (Packet)message.GetValue();
           if (packet == null)
                return;


           if (!packetHandler.PacketHandlerList.ContainsKey(packet.GetPacketIndex()))
               return;

            packetHandler.PacketHandlerList[packet.GetPacketIndex()](packet);
        }

        private void serverDisconnected()
        {
            Launcher.Instance.TerminateProcess();
        }
    }
}