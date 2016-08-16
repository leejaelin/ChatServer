using System.Collections.Generic;
using System.Windows.Forms;

namespace ChatClient.Client.Scene
{
    class SceneManager
    {
        #region Singleton
        private static SceneManager m_instance;
        public static SceneManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SceneManager();
                return m_instance;
            }
        }
        #endregion

        private Form currentForm;
        public Dictionary<int, Form> ChatFormList { get; set; }
        public SceneManager()
        {
            ChatFormList = new Dictionary<int, Form>();
        }
        
        public void RunLoginForm()
        {
            LoginForm loginForm = new LoginForm();
            currentForm = loginForm;
            Application.Run(loginForm);
        }

        public void RunLobbyForm()
        {
            LobbyForm lobbyForm = new LobbyForm();
            currentForm = lobbyForm;
            Application.Run(lobbyForm);

        }
    }
}
