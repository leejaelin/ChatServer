using ShareData.Data.Room;
using System.Collections.Generic;

namespace ChatClient.Data.Room
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

        public Dictionary<int, ChatRoom> ConRoomContainer { get; set; }

        public RoomContainer()
        {
            ConRoomContainer = new Dictionary<int, ChatRoom>();
        }

        public void Insert(ChatRoom room)
        {
            if( ConRoomContainer.ContainsKey( room.Index ) )
                ConRoomContainer.Remove(room.Index);
            ConRoomContainer.Add(room.Index, room);
        }

        public ChatRoom Find(int roomIdx)
        {
            ChatRoom room;
            ConRoomContainer.TryGetValue(roomIdx, out room);
            return room != null? room : null;
        }

        public void Pop( int roomIdx )
        {
            if (ConRoomContainer.ContainsKey(roomIdx))
                ConRoomContainer.Remove(roomIdx);
        }
    }
}
