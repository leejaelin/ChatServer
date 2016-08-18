using ChatClient.Library.MyScene;
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

        private bool isLoop;
        public MyScene CurrentScene { get; set; }
        public SceneManager()
        {
            isLoop = true;
            CurrentScene = new LoginScene();
        }

        public void StartScene()
        {
            while (isLoop)
            {
                isLoop = false;
                CurrentScene.OnEntry();
                Application.Run(CurrentScene);
            }
            removeChatScene(); // 열려 있던 채팅창 Close
        }
    
        public void ChangeScene( MyScene scene )
        {
            MyScene oldForm = CurrentScene;
            CurrentScene = scene;
            isLoop = true;
            oldForm.CloseScene();
        }

        private void removeChatScene()
        {
            if ( CurrentScene.GetType() == typeof(LobbyScene) )
            {
                ((LobbyScene)CurrentScene).CloseAllChatScene();
            }
        }
    }
}
