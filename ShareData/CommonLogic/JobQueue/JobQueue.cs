using System;
using System.Collections.Concurrent;
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
        private int inVal = 0;
        private int outVal = 0;
        private ConcurrentQueue<Message.Message> m_JobQueue;
        //private List<Message.Message> m_JobQueue;
        private readonly object m_Lock;

        public JobQueue()
        {
            m_JobQueue = new ConcurrentQueue<Message.Message>();
            //m_JobQueue = new List<Message.Message>();
            m_Lock = new object();
        }

        public void TryPushBack(Message.Message message)
        {
            inVal++;
            m_JobQueue.Enqueue(message);
            //lock (m_Lock)
            //{
            //    m_JobQueue.Add(message);
            //}
            
            // wait 상태의 메인 스레드를 깨우는 호출
            PrintQueueState();

        }

        public Message.Message TryPopFront()
        {
            if (m_JobQueue.Count <= 0)
                return null;

            outVal++;
            Message.Message msg;
            m_JobQueue.TryDequeue(out msg);

            PrintQueueState();

            return msg;

            //lock (m_Lock)
            //{
            //    if (m_JobQueue.Count <= 0)
            //        return null;

            //    Message.Message msg = m_JobQueue[0];
            //    m_JobQueue.RemoveAt(0);
            //    return msg;
            //}
        }

        public int GetTryGetQueueCount()
        {
            //lock (m_Lock)
            //    return m_JobQueue.Count;
            return m_JobQueue.Count;
        }

        public void PrintQueueState()
        {
            //Console.WriteLine("[IN:{0} / OU:{1}]", inVal, outVal);
        }
    }
}
