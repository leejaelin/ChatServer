using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace ClientBot
{
    class Program
    {
        static int botCnt = 0;

        static void Main(string[] args)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            const int ThreadCount = 1;
            Thread[] thread = new Thread[ThreadCount];
            for (int i = 0; i < ThreadCount; i++)
            {
                thread[i] = new Thread(new ParameterizedThreadStart(
                (idx) =>
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        socket.Connect(ipEndPoint);

                        ShareData.TestPacket noti = new ShareData.TestPacket();
                        noti.idx = (int)idx;
                        noti.testIdx = (int)idx * 100;
                        noti.testString = "테스트봇" + botCnt + "번 입니다.";
                        Console.WriteLine(botCnt++);
                        using (MemoryStream _stream = new MemoryStream())
                        {
                            MemoryStream mStream = new MemoryStream();
                            BinaryFormatter _binaryFormatter = new BinaryFormatter();
                            _binaryFormatter.Serialize(mStream, noti);
                            socket.EndSend(new IAsyncResult(() => {
                            }));
                            socket.Send(mStream.ToArray());
                        }

                        //Byte[] _data = new Byte[1024];
                        //socket.Receive(_data);

                        //MemoryStream stream = new MemoryStream(_data);
                        //BinaryFormatter binaryFormatter = new BinaryFormatter();
                        //stream.Position = 0;
                        //Object obj = binaryFormatter.Deserialize(stream);
                        //ShareData.TestPacket req = (ShareData.TestPacket)obj;
                        //Console.WriteLine(req.testString);


                        socket.Disconnect(false);
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        socket.Dispose();
                    }
                }
                ));
            }

            foreach (var t in thread)
            {
                t.Start(t.ManagedThreadId);
            }
        }
    }
}
