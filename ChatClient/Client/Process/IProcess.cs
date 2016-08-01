using ShareData.Message;

namespace Clinet.Process
{
    interface IProcess
    {
        void MsgProcess(Message message);
    }
}
