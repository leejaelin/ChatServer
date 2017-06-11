using ChatClient.Client.Scene;
using ChatClient.Client;
using ShareData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChatClient.Client.PacketHandler
{
    class PacketHandler
    {
        #region Singleton
        private static PacketHandler m_instance;
        public static PacketHandler Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PacketHandler();
                return m_instance;
            }
        }
        #endregion

        public PacketHandler()
        {
            packetHandlerList = new Dictionary<int, Func<ShareData.Packet, bool>>();
            init();
        }
        ~PacketHandler() { packetHandlerList = null; }
        private Dictionary<int, Func<ShareData.Packet, bool>> packetHandlerList;
        public Dictionary<int, Func<ShareData.Packet, bool>> PacketHandlerList
        {
            get{return this.packetHandlerList;}
        }
        
        public void init()
        {
            packetHandlerList.Add((int)PACKET_INDEX.SA_LOGIN, SA_LOGIN);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CHAT, SA_CHAT);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CHANGENICKNAME, SA_CHANGENICKNAME);
            packetHandlerList.Add((int)PACKET_INDEX.SA_CREATECHATROOM, SA_CREATECHATROOM);
            packetHandlerList.Add((int)PACKET_INDEX.SA_ENTERCHATROOM, SA_ENTERCHATROOM);
            packetHandlerList.Add((int)PACKET_INDEX.SN_CHATROOMLIST, SN_CHATROOMLIST);
        }

        public bool SA_LOGIN(ShareData.Packet packet)
        {
            SA_LOGIN ack = (SA_LOGIN)packet;
            Client client = Launcher.Instance.GetClient();
            client.UserIdx = ack.userIndex;

            SceneManager currentScene = Scene.SceneManager.Instance;
            currentScene.ChangeScene(new LobbyScene()); // 로그인이 성공 하면 LobbyScene을 실행 시켜준다
            return true;
        }

        public bool SA_CHAT(ShareData.Packet packet)
        {
            SceneManager sceneManager = SceneManager.Instance;
            if (sceneManager.CurrentScene.GetType() != typeof(LobbyScene)) // 현재 로비 Scene이 없으므로 실패
                return false;

            SA_CHAT ack = (SA_CHAT)packet;
            
            LobbyScene lobbyScene = (LobbyScene)sceneManager.CurrentScene;
            if (!lobbyScene.ChatRoomSceneList.ContainsKey(ack.RoomIdx))
                return false;

            lobbyScene.ChatRoomSceneList[ack.RoomIdx].RecvChatMessage("[ "+ack.SenderNickname+" ] " + ack.MsgStr);

            return true;
        }

        public bool SA_CHANGENICKNAME(ShareData.Packet packet)
        {
            SA_CHANGENICKNAME ack = (SA_CHANGENICKNAME)packet;
            if (ack.result == ShareData.SA_CHANGENICKNAME.E_RESULT.FAIL)
                return false;

            Launcher.Instance.GetClient().Nickname = ack.nickname;
            return true;
        }

        public bool SA_CREATECHATROOM(Packet packet)
        {
            SceneManager sceneManager = SceneManager.Instance;
            if (sceneManager.CurrentScene.GetType() != typeof(LobbyScene)) // 현재 로비 Scene이 없으므로 실패
                return false;

            SA_CREATECHATROOM ack = (SA_CREATECHATROOM)packet;
            
            LobbyScene lobbyScene = (LobbyScene)sceneManager.CurrentScene;
            lobbyScene.AddChatRoom(ack.chatRoomInfo);

            return true;
        }

        public bool SA_ENTERCHATROOM(Packet packet)
        {
            SA_ENTERCHATROOM ack = (SA_ENTERCHATROOM)packet;
            if (ack.Result == ShareData.SA_ENTERCHATROOM.E_RESULT.FAIL) // 서버에서 실패값 리턴
                return false;

            Form currentForm = SceneManager.Instance.CurrentScene;
            if (currentForm.GetType() != typeof(LobbyScene)) // 현재 로비 Scene이 없으므로 실패
                return false;

            LobbyScene lobbyScene = (LobbyScene)currentForm;
            lobbyScene.AddChatRoom(ack.ChatRoomInfo);

            return true;
        }

        public bool SN_CHATROOMLIST(Packet packet)
        {
            SceneManager sceneManager = SceneManager.Instance;
            if (sceneManager.CurrentScene.GetType() != typeof(LobbyScene)) // 현재 로비 Scene이 없으므로 실패
            {
                var reservedMessageContainer = sceneManager.GetReservedMessageContainer();
                if (reservedMessageContainer.ContainsKey(E_SCENE.LOBBY))
                {
                    reservedMessageContainer[E_SCENE.LOBBY].Add(packet);
                }
                else
                {
                    var list = new List<Packet>();
                    list.Add(packet);
                    reservedMessageContainer.Add( E_SCENE.LOBBY, list );
                }
                return false;
            }

            SN_CHATROOMLIST noti = (SN_CHATROOMLIST)packet;
            
            LobbyScene lobbyScene = (LobbyScene)sceneManager.CurrentScene;
            lobbyScene.RefreshChatRoomList(noti.Type, noti.ChatRoomList);
            return true;
        }
    }
}
