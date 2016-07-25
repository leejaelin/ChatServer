using System.Net.Sockets;

namespace ChatServer.Data.User
{
    class User
    {
        public User( int ipAddr, int ConnIdx, Socket socket )
        {
            this.ipAddr = ipAddr;
            this.ConnIdx = ConnIdx;
            buffer = new byte[4096];
            clientSocket = socket;
        }
        private int ipAddr { get; set; }
        private int ConnIdx { get; set; }
        private byte[] buffer { get; set; }
        private Socket clientSocket { get; set; }

        public int GetIndex() { return ConnIdx; }
        public byte[] GetBuffer() { return buffer; }
        public Socket GetSocket() { return clientSocket; }

    }
}
