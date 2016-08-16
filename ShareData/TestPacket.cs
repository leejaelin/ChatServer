using System;
using ShareData;

namespace ShareData
{
    [Serializable()]
    public class TestPacket : Packet
    {
        public TestPacket() : base(PACKET_INDEX.TESTPACKET) { }
        public string testString { get; set; }
    }

    [Serializable()]
    public class TestPacket2 : Packet
    {
        public TestPacket2() : base(PACKET_INDEX.TESTPACKET) { }
        public string testString { get; set; }
        public string testString2 { get; set; }
    }
}