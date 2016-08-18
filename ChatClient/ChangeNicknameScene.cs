using ChatClient.Library.MyScene;
using ChatClient.Client;
using ShareData;
using System;

namespace ChatClient
{
    public partial class ChangeNicknameScene : MyScene
    {
        public ChangeNicknameScene()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CQ_CHANGENICKNAME req = new CQ_CHANGENICKNAME();
            req.nickname = this.TextBox_Nickname.Text;
            Launcher.Instance.GetClient().SendPacket(req);
            this.Close();
        }
    }
}
