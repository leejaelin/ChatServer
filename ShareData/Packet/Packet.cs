using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShareData
{
    public enum PACKET_INDEX
    {
        TESTPACKET = 0,
        CQ_LOGIN,
        SN_LOGIN,
    }

    [Serializable]
    public class Packet
    {
        private PACKET_INDEX index;
        private int length;

        public Packet(PACKET_INDEX index) 
        {
            this.index = index;
            this.length = GetPacketSize();
        }

        public virtual int GetPacketSize()
        {
            object o = (object)this;
            Stream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, o);
            return (int)s.Length;
        }
    }

    [Serializable]
    public class CQ_LOGIN : Packet
    {
        public CQ_LOGIN() : base(PACKET_INDEX.CQ_LOGIN)
        {
        }

        public enum E_LOGIN_TYPE
        {
            USER = 0,
            BOT = 1,
        }
        public string id;
        public string pw;
        public E_LOGIN_TYPE type;
    }

    [Serializable]
    public class SN_LOGIN : Packet
    {
        public enum E_RESULT
        {
            FAIL = -1,
            SUCCESS = 0,
        }
        public SN_LOGIN()
            : base(PACKET_INDEX.SN_LOGIN)
        {
        }

        public E_RESULT type;
    }
}
