using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ShareData;
using ShareData.CommonLogic.Network;

namespace ClientBot.Client
{
    class Client : Network
    {
        public Client( int idx ) : base()
        {
            m_idx = 0;
        }

        // member variables
        private int m_idx;
        
        public int GetIdx() { return m_idx; }

        public void SendPacket(Packet packet)
        {
          
        }

        public void Do()
        {

        }
    };
}
