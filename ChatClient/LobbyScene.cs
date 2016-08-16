using ChatClient.Library.MyScene;
using Client;
using ShareData;
using ShareData.Data.Room;
using System;
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
            ListBox_RoomList.Items.Add(new object());
            ListBox_RoomList.Items.Add(new object());
            ListBox_RoomList.Items.Add(new object());
            ListBox_RoomList.Items.Add(new object());
        }

        public void AddChatRoom( int roomIdx )
        {
            Thread newThread = new Thread(new ThreadStart(() => 
            {
                ChatRoomScene newChatRoom = new ChatRoomScene();
                ChatRoomSceneList.Add(roomIdx, newChatRoom);
                Application.Run(newChatRoom);
            }));
            newThread.Start();
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    sendChat();
        //}

        //public RichTextBox GetTextMain()
        //{
        //    return this.TextReadChatMain;
        //}

        //public delegate void dgtSetTextBox(String s);
        //public void RefreshTextBox(String s)
        //{
        //    if (TextReadChatMain.InvokeRequired)
        //    {
        //        dgtSetTextBox stb = new dgtSetTextBox(RefreshTextBox);
        //        this.Invoke(stb, new object[] { s });
        //    }
        //    else
        //    {
        //        if (TextReadChatMain.Text.Length != 0)
        //            TextReadChatMain.Text += "\n";
        //        TextReadChatMain.Text += s;
        //        TextReadChatMain.SelectionStart = TextReadChatMain.Text.Length;
        //        TextReadChatMain.ScrollToCaret();
        //    }
        //}

        //private void KeyDown( object sender, KeyEventArgs e )
        //{
        //    if( e.KeyCode == Keys.Enter )
        //    {
        //        sendChat();
        //    }
        //}
        
        //private void sendChat()
        //{
        //    if (Text_WriteChatMain.Text.Length == 0)
        //        return;

        //    CQ_CHAT req = new CQ_CHAT();
        //    req.RoomIdx = 0;
        //    req.MsgStr = Text_WriteChatMain.Text;
        //    Text_WriteChatMain.Clear();

        //    var client = Launcher.Instance.GetClient();
        //    if (client != null)
        //        client.SendPacket(req);
        //}

        private void Btn_ChangeNickname_Click(object sender, EventArgs e)
        {
            ChangeNicknameScene nicknameChange = new ChangeNicknameScene();
            nicknameChange.ShowDialog();
        }

        private void Btn_EnterChatRoom_Click(object sender, EventArgs e)
        {
            if (ListBox_RoomList.Items.Count <= 0)
                return;

            if (ListBox_RoomList.SelectedIndex < 0)
                return;

            var client = Launcher.Instance.GetClient();
            if( client == null )
                return;

            CQ_ENTERCHATROOM req = new CQ_ENTERCHATROOM();
            req.RoomIdx = ListBox_RoomList.SelectedIndex;
            client.SendPacket(req);
        }

        private void Btn_CreateChatRoom_Click(object sender, EventArgs e)
        {

        }
    }
}
