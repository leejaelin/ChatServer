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
        PACKET_INDEX_BEGIN = PACKET_CATEGORY.USER, // 0 ~

        TESTPACKET,
        CQ_LOGIN,
        SA_LOGIN,
        CQ_CHANGENICKNAME,

        CQ_CHAT = PACKET_CATEGORY.CHAT, // 10000 ~
        SN_CHAT,

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

        public string id; // 로그인 시 유저 아이디
        public string pw; // 로그인 시 유저 비밀번호
        public string nickname; // 닉네임(임시)
        public E_LOGIN_TYPE type; // 유저 로그인 타입(유저, 봇)
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

        public E_RESULT result; // 로그인 성공 여부.
        public uint userIndex; // 로그인 성공하면 서버에서 전달하는 유저 인덱스
    }

    [Serializable]
    public class CQ_CHANGENICKNAME : Packet
    {
        public enum E_RESULT
        {
            FAIL = -1,
            SUCCESS = 0,
        }

        public CQ_CHANGENICKNAME()
            : base(PACKET_INDEX.CQ_CHANGENICKNAME)
        {
        }

        public E_RESULT result; // 로그인 성공 여부.
        public string nickname; // 닉네임(임시)
    }

    [Serializable]
    public class CQ_CHAT : Packet
    {
        public CQ_CHAT()
            : base(PACKET_INDEX.CQ_CHAT)
        {
        }

        public int RoomIdx; // 유저가 메시지 전달한 채팅방 인덱스
        public string MsgStr; // 유저가 전달한 채팅 메시지
    }

    [Serializable]
    public class SN_CHAT : Packet
    {
        public SN_CHAT()
            : base(PACKET_INDEX.SN_CHAT)
        {
        }

        public uint SenderIdx; // 메시지 보낸 유저 번호
        public string MsgStr; // 전달 받은 채팅 메시지
    }
}
