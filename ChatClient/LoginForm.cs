using Client;
using System;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text_ID.Enabled = false;
            this.Text_PW.Enabled = false;
            this.Btn_Login.Enabled = false;
            Launcher.Instance.Start();
        }
    }
}
