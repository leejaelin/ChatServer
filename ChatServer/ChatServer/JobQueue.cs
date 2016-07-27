using System;
using System.Collections.Generic;

using ChatServer.Lib.Singleton;
using ShareData;
using ShareData.Message;
using System.Threading;
using ChatServer.Data.User;

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
            lock( m_Lock )
            {
                m_JobQueue.Add(message);
            }
        }
        
        public void PopQueue()
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
