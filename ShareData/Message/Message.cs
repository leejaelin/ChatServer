using System;
using System.Net.Sockets;

namespace ShareData.Message
{
    public enum MessageType
    {
        M_PING = 0,
        M_PACKET,
        M_PROCEDURE,
        M_ALARM,
        M_USER_IN_OUT,
    }

    public class Message
    {
        public Message(uint idx, MessageType messageType, Object obj, Socket socket) 
        {
            m_callerIdx = idx;
            m_messageType = messageType;
            m_value = obj;
            m_socket = socket;
        }
        ~Message() { }

        private uint m_callerIdx;
        private MessageType m_messageType;
        private Object m_value;
        private Socket m_socket;

        public uint GetCallerIdx() { return m_callerIdx; }
        public MessageType GetMessageType() { return this.m_messageType; }
        public void SetValue(Object obj) { this.m_value = obj; }
        public Object GetValue() { return this.m_value; }
        public Socket GetSocket() { return m_socket; }
    }
}
