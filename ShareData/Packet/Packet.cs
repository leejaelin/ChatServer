using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShareData
{
    public enum PACKET_CATEGORY
    {
        USER = 0,
        CHAT = 10000,
        ROOM = 20000,
    }
    public enum PACKET_INDEX
    {
        PACKET_INDEX_BEGIN = PACKET_CATEGORY.USER,

        TESTPACKET = PACKET_INDEX_BEGIN+1,
        CQ_LOGIN,
        SA_LOGIN,

        PACKET_INDEX_END,
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

        public int GetPacketIndex()
        {
            return (int)index;
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
        public CQ_LOGIN()
            : base(PACKET_INDEX.CQ_LOGIN)
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

        public uint Index;
        public Socket socket;
    }

    [Serializable]
    public class SA_LOGIN : Packet
    {
        public enum E_RESULT
        {
            FAIL = -1,
            SUCCESS = 0,
        }

        public SA_LOGIN()
            : base(PACKET_INDEX.SA_LOGIN)
        {
        }

        public E_RESULT result;
        public string testStr;
    }
}
