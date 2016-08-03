using Client;
using System;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Launcher.Instance.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Launcher.Instance.SendPacket();
        }
    }
}
