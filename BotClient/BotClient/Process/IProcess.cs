using ShareData.Message;

namespace BotClient.BotClient.Process
{
    interface IProcess
    {
        void MsgProcess(Message message);
    }
}
