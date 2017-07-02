using ChatClient.Library.MyScene;
using ChatClient.Client;
using ShareData;
using ShareData.Data.Room;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using ChatClient.Client.Scene;

namespace ChatClient
{
    public partial class LobbyScene : MyScene
    {
        #region Singleton
        private static LobbyScene m_instance;
        public static LobbyScene Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new LobbyScene();
                return m_instance;
            }
        }
        #endregion

        private Thread m_UIThread;
        public Dictionary<int, ChatRoomScene> ParticipatedChatRoomList { get; set; }
        private List<ChatRoom> roomList; // 방 목록 리스트박스 삽입되어 있는 정보
        public LobbyScene()
        {
            InitializeComponent();
            m_UIThread = new Thread(new ThreadStart(ThreadProc));
            m_UIThread.Start();
            ParticipatedChatRoomList = new Dictionary<int, ChatRoomScene>();
            roomList = new List<ChatRoom>();
        }

        private void ThreadProc()
        {
        }

        public override void OnEntry()
        {
            SceneManager sceneManager = SceneManager.Instance;
            Client.Client client = Launcher.Instance.GetClient();
            client.SendPacket(new CQ_CHATROOMLIST());
        }

        public void AddChatRoom(ChatRoom chatRoom)
        {
            Thread newThread = new Thread(new ThreadStart(() =>
            {
                if (ParticipatedChatRoomList.ContainsKey(chatRoom.Index))
                {
                    ChatRoomScene chatRoomScene = null;
                    ParticipatedChatRoomList.TryGetValue(chatRoom.Index, out chatRoomScene);
                    chatRoomScene.AddUserList(chatRoom);
                    return;
                }

                ChatRoomScene newChatRoom = new ChatRoomScene(chatRoom);
                ParticipatedChatRoomList.Add(chatRoom.Index, newChatRoom);
                AddRoomList(chatRoom);
                Application.Run(newChatRoom);

                // 삭제
                ParticipatedChatRoomList.Remove(chatRoom.Index);
                newChatRoom = null;

                // 유저 채팅방에서 나감 처리
                Client.Client client = Launcher.Instance.GetClient();
                client.SendPacket(new CN_LEAVECHATROOM() { roomIdx = chatRoom.Index });
            }));
            newThread.Start();
        }

        public void RefreshChatRoomList(SA_CHATROOMLIST.E_TYPE type, Dictionary<int, ChatRoom> roomList)
        {
            switch (type)
            {
                case SA_CHATROOMLIST.E_TYPE.ADD_LIST:
                    {
                        foreach (var chatRoom in roomList)
                            AddRoomList(chatRoom.Value);
                    }
                    break;
                case SA_CHATROOMLIST.E_TYPE.DEL_LIST:
                    {
                        foreach (var chatRoom in roomList)
                            RemoveRoomList(chatRoom.Value);
                    }
                    break;
            }
        }

        private void AddRoomList(ChatRoom chatRoomInfo)
        {
            invokeFunc(() =>
            {
                foreach (ChatRoom room in roomList) // 존재하는 리스트 여부 검사
                {
                    if (chatRoomInfo.Index == room.Index)
                        return;
                }

                roomList.Add(chatRoomInfo);
                this.ListBox_RoomList.Items.Add(chatRoomInfo.Title);
               // ParticipatedChatRoomList.Add(chatRoomInfo.Index, new ChatRoomScene( chatRoomInfo ));
            });
        }

        private delegate void invokeDelegate();
        private void invokeFunc(invokeDelegate func)
        {
            if (!this.IsHandleCreated)
                return;

            this.Invoke(new MethodInvoker(func));
        }

        private void RemoveRoomList(ChatRoom chatRoomInfo)
        {
            invokeFunc(() =>
            {
                for (int idx = 0; idx < roomList.Count; ++idx) // 존재하는 리스트 여부 검사
                {
                    if (chatRoomInfo.Index == roomList[idx].Index)
                    {
                        roomList.RemoveAt(idx);
                        this.ListBox_RoomList.Items.RemoveAt(idx);
                        break;
                    }
                }
            });
        }

        public void CloseAllChatScene()
        {
            int size = ParticipatedChatRoomList.Count;
            if (0 >= size)
                return;

            Client.Client client = Launcher.Instance.GetClient();

            List<ChatRoomScene> tmpList = new List<ChatRoomScene>(size);

            foreach (var chatRoomScene in ParticipatedChatRoomList)
            {
                tmpList.Add(chatRoomScene.Value);
                client.SendPacket(new CN_LEAVECHATROOM() { roomIdx = chatRoomScene.Key });
            }

            for (int idx = 0; idx < size; ++idx)
            {
                tmpList[0].CloseScene();
                tmpList[0].Dispose();
                tmpList.RemoveAt(0);
            }
        }

        private void enterRoom()
        {
            if (ListBox_RoomList.Items.Count <= 0)
                return;

            if (ListBox_RoomList.SelectedIndex < 0)
                return;

            var client = Launcher.Instance.GetClient();
            if (client == null)
                return;

            if( roomList.Count < ListBox_RoomList.SelectedIndex ) { return; }
            CQ_ENTERCHATROOM req = new CQ_ENTERCHATROOM();
            req.RoomIdx = roomList[ListBox_RoomList.SelectedIndex].Index;
            client.SendPacket(req);
        }

        private void Btn_ChangeNickname_Click(object sender, EventArgs e)
        {
            ChangeNicknameScene nicknameChange = new ChangeNicknameScene();
            nicknameChange.ShowDialog();
        }

        private void Btn_EnterChatRoom_Click(object sender, EventArgs e)
        {
            this.enterRoom();
        }

        private void DoubleClick_EnterRoom(object sender, EventArgs e)
        {
            this.enterRoom();
        }

        private void Btn_CreateChatRoom_Click(object sender, EventArgs e)
        {
            var client = Launcher.Instance.GetClient();
            CQ_CREATECHATROOM req = new CQ_CREATECHATROOM();
            req.chatRoomInfo.Title = "";
            //req.chatRoomInfo.Index = 
            client.SendPacket(req);
        }
    }
}
