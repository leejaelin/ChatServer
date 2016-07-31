using System;
using ChatServer.Data.User;
using ShareData.Message;

namespace ChatServer.ChatServer.Process
{
    class ProcedureProcess : IProcess
    {
        #region Singleton
        private static ProcedureProcess m_instance;
        public static ProcedureProcess Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ProcedureProcess();
                return m_instance;
            }
        }
        #endregion

        public ProcedureProcess() { }
        ~ProcedureProcess() { }

        public void MsgProcess(User user, Message message)
        {
            
        }
    }
}
