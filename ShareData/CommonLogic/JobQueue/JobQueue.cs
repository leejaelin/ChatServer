using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShareData.CommonLogic.JobQueue
{
    public class JobQueue
    {
        private int inCnt = 0;
        private int outCnt = 0;
        private ConcurrentQueue<Message.Message> m_JobQueue;
        private readonly object m_Lock;

        public JobQueue()
        {
            m_JobQueue = new ConcurrentQueue<Message.Message>();
            m_Lock = new object();
        }

        public void TryPushBack(Message.Message message)
        {
            inCnt++;
            m_JobQueue.Enqueue(message);
          
            // wait 상태의 메인 스레드를 깨우는 호출
        }

        public Message.Message TryPopFront()
        {
            if (m_JobQueue.Count <= 0)
                return null;
            outCnt++;

            Message.Message msg;
            m_JobQueue.TryDequeue(out msg);

            PrintQueueState();
            return msg;
        }

        public int GetTryGetQueueCount()
        {
            return m_JobQueue.Count;
        }

        public void PrintQueueState()
        {
            Debug.WriteLine("[ IN:{0} / OUT:{1} ]", inCnt, outCnt);
        }
    }
}
