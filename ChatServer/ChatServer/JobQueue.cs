using System;
using System.Collections.Generic;

using ChatServer.Lib.Singleton;
using ShareData;
using ShareData.Message;
using System.Threading;

namespace ChatServer.ChatServer
{
    class JobQueue : Singleton<JobQueue> 
    {
        private List<Message> m_JobQueue;
        private readonly object m_Lock;

        public JobQueue()
        {
            m_JobQueue = new List<Message>();
            m_Lock = new object();
        }

        public void TryPushBack( Message message )
        {
            //m_mutex.WaitOne();
            //try
            //{
            lock( m_Lock )
            {
                m_JobQueue.Add(message);
            }
            //}
            //catch (Exception e) { }
            //finally 
            //{
            //    m_mutex.ReleaseMutex();
            //}
        }


        public void itQueue()
        {
            lock (m_Lock)
            {
                foreach (var q in m_JobQueue)
                {
                    Packet p = (Packet)q.GetValue();
                    TestPacket t = (TestPacket)p;
                    Console.WriteLine(t.testString);
                }

                m_JobQueue.Clear();
            }
        }
    }
}
