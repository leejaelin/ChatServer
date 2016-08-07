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

        public Dictionary<int, Room> ConRoomContainer { get; set; }

        public RoomContainer()
        {
            ConRoomContainer = new Dictionary<int, Room>();
        }

        public void Inser( Room room )
        {
            if( ConRoomContainer.ContainsKey( room.Index ) )
                ConRoomContainer.Remove(room.Index);
            ConRoomContainer.Add(room.Index, room);
        }

        public Room Find( int roomIdx )
        {
            Room room;
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
