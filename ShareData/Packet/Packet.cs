using ShareData.Data.Room;
using System;
using System.Collections.Generic;
using System.IO;
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
        SA_CHANGENICKNAME,

        CQ_CHAT = PACKET_CATEGORY.CHAT, // 10000 ~
        SA_CHAT,

        CQ_CREATECHATROOM = PACKET_CATEGORY.ROOM, // 20000 ~
        SA_CREATECHATROOM,

        CQ_ENTERCHATROOM,
        SA_ENTERCHATROOM,

        CN_LEAVECHATROOM,
        
        SN_CHATROOMLIST,

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

    /// <summary>
    /// USER 관련 패킷
    /// </summary>
    /// 
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
        public CQ_CHANGENICKNAME()
            : base(PACKET_INDEX.CQ_CHANGENICKNAME)
        {
        }

        public string nickname; // 요청 닉네임
    }

    [Serializable]
    public class SA_CHANGENICKNAME : Packet
    {
        public enum E_RESULT
        {
            FAIL = -1,
            SUCCESS = 0,
        }

        public SA_CHANGENICKNAME()
            : base(PACKET_INDEX.SA_CHANGENICKNAME)
        {
        }

        public E_RESULT result; // 로그인 성공 여부.
        public string nickname; // 변경 닉네임
    }

    /// <summary>
    /// Chat 관련 패킷
    /// </summary>
    /// 

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
    public class SA_CHAT : Packet
    {
        public SA_CHAT()
            : base(PACKET_INDEX.SA_CHAT)
        {
        }

        public int RoomIdx; // 메시지 보낸 방 번호
        public uint SenderIdx; // 메시지 보낸 유저 번호
        public string SenderNickname; // 메시지 보낸 유저 닉네임(임시)
        public string MsgStr; // 전달 받은 채팅 메시지
    }

    /// <summary>
    /// ROOM 관련 패킷
    /// </summary>
    /// 
    [Serializable]
    public class CQ_CREATECHATROOM : Packet
    {
        public CQ_CREATECHATROOM()
            : base(PACKET_INDEX.CQ_CREATECHATROOM)
        {
            chatRoomInfo = new ChatRoom();
        }

        public ChatRoom chatRoomInfo; // 생성 요청 방 정보
    }

    [Serializable]
    public class SA_CREATECHATROOM : Packet
    {
        public SA_CREATECHATROOM()
            : base(PACKET_INDEX.SA_CREATECHATROOM)
        {
            chatRoomInfo = new ChatRoom();
        }

        public enum E_RESULT
        {
            FAIL = -1,
            SUCCESS = 0,
        }

        public E_RESULT Result;
        public ChatRoom chatRoomInfo; // 생성 방 정보
    }

    [Serializable]
    public class CQ_ENTERCHATROOM : Packet
    {
        public CQ_ENTERCHATROOM()
            : base(PACKET_INDEX.CQ_ENTERCHATROOM)
        {
        }

        public int RoomIdx; // 요청 방 번호
    }

    [Serializable]
    public class SA_ENTERCHATROOM : Packet
    {
        public SA_ENTERCHATROOM()
            : base(PACKET_INDEX.SA_ENTERCHATROOM)
        {
            ChatRoomInfo = new ChatRoom();
        }

        public enum E_RESULT
        {
            FAIL = -1,
            SUCCESS = 0,
        }

        public E_RESULT Result;  // 성공 여부
        public ChatRoom ChatRoomInfo;   // 채팅방 정보
    }

    [Serializable]
    public class CN_LEAVECHATROOM : Packet
    {
        public CN_LEAVECHATROOM()
            : base(PACKET_INDEX.CN_LEAVECHATROOM)
        {
        }

        public int roomIdx;
    }

    [Serializable]
    public class SN_CHATROOMLIST : Packet
    {
        public SN_CHATROOMLIST()
            : base(PACKET_INDEX.SN_CHATROOMLIST)
        {
            ChatRoomList = new Dictionary<int, ChatRoom>();
        }

        public enum E_TYPE
        {
            ADD_LIST,   // 추가된 방 목록 전달
            DEL_LIST,   // 삭제된 방 목록 전달
        }
        public E_TYPE Type;
        public Dictionary<int, ChatRoom> ChatRoomList;
    }
}
