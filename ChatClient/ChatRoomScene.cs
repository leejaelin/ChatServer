using ChatClient.Library.MyScene;
using ChatClient.Client;
using System;
using System.Windows.Forms;
using ShareData;
using ShareData.Data.Room;

namespace ChatClient
{
    public partial class ChatRoomScene : MyScene
    {
        private ChatClient.Client.Client client; // 세션 및 소켓 정보
        private ChatRoom chatRoomInfo;  // 현재 방의 정보
        public ChatRoomScene( ChatRoom chatRoom )
        {
            InitializeComponent();
            client = Launcher.Instance.GetClient();
            chatRoomInfo = chatRoom;
        }

        public override void CloseScene()
        {
          //  forceToCreateHandle();
            this.Invoke(new MethodInvoker(() => { this.Close(); }));
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
    }
}
