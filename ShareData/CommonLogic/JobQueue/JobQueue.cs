using System;
using System.Collections.Generic;

using ShareData;
using ShareData.Message;
using System.Threading;

namespace ShareData.CommonLogic.JobQueue
{
    class JobQueue
    {
        #region Singleton
        public static JobQueue Instance
        {
            private set { Instance = value; }
            get
            {
                if (Instance == null)
                    Instance = new JobQueue();
                return Instance;
            }
        }
        #endregion

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

        public void TryPopFront()
        {
            lock (m_Lock)
            {
                while (m_JobQueue.Count > 0)
                {
                    Message.Message msg = m_JobQueue[0];
                    m_JobQueue.RemoveAt(0);

                    // msg가 큐 맨앞에 있던 메시지 이다.
                    // 이 곳에서 분기 태워서 처리한다.
                }
            }
        }
    }
}
