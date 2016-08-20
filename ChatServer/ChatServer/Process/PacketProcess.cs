using ChatServer.Data.Room;
using ChatServer.Data.User;
using ChatServer.Data.User.UserState;
using ShareData;
using ShareData.Data.Room;
using ShareData.Message;
using System;
using System.Collections.Generic;

namespace ChatServer.Process
{
    class PacketProcess : IProcess
    {
        #region Singleton
        private static PacketProcess m_instance;
        public static PacketProcess Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PacketProcess();
                return m_instance;
            }
        }
        #endregion

        public PacketProcess()
        {
            packetHandlerList = new Dictionary<int, Func<User, Packet, bool>>();
            initPacketHandlerList();
        }
        ~PacketProcess() { }

        private Dictionary<int, Func<User, Packet, bool>> packetHandlerList;
        private void initPacketHandlerList()
        {
            // (int)Enum값을 키로 사용하는 패킷 핸들러들을 등록 한다.
            packetHandlerList.Add((int)PACKET_INDEX.TESTPACKET, TESTPACKET);
            packetHandlerList.Add((int)PACKET_INDEX.CQ_LOGIN, CQ_LOGIN );
            packetHandlerList.Add((int)PACKET_INDEX.CQ_CHAT, CQ_CHAT);
            packetHandlerList.Add((int)PACKET_INDEX.CQ_CHANGENICKNAME, CQ_CHANGENICKNAME);
            packetHandlerList.Add((int)PACKET_INDEX.CQ_CREATECHATROOM, CQ_CREATECHATROOM);
            packetHandlerList.Add((int)PACKET_INDEX.CQ_ENTERCHATROOM, CQ_ENTERCHATROOM);
            packetHandlerList.Add((int)PACKET_INDEX.CN_LEAVECHATROOM, CN_LEAVECHATROOM);
        }

        public void MsgProcess(User user, Message message)
        {
            Packet packet = (Packet)message.GetValue();

            PacketDispatcher(user, packet);
        }

        private void PacketDispatcher(User user, Packet packet)
        {
            if (!packetHandlerList.ContainsKey(packet.GetPacketIndex()))
                return;
            packetHandlerList[packet.GetPacketIndex()](user, packet);
        }

        private void broadCastForServer(Packet packet)
        {
            foreach (var user in UserContainer.Instance.ConUserContainer.Values)
                user.DoSend(packet);
        }

        private void broadCastForChatRoom(Packet packet, int roomIdx)
        {
            RoomContainer roomContainer = RoomContainer.Instance;
            ChatRoom chatRoom = roomContainer.Find(roomIdx); // 존재 하는 방인지 먼저 검색
            if( null == chatRoom )
                return;

            UserContainer userContainer = UserContainer.Instance;
            foreach( ChatRoomUserInfo userInfo in chatRoom.RoomUserList.Values )
            {
                User user = userContainer.Find(userInfo.userIndex); // 존재 하는 유저인지 검색
                if (null == user)
                    continue;
                user.DoSend(packet);
            }
        }
        /// ////////////////////////////////////////////////////////////////////////////////////
        /// ////////////////////////////////////////////////////////////////////////////////////

        private bool TESTPACKET(User user, Packet packet)
        {
            if (null == user)
                return false;

            TestPacket req = (TestPacket)packet;
            //Console.WriteLine(req.testString);
            
            SA_LOGIN ack = new SA_LOGIN();
            ack.result = 0;
            user.DoSend(ack);

            return true;
        }

        private bool CQ_LOGIN(User user, Packet packet)
        {
            CQ_LOGIN req = (CQ_LOGIN)packet;

            user.State = LoginedState.Instance;
            user.NickName = req.id;

            SA_LOGIN ack = new SA_LOGIN();
            ack.result = SA_LOGIN.E_RESULT.SUCCESS;
            ack.userIndex = user.Index;

            user.DoSend(ack);
            return true;
        }

        private bool CQ_CHAT(User user, Packet packet)
        {
            if (null == user)
                return false;

            CQ_CHAT req = (CQ_CHAT)packet;
            
            SA_CHAT ack = new SA_CHAT();
            ack.SenderIdx = user.Index;
            ack.SenderNickname = user.NickName;
            ack.MsgStr = req.MsgStr;
            ack.RoomIdx = req.RoomIdx;
            broadCastForChatRoom(ack, req.RoomIdx);

            return true;
        }

        private bool CQ_CHANGENICKNAME(User user, Packet packet) 
        {
            if( user == null )
                return false;

            CQ_CHANGENICKNAME req = (CQ_CHANGENICKNAME)packet;
            user.NickName = req.nickname;
            
            SA_CHANGENICKNAME ack = new SA_CHANGENICKNAME();
            ack.nickname = req.nickname;
            ack.result = SA_CHANGENICKNAME.E_RESULT.SUCCESS;
            user.DoSend(ack);

            return true;
        }

        private bool CQ_CREATECHATROOM(User user, Packet packet)
        {
            if (user == null)
                return false;

            CQ_CREATECHATROOM req = (CQ_CREATECHATROOM)packet;

            req.chatRoomInfo.RoomUserList.Add(user.Index, new ChatRoomUserInfo(user.Index, user.NickName));
            
            RoomContainer roomContainer = RoomContainer.Instance;
            bool ret = roomContainer.Insert(req.chatRoomInfo);
            
            SA_CREATECHATROOM ack = new SA_CREATECHATROOM();
            if (ret)
            {
                ack.Result = SA_CREATECHATROOM.E_RESULT.SUCCESS;
                ack.chatRoomInfo = req.chatRoomInfo;
            }
            else
                ack.Result = SA_CREATECHATROOM.E_RESULT.FAIL;

            user.DoSend(ack);

            SN_CHATROOMLIST noti = new SN_CHATROOMLIST();
            noti.Type = SN_CHATROOMLIST.E_TYPE.ADD_LIST;
            noti.ChatRoomList.Add(req.chatRoomInfo.Index, req.chatRoomInfo);
            broadCastForServer(noti);
            return true;
        }

        private bool CQ_ENTERCHATROOM(User user, Packet packet)
        {
            if (user == null)
                return false;

            CQ_ENTERCHATROOM req = (CQ_ENTERCHATROOM)packet;

            SA_ENTERCHATROOM ack = new SA_ENTERCHATROOM();

            RoomContainer roomContainer = RoomContainer.Instance;
            ChatRoom chatRoom = roomContainer.Find(req.RoomIdx);
            if (null == chatRoom)
            {
                ack.Result = SA_ENTERCHATROOM.E_RESULT.FAIL;
                user.DoSend(ack);
                return false;
            }

            // 방 안에 유저 리스트에 추가
            if( chatRoom.RoomUserList.ContainsKey( user.Index ) ) // 방안에 유저 이미 존재
            {
                ack.Result = SA_ENTERCHATROOM.E_RESULT.FAIL;
                user.DoSend(ack);
                return false;
            }

            chatRoom.RoomUserList.Add(user.Index, new ChatRoomUserInfo(user.Index, user.NickName));
            
            ack.Result = SA_ENTERCHATROOM.E_RESULT.SUCCESS;
            ack.ChatRoomInfo = chatRoom;
            broadCastForChatRoom(ack, chatRoom.Index);

            return true;
        }

        private bool CN_LEAVECHATROOM(User user, Packet packet)
        {
            if (null == user)
                return false;
            
            CN_LEAVECHATROOM noti = (CN_LEAVECHATROOM)packet;

            RoomContainer roomContainer = RoomContainer.Instance;
            ChatRoom chatRoom = roomContainer.Find(noti.roomIdx);
            if (null == chatRoom)
            {
                return false;
            }
            
            chatRoom.RoomUserList.Remove(user.Index);
            if( 0 == chatRoom.RoomUserList.Count )
            {
                roomContainer.Pop(noti.roomIdx);
                SN_CHATROOMLIST relay = new SN_CHATROOMLIST();
                relay.ChatRoomList.Add(noti.roomIdx, chatRoom);
                relay.Type = SN_CHATROOMLIST.E_TYPE.DEL_LIST;
                broadCastForServer(relay);
            }

            return true;
        }
    }
}
