using Client;
using ShareData;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        #region Singleton
        private static Form1 m_instance;
        public static Form1 Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Form1();
                return m_instance;
            }
        }
        #endregion

        private Thread m_UIThread;
        public Form1()
        {
            InitializeComponent();
            m_UIThread = new Thread(new ThreadStart(ThreadProc));
            m_UIThread.Start();
        }

        private void ThreadProc()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Launcher.Instance.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendChat();
        }

        public RichTextBox GetTextMain()
        {
            return this.TextReadChatMain;
        }

        public delegate void dgtSetTextBox(String s);
        public void RefreshTextBox(String s)
        {
            if (TextReadChatMain.InvokeRequired)
            {
                dgtSetTextBox stb = new dgtSetTextBox(RefreshTextBox);
                this.Invoke(stb, new object[] { s });
            }
            else
            {
                if (TextReadChatMain.Text.Length != 0)
                    TextReadChatMain.Text += "\n";
                TextReadChatMain.Text += s;
                TextReadChatMain.SelectionStart = TextReadChatMain.Text.Length;
                TextReadChatMain.ScrollToCaret();
            }
        }

        private void KeyDown( object sender, KeyEventArgs e )
        {
            if( e.KeyCode == Keys.Enter )
            {
                sendChat();
            }
        }
        
        private void sendChat()
        {
            if (TextWriteChatMain.Text.Length == 0)
                return;

            CQ_CHAT req = new CQ_CHAT();
            req.RoomIdx = 0;
            req.MsgStr = TextWriteChatMain.Text;
            TextWriteChatMain.Clear();

            var client = Launcher.Instance.GetClient();
            if (client != null)
                client.SendPacket(req);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
