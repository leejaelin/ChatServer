using System;
using System.Collections.Generic;

namespace ShareData.Data.Room
{
    [Serializable]
    public class ChatRoomUserInfo
    {
        public uint userIndex { get; set; }
        public string userNickname { get; set; }
        
        public ChatRoomUserInfo( uint idx, string nickname)
        {
            userIndex = idx;
            userNickname = nickname;
        }
    }

    [Serializable]
    public class ChatRoom
    {
        public int Index { get; set; }  // 방 번호
        public String Title { get; set; }   // 방 제목

        public Dictionary<uint, ChatRoomUserInfo> RoomUserList { get; set; }

        public ChatRoom()
        {
            Index = 0;
            Title = null;
            RoomUserList = new Dictionary<uint, ChatRoomUserInfo>();
        }
    }
}
