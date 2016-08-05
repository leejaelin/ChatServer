using Client;
using ShareData;
using System;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form2 : Form
    {
        public Form2()
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
            req.nickname = this.textBox1.Text;
            Launcher.Instance.GetClient().SendPacket(req);
            this.Close();
        }
    }
}
