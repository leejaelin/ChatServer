using ChatServer.Data.User;
using ShareData.Message;

namespace ChatServer.ChatServer.Process
{
    interface IProcess
    {
        void MsgProcess(User user, Message message);
    }
}
