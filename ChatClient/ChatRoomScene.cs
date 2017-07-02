using ChatClient.Library.MyScene;
using ChatClient.Client;
using System;
using System.Windows.Forms;
using ShareData;
using ShareData.Data.Room;
using System.Collections.Generic;

namespace ChatClient
{
    public partial class ChatRoomScene : MyScene
    {
        private Client.Client client; // 세션 및 소켓 정보
        private ChatRoom chatRoomInfo;  // 현재 방의 정보
        private List<uint> userList; // 유저 목록 순서
        public ChatRoomScene(ChatRoom chatRoom)
        {
            InitializeComponent();
            client = Launcher.Instance.GetClient();
            chatRoomInfo = chatRoom;
            init();
        }

        private void init()
        {
            userList = new List<uint>();
            foreach (ChatRoomUserInfo chatUser in chatRoomInfo.RoomUserList.Values)
            {
                userList.Add(chatUser.userIndex);
                this.listBox_UserList.Items.Add(chatUser.userNickname);
            }
        }

        private delegate void invokeDelegate();
        private void invokeFunc(invokeDelegate func)
        {
            if (!this.IsHandleCreated)
                return;

            this.Invoke(new MethodInvoker(func));
        }

        public void AddUserList(ChatRoom chatRoom)
        { 
            invokeFunc(() =>
            {
                userList.Clear();
                this.listBox_UserList.Items.Clear();
                foreach (ChatRoomUserInfo chatUser in chatRoom.RoomUserList.Values)
                {
                    //userList.Add(chatUser.userIndex);
                    //this.listBox_UserList.Items.Add(chatUser.userNickname + chatUser.userIndex);
                    //bool isExist = false;
                    //foreach (uint idx in userList)
                    //{
                    //    if (chatUser.userIndex == idx)
                    //    {
                    //        continue;
                    //    }

                    //}
                    userList.Add(chatUser.userIndex);
                    this.listBox_UserList.Items.Add(chatUser.userNickname);
                }
            });
        }

        public void RemoveUserList(ChatRoom chatRoom)
        {
            invokeFunc(() =>
            {
                foreach (ChatRoomUserInfo chatUser in chatRoom.RoomUserList.Values)
                {
                    for (int idx = 0; idx < userList.Count; ++idx)
                    {
                        if (chatUser.userIndex == userList[idx])
                        {
                            userList.RemoveAt(idx);
                            this.listBox_UserList.Items.RemoveAt(idx);
                            break;
                        }
                    }
                }
            });
        }

        public override void CloseScene()
        {
            try
            {
                this.Invoke(new MethodInvoker(() => { this.Close(); }));
            }
            catch (Exception /*e*/) { }
        }

        private void forceToCreateHandle() // 내부적으로 핸들이 없는 경우 강제로 만들도록 예외 처리
        {
            IntPtr p;
            if (!this.IsHandleCreated)
                p = this.Handle;
        }

        private void KeyDown_SendChat(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            CQ_CHAT req = new CQ_CHAT();
            req.RoomIdx = chatRoomInfo.Index;
            req.MsgStr = this.TextBox_WriteChat.Text;
            this.TextBox_WriteChat.Clear();
            client.SendPacket(req);
        }

        public void RecvChatMessage( String message )
        {
            this.Invoke( new MethodInvoker(() => {
                if (this.RichTextBox_ReadChat.Text.Length != 0)
                    this.RichTextBox_ReadChat.Text += "\n";
                this.RichTextBox_ReadChat.Text += message;
            }) );
        }

        private void KeyDown_SendChat(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            CQ_CHAT req = new CQ_CHAT();
            req.RoomIdx = chatRoomInfo.Index;
            req.MsgStr = this.TextBox_WriteChat.Text;
            this.TextBox_WriteChat.Clear();
            client.SendPacket(req);

            e.Handled = true;
        }
    }
}
