using System;

namespace ShareData
{
    [Serializable()]
    public class TestPacket
    {
        public int idx { get; set; }
        public int testIdx { get; set; }
        public string testString { get; set; }
    }
}