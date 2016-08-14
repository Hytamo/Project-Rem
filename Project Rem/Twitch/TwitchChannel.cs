using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_Rem.Twitch
{
    class TwitchChannel
    {
        List<TwitchRoom> Rooms;
        bool Connected;
        private ChannelType MyType;

        /// <summary>
        /// TCP client handle
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// Network stream handle
        /// </summary>
        private NetworkStream nStream;

        /// <summary>
        ///  Twitch's internet address
        /// </summary>
        private String Url;

        /// <summary>
        /// Twitch's port connection
        /// </summary>
        private Int32 Port;

        /// <summary>
        /// Twitch read/write data
        /// </summary>
        private Byte[] dataBuffer;


        private TwitchChannel() { }

        public TwitchChannel(string channelName, int channelPort, ChannelType channelType)
        {
            Connected = false;
            Rooms = new List<TwitchRoom>();
            MyType = channelType;
            Url = channelName;
            Port = channelPort;
        }

        public ChannelType GetChannelType()
        {
            return MyType;
        }

        public List<string> GetRoomList()
        {
            List<string> toReturn = new List<string>();
            foreach (TwitchRoom room in Rooms)
            {
                toReturn.Add(room.GetRoomName());
            }
            return toReturn;
        }

        private bool ConnectDefault(string username, string oauth)
        {
            if (Connected)
            {
                Console.WriteLine("Controller: Already Connected to Twitch.");
                return true;
            }

            try
            {
                Console.WriteLine("Controller: Attempting to connect to Twitch as user: " + username);
                string loginString = "PASS " + oauth + "\r\nNICK " + username + "\r\n";
                byte[] login = System.Text.Encoding.ASCII.GetBytes(loginString);
                tcpClient = new TcpClient(Url, Port);

                if (tcpClient != null)
                {
                    nStream = tcpClient.GetStream();
                    nStream.Write(login, 0, login.Length);
                    string message = string.Empty;
                    dataBuffer = new byte[512];
                    Int32 bytes = nStream.Read(dataBuffer, 0, dataBuffer.Length);
                    message = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, bytes);
                    Connected = true;
                    Console.WriteLine("Controller: Connected to Twitch as: " + username);

                    return true;
                }
                else
                {
                    Console.WriteLine("FAILURE DETECTED CONNECTING TO TWITCH");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR DETECTED CONNECTING TO TWITCH");
                Console.Write(e.ToString());
                Connected = false;
                return false;
            }
        }

        public bool Connect(string username, string oauth)
        {
            switch (MyType)
            {
                case ChannelType.Default:
                    return ConnectDefault(username, oauth);
                case ChannelType.Group:
                    //return ConnectGroup(username, oauth);
                default:
                    return false;
            }
        }

        private bool ConnectGroup(string username, string oauth)
        {
                if (Connected)
                {
                    Console.WriteLine("Controller: Already Connected to Twitch.");
                    return true;
                }

                try
                {
                    Console.WriteLine("Controller: Attempting to connect to Twitch as user: " + username);
                    string loginString = "PASS " + oauth + "\r\nNICK " + username + "\r\n";
                    byte[] login = System.Text.Encoding.ASCII.GetBytes(loginString);
                    tcpClient = new TcpClient("199.9.253.119", Port);


                if (tcpClient != null)
                    {
                        nStream = tcpClient.GetStream();
                        nStream.Write(login, 0, login.Length);
                        string message = string.Empty;
                        dataBuffer = new byte[512];
                        Int32 bytes = nStream.Read(dataBuffer, 0, dataBuffer.Length);
                        message = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, bytes);
                        Connected = true;
                        Console.WriteLine("Controller: Connected to Twitch as: " + username);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("FAILURE DETECTED CONNECTING TO TWITCH");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR DETECTED CONNECTING TO TWITCH");
                    Console.Write(e.ToString());
                    Connected = false;
                    return false;
                }
            }

        public bool JoinRoom(string roomName)
        {
            if (!Connected)
            {
                Console.WriteLine("Controller: Failure. Attempting to join room without being connected to Twitch.");
                return false;
            }
            try
            {
                roomName = "#" + roomName.ToLowerInvariant();
                if (Rooms.Where(room => room.GetRoomName().Contains(roomName)).ToList().Count > 0)
                {
                    Console.WriteLine("Controller: Failure. Attempted to join room you're already in.");
                    return true;
                }

                String joinString = "JOIN " + roomName + "\r\n";
                Byte[] join = System.Text.Encoding.ASCII.GetBytes(joinString);
                nStream.Write(join, 0, join.Length);
                Rooms.Add(new TwitchRoom(roomName));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool LeaveRoom(string roomName)
        {
            bool toReturn = false;

            return toReturn;
        }

        public void SendPriorityMessage(Message message)
        {
            if (message == null) return;

            string formattedMessage = null;
            if (!message.whisper && message.system == false && message.room != null)
            {
                if (!message.room.StartsWith("#")) message.room = "#" + message.room;
                formattedMessage = "PRIVMSG " + message.room + " :" + message.message + " \r\n";
            }
            else if (message.room != null && message.system == false)
            {
                // Whisper logic here
            }
            else
            {
                formattedMessage = message.message;
            }
            byte[] toSend = System.Text.Encoding.ASCII.GetBytes(formattedMessage);
            nStream.Write(toSend, 0, toSend.Length);
            Console.WriteLine("Sending: " + DateTime.Now + " - " + message.room + " : " + message.message);
        }

        public bool IsConnected()
        {
            return Connected;
        }

        private bool ParseSystemMessage(string message, out string returnMessage)
        {
            returnMessage = null;
            bool toReturn = false;

            // Ping Pong
            if (message.Contains("PING :tmi.twitch.tv"))
            {
                returnMessage = "PONG :tmi.twitch.tv\r\n";
                return true;
            }

            // Room Joins
            foreach (TwitchRoom room in Rooms)
            {
                if (message.Contains("JOIN " + room.GetRoomName()))
                {
                    Console.WriteLine("Controller: Successfully joined room: " + room.GetRoomName() + ".");
                    return true;
                }
            }

            if (message.Contains("End of /NAMES list"))
            {
                // parse users here
                return true;
            }
            return toReturn;
        }

        public Message ReadMessages()
        {
            byte[] readBuffer = new byte[1024];
            StringBuilder completeMessage = new StringBuilder();
            Int32 bytesRead = 0;

            // If data is available, let's read it.
            if (nStream.DataAvailable)
            {
                // Set variables to initial states.
                try
                {
                    bytesRead = nStream.Read(readBuffer, 0, readBuffer.Length);
                    completeMessage.AppendFormat("{0}", Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
                    string messageAsString = completeMessage.ToString();
                    if (string.IsNullOrEmpty(messageAsString)) throw new Exception();

                    // Fill out Message here with Message Parser
                    string sysReturnMsg;
                    if (!ParseSystemMessage(messageAsString, out sysReturnMsg))
                    {
                        string room = MessageParser.ParseOriginRoom(messageAsString);
                        string message = MessageParser.ParseRawMessage(messageAsString);
                        string sender = MessageParser.ParseMessageSender(messageAsString);
                        bool whisper = MessageParser.ParseWhisperState(messageAsString);
                        Console.WriteLine(DateTime.Now.ToString() + " #" + room + " : " + sender + " : " + message);
                        return new Message(message, room, whisper, sender);
                    }
                    return (sysReturnMsg == null) ? null : new Message(sysReturnMsg, null, false, null, true);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Controller: Error detected while trying to read messages: " + exc.Message);
                    return null;
                }
            }
            return null;
        }
    }
}
