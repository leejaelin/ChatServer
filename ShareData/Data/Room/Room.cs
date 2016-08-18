using System;
using System.Collections.Generic;

namespace ShareData.Data.Room
{
    public class ChatRoomUserInfo
    {
        private uint userIndex;
        private string userNickname;
        
        ChatRoomUserInfo( uint idx, string nickname)
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
