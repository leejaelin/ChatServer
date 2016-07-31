using System.Collections.Generic;

namespace ShareData.CommonLogic.JobQueue
{
    public class JobQueue
    {
        //#region Singleton
        //private static JobQueue instance;
        //public static JobQueue Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = new JobQueue();
        //        return instance;
        //    }
        //    private set { instance = value; }
        //}
        //#endregion

        private List<Message.Message> m_JobQueue;
        private readonly object m_Lock;

        public JobQueue()
        {
            m_JobQueue = new List<Message.Message>();
            m_Lock = new object();
        }

        public void TryPushBack(Message.Message message)
        {
            lock (m_Lock)
            {
                m_JobQueue.Add(message);
            }
        }

        public Message.Message TryPopFront()
        {
            lock (m_Lock)
            {
                if (m_JobQueue.Count <= 0)
                    return null;

                Message.Message msg = m_JobQueue[0];
                m_JobQueue.RemoveAt(0);
                return msg;
            }
        }
    }
}
