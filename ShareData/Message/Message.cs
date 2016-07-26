using System;

namespace ShareData.Message
{
    public enum MessageType
    {
        M_PING      = 0x01,
        M_PACKET    = 0x02,
        M_PROCEDURE = 0x03,
        M_ALARM     = 0x04,
    }

    public class Message
    {
        public Message(MessageType messageType) { this.m_messageType = messageType; }
        public Message(MessageType messageType, Object obj) 
        {
            this.m_messageType = messageType;
            this.m_value = obj;
        }
        ~Message() { }

        private MessageType m_messageType;
        private Object m_value;

        public MessageType GetMessageType() { return this.m_messageType; }
        public void SetValue(Object obj) { this.m_value = obj; }
        public Object GetValue() { return this.m_value; }
    }
}
