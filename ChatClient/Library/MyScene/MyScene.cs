using System.Windows.Forms;

namespace ChatClient.Library.MyScene
{
    public class MyScene : Form
    {
        public virtual void OnEntry() { }
        public virtual void CloseScene() { } // 폼을 닫기 위해 사용되는 virtual 메서드
    }
}
