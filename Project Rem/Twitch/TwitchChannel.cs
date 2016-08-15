using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

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

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void ChatLog(Message message);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public ChatLog ChatLogHandler;


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

        public void LogInfo(Message message, int priority = 0)
        {
            List<Message> toSend = new List<Message>();
            toSend.Add(message);
            SendPriorityMessage(message);
            ChatLogHandler(message);
        }

        private bool ConnectDefault(string username, string oauth)
        {
            try
            {
                List<Message> toSend = new List<Message>();
                LogInfo(new Message("Controller: Attempting to connect to Twitch as user: " + username, "System", null, false, "void", true));
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
                    LogInfo(new Message("Controller: Connected to Twitch as: " + username, "System", null, false, "void", true));
                    CreateSystemRoom();

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

        private void CreateSystemRoom()
        {
            if (Rooms.Where(r => r.GetRoomName().Contains("system")).ToList().Count > 0)
            {
                return;
            }

            TwitchRoom system = new TwitchRoom("System");
            Rooms.Add(system);
        }

        public bool JoinRoom(string roomName)
        {
            if (!Connected)
            {
                LogInfo(new Message("Controller: Failure. Attempting to join room without being connected to Twitch.", "system", null, false, "void", true));
                return false;
            }
            try
            {
                if (Rooms.Where(room => room.GetRoomName().ToLowerInvariant().Contains(roomName.ToLowerInvariant())).ToList().Count > 0)
                {
                    LogInfo(new Message("Controller: Failure. Attempted to join room you're already in.", "system", null, false, "void", true));
                    return false;
                }
                roomName = "#" + roomName.ToLowerInvariant();
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
            SendPriorityMessage(new Message("PART #" + roomName.TrimStart('#').ToLowerInvariant(), "System", null, false, null, true));
            return toReturn;
        }

        public void SendPriorityMessage(Message message)
        {
            if (message == null) return;

            if (message.whisperTarget != null && message.whisperTarget.Contains("void"))
            {
                GetRoom(message).LogMessage(message);
                return;
            }

            string sysReturnMsg;
            if (ParseSystemMessage(message.message, out sysReturnMsg))
            {
                if (sysReturnMsg != null)
                {
                    message.message = sysReturnMsg;
                }
            }

            string formattedMessage = null;
            if (!message.whisper && message.system == false && message.room != null)
            {
                message.room = message.room.ToLowerInvariant();
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
            string roomLookup = (message.room == null) ? "System" : message.room;
            GetRoom(message).LogMessage(message);
        }

        public List<TwitchRoom> GetAllRooms()
        {
            return Rooms;
        }

        public TwitchRoom GetRoom(Message message)
        {
            if (message.system == true)
            {
                if (message.room == null)
                {
                    message.room = "System";
                }
            }

            message.room = message.room.Replace("#", "");

            foreach (TwitchRoom room in Rooms)
            {
                if (room.GetRoomName().ToLowerInvariant() == message.room.ToLowerInvariant())
                {
                    return room;
                }
            }

            TwitchRoom newRoom = new TwitchRoom(message.room);
            Rooms.Add(newRoom);
            return newRoom;
        }

        public bool IsConnected()
        {
            return Connected;
        }

        private bool ParseSystemMessage(string message, out string returnMessage)
        {
            returnMessage = null;

            // Ping Pong
            if (message.Contains("PING :tmi.twitch.tv"))
            {
                returnMessage = "PONG :tmi.twitch.tv\r\n";
                return true;
            }

            // Room/Part Joins
            foreach (TwitchRoom room in Rooms)
            {
                // :remubot!remubot@remubot.tmi.twitch.tv PART #hytamoJOIN
                if (message.Contains("tmi.twitch.tv PART #" + room.GetRoomName() + "JOIN"))
                {
                    LogInfo(new Message("Controller: Successfully joined room: " + room.GetRoomName() + ".", "system", null, false, "void", true));
                    return true;
                }

                if (message.Contains("JOIN #" + room.GetRoomName()))
                {
                    LogInfo(new Message("Controller: Successfully joined room: " + room.GetRoomName() + ".", "system", null, false, "void", true));
                    return true;
                }

                if (message.Contains("PART #" + room.GetRoomName()))
                {
                    LogInfo(new Message("Controller: Successfully left room: " + room.GetRoomName() + ".", "system", null, false, "void", true));
                    Rooms.Remove(room);
                    return true;
                }
            }
            

            if (message.Contains("End of /NAMES list"))
            {
                // parse users here
                return true;
            }

            if (message.ToLowerInvariant().Contains("-disconnect"))
            {
                LogInfo(new Message("Disconnected from Twitch.", "system", null, false, "void", true));
                Connected = false;
                return true;
            }
            return false;
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
                        Message toReturn = new Message(message, "#" + room, sender);
                        GetRoom(toReturn).LogMessage(toReturn);
                        return toReturn;
                    }
                    if (sysReturnMsg != null)
                    {
                        Message toReturn = new Message(sysReturnMsg, "System", null, false, null, true);
                        return toReturn;
                    }
                    return null;
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
