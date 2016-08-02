using Client;
using System;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        private Launcher launcher;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            launcher = new Launcher();
            launcher.Init(Launcher.E_MODE.BOT, 1000);
            launcher.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (launcher == null)
                return;

            launcher.SendPacket();
        }
    }
}
