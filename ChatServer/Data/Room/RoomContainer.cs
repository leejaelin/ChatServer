using ShareData.Data.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Data.Room
{
    class RoomContainer
    {
        #region Singleton
        private static RoomContainer m_instance;
        public static RoomContainer Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new RoomContainer();
                return m_instance;
            }
        }
        #endregion

        public Dictionary<int, ChatRoom> ChatRoomList { get; set; }
        
        private int roomCount;
        public RoomContainer()
        {
            ChatRoomList = new Dictionary<int, ChatRoom>();
            roomCount = 0;
        }
        ~RoomContainer()
        {
            ChatRoomList.Clear();
            ChatRoomList = null;
        }

        public bool Insert(ChatRoom chatRoom)
        {
            chatRoom.Index = roomCount;
            ChatRoomList.Add(roomCount++, chatRoom);
            return true;
        }

        public ChatRoom Find(int chatRoomIdx)
        {
            ChatRoom chatRoom;

            ChatRoomList.TryGetValue(chatRoomIdx, out chatRoom);
            return chatRoom != null ? chatRoom : null;
        }

        public void Pop(int chatRoomIdx)
        {
            if (ChatRoomList.ContainsKey(chatRoomIdx))
                ChatRoomList.Remove(chatRoomIdx);
        }
        
    }
}
