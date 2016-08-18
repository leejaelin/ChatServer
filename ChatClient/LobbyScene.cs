using ChatClient.Library.MyScene;
using ChatClient.Client;
using ShareData;
using ShareData.Data.Room;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

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
        public Dictionary<int, ChatRoomScene> ChatRoomSceneList { get; set; }
        public LobbyScene()
        {
            InitializeComponent();
            m_UIThread = new Thread(new ThreadStart(ThreadProc));
            m_UIThread.Start();
            ChatRoomSceneList = new Dictionary<int, ChatRoomScene>();
        }

        private void ThreadProc()
        {

        }

        public override void OnEntry()
        {
        }

        public void AddChatRoom( ChatRoom chatRoom )
        {
            Thread newThread = new Thread(new ThreadStart(() => 
            {
                ChatRoomScene newChatRoom = new ChatRoomScene(chatRoom);
                if (ChatRoomSceneList.ContainsKey(chatRoom.Index))
                    return;

                ChatRoomSceneList.Add(chatRoom.Index, newChatRoom);
                AddListBox(chatRoom);
                Application.Run(newChatRoom);
                
                // 삭제
                ChatRoomSceneList.Remove(chatRoom.Index);
                newChatRoom = null;
            }));
            newThread.Start();
        }

        public void RefreshChatRoomList( SN_CHATROOMLIST.E_TYPE type, Dictionary<int, ChatRoom> roomList )
        {
            switch (type)
            {
                case ShareData.SN_CHATROOMLIST.E_TYPE.ADD_LIST:
                    {
                        foreach (var chatRoom in roomList)
                            AddListBox(chatRoom.Value);
                    }
                    break;
                case ShareData.SN_CHATROOMLIST.E_TYPE.DEL_LIST:
                    break;
            }
        }

        private void AddListBox(ChatRoom chatRoomInfo)
        {
            this.Invoke(new MethodInvoker(() => 
            {
                bool isExist = false;
                foreach (ChatRoom chatRoom in this.ListBox_RoomList.Items)
                {
                    if (chatRoomInfo.Index == chatRoom.Index)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (isExist)
                    return;

                this.ListBox_RoomList.Items.Add((object)chatRoomInfo); 
            }));
        }

        private void DelListBox(ChatRoom chatRoomInfo)
        {
            this.Invoke(new MethodInvoker(() => 
            {
                int listBoxIdx = 0;
                foreach( ChatRoom chatRoom in this.ListBox_RoomList.Items )
                {
                    if( chatRoomInfo.Index == chatRoom.Index )
                        break;
                    listBoxIdx++;
                }
                this.ListBox_RoomList.Items.RemoveAt(listBoxIdx);
            }));
        }

        public void CloseAllChatScene()
        {
            int size = ChatRoomSceneList.Count;
            if (0 >= size)
                return;

            List<ChatRoomScene> tmpList = new List<ChatRoomScene>(size);

            foreach( var chatRoomScene in ChatRoomSceneList )
            {
                tmpList.Add(chatRoomScene.Value);
            }

            for (int idx = 0; idx < size; ++idx)
            {
                tmpList[0].CloseScene();
                tmpList[0].Dispose();
                tmpList.RemoveAt(0);
            }

            //try
            //{
            //    foreach (var chatRoom in ChatRoomSceneList)
            //    {
            //        chatRoom.Value.CloseScene();
            //        chatRoom.Value.Dispose();
            //        if (null == ChatRoomSceneList)
            //            break;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
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

            CQ_ENTERCHATROOM req = new CQ_ENTERCHATROOM();
            req.RoomIdx = ListBox_RoomList.SelectedIndex;
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
            CQ_CREATECHATROOM req = new CQ_CREATECHATROOM();

            req.chatRoomInfo.Title = "";
            var client = Launcher.Instance.GetClient();
            client.SendPacket(req);
        }
    }
}
