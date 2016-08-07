using BotClient.BotClient;
using System;
using System.Windows.Forms;

namespace BotClient
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

        public Form1()
        {
            InitializeComponent();
        }

        public RichTextBox GetTextMain()
        {
            return this.TextReadChatMain;
        }

        public delegate void dgtSetTextBox(string s);
        public void RefreshTextBox(string s)
        {
            try
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
            catch(Exception /*e*/)
            {
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Launcher.Instance.Init((int)numericUpDown1.Value);
            Launcher.Instance.Start();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Launcher.Instance.BotClose();
        }
    }
}
