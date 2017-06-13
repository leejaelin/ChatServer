using ChatClient.Library.MyScene;
using ChatClient.Client;
using System;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class LoginScene : MyScene
    {
        public LoginScene()
        {
            InitializeComponent();
        }

        public override void OnEntry()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text_ID.Enabled = false;
            this.Text_PW.Enabled = false;
            this.Btn_Login.Enabled = false;
            Launcher.Instance.Start(this.IPtextBox.Text);
        }

        public override void CloseScene()
        {
            this.Invoke(new MethodInvoker(() => { this.Close(); }));
        }
    }
}
