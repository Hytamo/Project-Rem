using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Twitch.Parser;

namespace Twitch.Controller
{
    /// <summary>
    /// this is the main twitch controller class. this handles all functionality and features associated 
    /// with direct communication to the irc and twitch servers. all that is provided is the twitch username 
    /// and the oauth associated with that account.
    /// NOTE: all room names should be in lowercase when being passed to the twitch controller.
    /// </summary>
    public class TwitchController
    {
        /// <summary>
        /// whether or not we've been muted from default chat.
        /// </summary>
        public bool IsMuted;

        /// <summary>
        /// constructor, called on Twitch creation.
        /// </summary>
        /// <param name="botname">twitch user connection.</param>
        /// <param name="oauth">oauth password. should look like: oauth:23zakj42cvb4j2k34j2324ds.</param>
        public TwitchController(string botname, string oauth)
        {
            if (botname == null || oauth == null)
                throw new Exception("Botname or OAuth was null.");

            BotName = botname;
            OAuth = oauth;
            IsMuted = false;
            UserLists = null;
        }

        /// <summary>
        /// call this when trying to establish a primary twitch connection.
        /// all other functionality will fail until this is called.
        /// </summary>
        public void Connect()
        {
            if (IsConnected()) { Console.WriteLine("Already connected!"); return; }

            UserLists = new Dictionary<string, Dictionary<string, List<string>>>();

            Client = new TcpClient();
            Client.Connect("irc.twitch.tv", 80);
            if (!Client.Connected) { Console.WriteLine("Connection attempt failed. Please check internet connection."); }

            rooms = new TwitchRooms();

            Input = new StreamReader(Client.GetStream());
            Output = new StreamWriter(Client.GetStream());

            Output.Write("PASS " + OAuth + "\r\nNICK " + BotName + "\r\n");
            Output.Flush();

            Task task = new Task(new Action(ReadMessagesAsync));
            task.Start();

            GroupClient = new TcpClient();
            GroupClient.Connect("irc.chat.twitch.tv", 80);
            if (!GroupClient.Connected) { Console.WriteLine("Connection attempt failed. Please check internet connection."); }

            GroupInput = new StreamReader(GroupClient.GetStream());
            GroupOutput = new StreamWriter(GroupClient.GetStream());

            GroupOutput.Write("PASS " + OAuth + "\r\nNICK " + BotName + "\r\n");
            GroupOutput.Flush();

            Task Grouptask = new Task(new Action(ReadGroupMessagesAsync));
            Grouptask.Start();
        }

        /// <summary>
        /// call this when attempting to tear down the twitch controller
        /// </summary>
        public void Disconnect()
        {
            if (Client != null){ Client.Close(); Client = null; }
            if (Input != null){ Input.Close(); Input = null; }
            if (Output != null){ Output.Close(); Output = null; }

            if (GroupClient != null){ GroupClient.Close(); GroupClient = null; }
            if (GroupInput != null){ GroupInput.Close(); GroupInput = null; }
            if (GroupOutput != null){ GroupOutput.Close(); GroupOutput = null; }

            UserLists = null;
            DisconnectedHandler();
        }

        /// <summary>
        /// are we connected to the group chat servers?
        /// </summary>
        /// <returns></returns>
        public bool IsGroupConnected()
        {
            return GroupClient != null && GroupClient.Connected;
        }

        /// <summary>
        /// returns whether or not the client is connected to twitch
        /// </summary>
        /// <returns>true if connected</returns>
        public bool IsConnected()
        {
            return Client != null && Client.Connected;
        }

        /// <summary>
        /// once the client is connected, this joins a room given as a string.
        /// </summary>
        /// <param name="room">name of the desired room to join.</param>
        public void JoinRoom(string room)
        {
            if (!IsConnected()) { Console.WriteLine("Please connect to Twitch before attempting to join a room."); return; }
            Output.Write("JOIN #" + room + "\r\n");
            Output.Flush();
        }

        /// <summary>
        /// allows a user to leave a specific twitch room.
        /// </summary>
        /// <param name="roomname">the room to leave.</param>
        public void LeaveRoom(string room)
        {
            if (!IsConnected()) { Console.WriteLine("Please connect to Twitch before attempting to leave rooms. also join a room first."); return; }
            Output.Write("PART #" + room + "\r\n");
            Output.Flush();
        }

        /// <summary>
        /// this controls sending a message to twitch. it will fail if user either isn't in the current room, or twitch at all.
        /// </summary>
        /// <param name="message">the message to be sent.</param>
        /// <param name="room">if null, you wish to send a raw message, otherwise message will be sent to desired room.</param>
        public void SendMessage(string message, string room)
        {
            if (!IsConnected()) { Console.WriteLine("Please connect to Twitch before attempting to send any messages."); return; }
            if (IsMuted) return;
            message = "PRIVMSG #" + ((room == null) ? "" : room) + " :" + message + " \r\n";
            Output.Write(message);
            Output.Flush();
        }

        public void SendWhisper(string message, string user)
        {
            if (!IsConnected()) { Console.WriteLine("Please connect to Twitch before attempting to send any messages."); return; }
            if (IsMuted) return;
            message = "PRIVMSG #jtv :/w " + user + " " + message + " \r\n";
            GroupOutput.Write(message);
            GroupOutput.Flush();
        }

        /// <summary>
        /// checks whether or not the bot is in the specified room.
        /// </summary>
        /// <param name="room">name of room to check against.</param>
        /// <returns></returns>
        public bool IsInRoom(string room)
        {
            return UserLists.Where(x => x.Key == room).Count() == 1;
        }

        #region vars
        /// <summary>
        /// our main tcp client.
        /// </summary>
        TcpClient Client;

        /// <summary>
        /// our network's input stream.
        /// </summary>
        TextReader Input;

        /// <summary>
        /// our network's output stream.
        /// </summary>
        TextWriter Output;

        /// <summary>
        /// our main tcp client.
        /// </summary>
        TcpClient GroupClient;

        /// <summary>
        /// our network's input stream.
        /// </summary>
        TextReader GroupInput;

        /// <summary>
        /// our network's output stream.
        /// </summary>
        TextWriter GroupOutput;

        /// <summary>
        /// our twitch account's username.
        /// </summary>
        string BotName;

        /// <summary>
        /// our twitch account's oauth password.
        /// </summary>
        string OAuth;

        /// <summary>
        /// our compiled list of users.
        /// </summary>
        Dictionary<string, Dictionary<string, List<string>>> UserLists;
        #endregion

        #region background methods
        /// <summary>
        /// hidden default constructor.
        /// </summary>
        TwitchController() { }

        /// <summary>
        /// destructor, closing down everything properly.
        /// </summary>
        ~TwitchController()
        {
            Disconnect();
        }

        /// <summary>
        /// reads and passes messages along to the invoker. also handles
        /// the parsing and replying of applicable twitch system messages,
        /// such as ping pongs, etc.
        /// </summary>
        private void ReadGroupMessagesAsync()
        {
            while (IsGroupConnected())
            {
                try
                {
                    if (!GroupClient.GetStream().DataAvailable) continue;
                    string inputstring = GroupInput.ReadLine();
                    if (GroupSystemMessageHandler(inputstring)) continue;
                    MessageReceivedHandler?.Invoke(MessageParser.GetRawMessage(inputstring), MessageParser.GetOriginRoom(inputstring), MessageParser.GetMessageSender(inputstring), MessageParser.GetUserType(inputstring));
                }
                catch (ObjectDisposedException) { };
            }
            Thread.Sleep(5);
        }

        /// <summary>
        /// reads and passes messages along to the invoker. also handles
        /// the parsing and replying of applicable twitch system messages,
        /// such as ping pongs, etc.
        /// </summary>
        private void ReadMessagesAsync()
        {
            while (IsConnected())
            {
                try
                {
                    if (!Client.GetStream().DataAvailable) continue;
                    string inputstring = Input.ReadLine();
                    if (SystemMessageHandler(inputstring)) continue;
                    MessageReceivedHandler?.Invoke(MessageParser.GetRawMessage(inputstring), MessageParser.GetOriginRoom(inputstring), MessageParser.GetMessageSender(inputstring), MessageParser.GetUserType(inputstring));
                }
                catch (ObjectDisposedException) { };
            }
            Thread.Sleep(5);
        }

        private bool GroupSystemMessageHandler(string message)
        {
            if (message.StartsWith("PING"))
            { // this is our normal ping/pong with twitch.
                GroupOutput.Write(message.Replace("PING", "PONG") + "\r\n");
                GroupOutput.Flush();
                return true;
            }

            if (message.StartsWith(":tmi.twitch.tv 001 " + BotName + " :Welcome, GLHF!"))
            { // this lets us know we have successfully established a connection to twitch.
                GroupOutput.Write("JOIN #" + "_remubot_1471411146556" + "\r\n");
                GroupOutput.Flush();

                GroupOutput.Write("CAP REQ :twitch.tv/membership \r\n");
                GroupOutput.Flush();

                GroupOutput.Write("CAP REQ :twitch.tv/commands \r\n");
                GroupOutput.Flush();
                return true;
            }

            if (!MessageParser.IsUserMessage(message))
            { // It's garbage system stuff
                return true;
            }
            return false;
        }

        /// <summary>
        /// handles the parsing and replying of all pertinent system messages.
        /// this also handles calling appropriate main form handlers when needed.
        /// </summary>
        /// <param name="message">message received from the controller's read function.</param>
        /// <returns>true if the message was a system message.</returns>
        private bool SystemMessageHandler(string message)
        {
            if (message.StartsWith("PING"))
            { // this is our normal ping/pong with twitch.
                Output.Write(message.Replace("PING", "PONG") + "\r\n");
                Output.Flush();
                return true;
            }

            if (message.StartsWith(":tmi.twitch.tv 001 " + BotName + " :Welcome, GLHF!"))
            { // this lets us know we have successfully established a connection to twitch.
                ConnectedHandler?.Invoke();
                return true;
            }

            if (message.StartsWith(":" + BotName + "!" + BotName + "@" + BotName + ".tmi.twitch.tv JOIN #"))
            { // this lets us know that we've successfully joined a room.
                string room = message.Split(new string[] { "JOIN #" }, StringSplitOptions.None).LastOrDefault();
                JoinedRoomHandler?.Invoke(room);
                Task task = new Task(() => GetUserListAsync(room));
                task.Start();
                return true;
            }

            if (message.StartsWith(":" + BotName + "!" + BotName + "@" + BotName + ".tmi.twitch.tv PART #"))
            { // this lets us know that we've successfully left a room.
                string room = message.Split(new string[] { "PART #" }, StringSplitOptions.None).LastOrDefault();
                LeftRoomHandler?.Invoke(room);
                ClearUserListbyRoom(room);
                return true;
            }

            if (!MessageParser.IsUserMessage(message))
            { // It's garbage system stuff
                return true;
            }
            return false;
        }

        /// <summary>
        /// clears the userlist of the specified room.
        /// </summary>
        /// <param name="room">room to clear.</param>
        private void ClearUserListbyRoom(string room)
        {
            UserLists.Remove(room);
        }

        /// <summary>
        /// calling this sets the userlist of a room to the supplied list.
        /// </summary>
        /// <param name="room">room to set.</param>
        /// <param name="list">list to apply.</param>
        private void SetUserList(string room, Dictionary<string, List<string>> list)
        {
            UserLists[room] = list;
            //rooms.ApplyRoomData(list, room);
        }

        /// <summary>
        /// threaded function that books up when a room is joined and validates current room users every _ seconds.
        /// </summary>
        /// <param name="room">room to check against.</param>
        private void GetUserListAsync(string room)
        {
            do
            {
                try
                {
                    using (WebClient web = new WebClient())
                    {
                        string json = web.DownloadString("http://tmi.twitch.tv/group/user/" + room + "/chatters");
                        Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();
                        object jsonobject = new JavaScriptSerializer().DeserializeObject(json);
                        Dictionary<string, object> objectpullroot = (Dictionary<string, object>)jsonobject;
                        if (objectpullroot.Count <= 0) throw new Exception("Failure converting json string.");
                        Dictionary<string, object> chatters = (Dictionary<string, object>)objectpullroot["chatters"];
                        if (chatters.Count <= 0) throw new Exception("Failure converting json string.");

                        foreach (KeyValuePair<string, object> group in chatters)
                        {
                            object[] userlist = (object[])group.Value;
                            List<string> users = userlist.Where(x => x != null).Select(x => x.ToString()).ToList();
                            list.Add(group.Key, users);
                        }
                        SetUserList(room, list);
                        GetUserListHandler?.Invoke(list, room);
                    }
                }
                catch
                {
                    Console.WriteLine("Could not obtain userlist for this room. Probably due to lack of network connectivity.");
                }
                for (int i = 0; i < 10; i++)
                { // sleeps for 1 minute. checks every 6 seconds for connectivity before killing itself early.
                    Thread.Sleep(TimeSpan.FromSeconds(6));
                    if (!IsConnected() || !IsInRoom(room)) return;
                }
            } while (IsConnected() && GetUserListHandler != null);
        }

        /// <summary>
        /// calling this gets the userlist of a specific room, if one exists.
        /// </summary>
        /// <param name="room">name of the room to pull users from.</param>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetUserList(string room)
        {
            if (UserLists.Where(x => x.Key == room).ToList().Count == 1)
                return UserLists[room];
            return null;
        }
        #endregion

        #region handlers
        /// <summary>
        /// called when a non-system message is received.
        /// </summary>
        /// <param name="message">the message received.</param>
        /// <param name="room">originating room.</param>
        /// <param name="user">originating user.</param>
        public delegate void messagereceivedhandler(string message, string room, string user, string messagetype);

        /// <summary>
        /// called when a non-system message is received.
        /// string message, string room, string user, string messagetype
        /// </summary>
        public messagereceivedhandler MessageReceivedHandler;

        /// <summary>
        /// called when connection to twitch has been established.
        /// </summary>
        public delegate void connectedhandler();

        /// <summary>
        /// called when connection to twitch has been established.
        /// </summary>
        public connectedhandler ConnectedHandler;

        /// <summary>
        /// called when a room has been joined.
        /// string room
        /// </summary>
        /// <param name="room"></param>
        public delegate void joinedroomhandler(string room);

        /// <summary>
        /// called when a room has been joined.
        /// </summary>
        public joinedroomhandler JoinedRoomHandler;

        /// <summary>
        /// called when a room has been left.
        /// string room
        /// </summary>
        /// <param name="room"></param>
        public delegate void leftroomhandler(string room);

        /// <summary>
        /// called when a room has been left.
        /// </summary>
        public leftroomhandler LeftRoomHandler;

        /// <summary>
        /// called when userlist is updated - once every 60s.
        /// Dictionary<string, List<string>> list, string room
        /// </summary>
        /// <param name="list"></param>
        /// <param name="room"></param>
        public delegate void getuserlisthandler(Dictionary<string, List<string>> list, string room);

        /// <summary>
        /// called when userlist is updated - once every 60s.
        /// </summary>
        public getuserlisthandler GetUserListHandler;

        /// <summary>
        /// called when disconnection from twitch has been triggered.
        /// </summary>
        public delegate void disconnectedhandler();

        /// <summary>
        /// called when disconnection from twitch has been triggered.
        /// </summary>
        public disconnectedhandler DisconnectedHandler;
        #endregion

        #region Rooms
        TwitchRooms rooms;


        #endregion
    }
}