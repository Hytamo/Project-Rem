using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_Rem.Twitch
{
    class TwitchCore
    {
        TcpClient socket;

        NetworkStream socketstream;

        StreamWriter streamwriter;

        object readlocker;
        object connectionlocker;
        public TwitchCore()
        {
            socket = new TcpClient();
            readlocker = new object();
            connectionlocker = new object();

        }

        public void Connect()
        {
            if (socket != null) socket.Connect("irc.twitch.tv", 80);
            if (!socket.Connected){ throw new Exception("Connection Failure."); }
            socketstream = socket.GetStream();

            streamwriter = new StreamWriter(socketstream);
            streamwriter.Write("PASS " + "oauth:jumjklxvmvhgi6s4ae93ib5v8cyt4w" + "\r\nNICK " + "remubot" + "\r\n");
            streamwriter.Flush();

            Task task = new Task(new Action(ReadMessages));
            task.Start();
        }

        public bool IsConnected()
        {
            lock(connectionlocker){ return socket != null && socket.Connected && socketstream != null; }
        }

        private void ReadMessages()
        {
            while (IsConnected())
            {
                byte[] readbuffer = new byte[1024];
                StringBuilder completemessage = new StringBuilder();
                StreamReader reader = new StreamReader(socketstream);

                // If data is available, let's read it.
                if (socketstream != null && socketstream.DataAvailable)
                {
                    string messageAsString = reader.ReadLine();

          //          bytesread = socketstream.Read(readbuffer, 0, readbuffer.Length);
          //          completemessage.AppendFormat("{0}", Encoding.ASCII.GetString(readbuffer, 0, bytesread));
          //          string messageAsString = completemessage.ToString();

                    // Handle System Messages

                    // Call Message Received Handler

                }
            }
        }
    }
}
