using BotClient.BotClient;
using ShareData.Message;
namespace BotClient.BotClient.Process
{
    class PacketProcess
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
            ShareData.Packet packet = (ShareData.Packet)message.GetValue();
            if (packet == null)
                return;

            PacketHandler.PacketHandler.Instance.PacketHandlerList[packet.GetPacketIndex()](packet);
        }
    }
}
