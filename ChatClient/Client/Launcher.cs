using ShareData;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Client
{
    class Launcher
    {
        #region Singleton
        private static Launcher m_instance;
        public static Launcher Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Launcher();
                return m_instance;
            }
        }
        #endregion

        private Thread workerThread;
        private bool isWorkerThread;
        private AutoResetEvent m_MainThreadEventHandler;
        private Client client;
        public Launcher()
        {
            isWorkerThread = true;
            m_MainThreadEventHandler = new AutoResetEvent(true);
            client = null;
        }

        // member methods
        public void Start()
        {
            if (null != workerThread)
                return;

            helloMessage();
            startStandAlone();
            createThread();
        }

        private void createThread()
        {
            isWorkerThread = true;
            workerThread = new Thread(Jobloop);
            workerThread.Start();
        }
        public void Jobloop()
        {
            do 
            {
                if (!Do())
                {
                    m_MainThreadEventHandler.WaitOne();
                }
            }
            while(isWorkerThread);
            workerThread = null;
        }
        
        public bool Do()
        {
            return client.Do();
        }

        public Client GetClient()
        {
            return client;
        }

        public void SendPacket()
        {
            ShareData.CQ_CHAT noti = new ShareData.CQ_CHAT();
            noti.MsgStr = "안녕? Hello Server!!";
            client.SendPacket(noti);
        }

        public void ReleaseJobLoop()
        {
            m_MainThreadEventHandler.Set();
        }

        public void TerminateProcess()
        {
            isWorkerThread = false;
            ReleaseJobLoop();
        }

        private void startStandAlone()
        {
            createClient();
        }

        private void createClient()
        {
            // Client 생성
            client = new Client();
        }

        private void helloMessage()
        {
            Console.WriteLine("******Client Start******");
            Console.WriteLine("******StandAlone MODE******");
        }
    }
}
