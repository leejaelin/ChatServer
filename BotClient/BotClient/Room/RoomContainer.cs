using System.Collections.Generic;

namespace BotClient.BotClient.Room
{
    class RoomContainer
    {
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
