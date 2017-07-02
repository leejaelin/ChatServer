using ChatServer.Data.Room;
using ChatServer.Data.User;
using ShareData;
using ShareData.Data.Room;
using ShareData.Message;
using System.Collections.Generic;

namespace ChatServer.Process
{
    class UserInOutProcess : IProcess
    {
        #region Singleton
        private static UserInOutProcess m_instance;
        public static UserInOutProcess Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new UserInOutProcess();
                return m_instance;
            }
        }
        #endregion

        public UserInOutProcess() { } // 유저가 서버에서 연결/종료 시 최대한 늦은 시점에 유저 컨테이너에서 뺴주기 위한 작업
        ~UserInOutProcess() { }

        public void MsgProcess(User user, Message message)
        {
            if (null == user) // 유저가 서버에 접속 하였을때
            {
                LoginProcess(message);
            }
            else // 유저가 서버 접속을 끊었을때
            {
                LogoutProcess(user, message);
            }
        }

        void LoginProcess(Message message)
        {
            User user = new User(message.GetCallerIdx(), message.GetSocket());
            UserContainer.Instance.Insert(user);

            RoomContainer roomContainer = RoomContainer.Instance;
            if( 0 >= roomContainer.ChatRoomList.Count ) { return; }
            SA_CHATROOMLIST noti = new SA_CHATROOMLIST();
            noti.ChatRoomList = roomContainer.ChatRoomList;
            noti.Type = SA_CHATROOMLIST.E_TYPE.ADD_LIST;
            user.DoSend(noti);
        }

        void LogoutProcess(User user, Message message)
        {
            RoomContainer roomContainer = RoomContainer.Instance;
            List<int> tmplistPopupRoom = new List<int>();
            foreach( var chatRoom in roomContainer.ChatRoomList )
            {
                //if( chatRoom.Value.RoomUserList.ContainsKey(user.Index) )
                //{
                    chatRoom.Value.RoomUserList.Remove(user.Index);
                    if (0 == chatRoom.Value.RoomUserList.Count)
                    {
                        tmplistPopupRoom.Add(chatRoom.Key);
                        SA_CHATROOMLIST relay = new SA_CHATROOMLIST();
                        relay.ChatRoomList.Add(chatRoom.Key, chatRoom.Value);
                        relay.Type = SA_CHATROOMLIST.E_TYPE.DEL_LIST;
                        foreach (var _user in UserContainer.Instance.ConUserContainer.Values)
                        {
                            if( user == _user ) { continue; }
                            _user.DoSend(relay);
                        }
                    }
                    else
                    {
                        SA_ENTERCHATROOM ack = new SA_ENTERCHATROOM();
                        ack.Result = SA_ENTERCHATROOM.E_RESULT.SUCCESS;
                        ack.ChatRoomInfo = chatRoom.Value;
                        
                        ChatRoom _chatRoom = roomContainer.Find(chatRoom.Key); // 존재 하는 방인지 먼저 검색
                        if (null == _chatRoom) { return; }

                        UserContainer userContainer = UserContainer.Instance;
                        foreach (ChatRoomUserInfo userInfo in _chatRoom.RoomUserList.Values)
                        {
                            User _user = userContainer.Find(userInfo.userIndex); // 존재 하는 유저인지 검색
                            if (null == user) { continue; }
                            _user.DoSend(ack);
                        }
                    }
                //}
            }

            foreach (var idx in tmplistPopupRoom) { roomContainer.Pop(idx); }
            UserContainer.Instance.Pop(message.GetCallerIdx());
        }
    }
}