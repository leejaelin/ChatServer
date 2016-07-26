using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShareData
{
    public enum PACKET_INDEX
    {
        TESTPACKET = 0,
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
}
