using System;
using System.IO;
using System.Text;

namespace ShareData.CommonLogic.Log
{
    public class LogMaker
    {
        public LogMaker() {}

        public void File( string message )
        {
            string path = Directory.GetCurrentDirectory() + "\\Log.txt";
            FileStream fileStream = new FileStream(path, FileMode.Append);

            Byte[] logByte = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + message + "\r\n");
            fileStream.Write( logByte, 0, logByte.GetLength(0) );
            fileStream.Close();
        }
    }
}
