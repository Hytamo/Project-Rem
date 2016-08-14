using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Rem.Twitch
{
    partial class TwitchController
    {
        List<TwitchChannel> Channels;

        #region Handlers
        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void MessageHandler(Message message);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public MessageHandler MessageReceivedHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void JoinedRoom(string roomName);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public JoinedRoom JoinedRoomHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void LeftRoom(string roomName);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public LeftRoom LeftRoomHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void Disconnected();

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public Disconnected DisconnectedHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void Connected();

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public Connected ConnectedHandler;
        #endregion

        private TwitchController() { }
        string UserName;
        string Oauth;
        Queue<Message> PendingMessages;
        Object SendLocker;
        TimeSpan SenderCooldown;

        public TwitchController(string username, string oauth)
        {
            Channels = new List<TwitchChannel>();
            UserName = username;
            Oauth = oauth;
            PendingMessages = new Queue<Message>();
            SenderCooldown = TimeSpan.FromMilliseconds(300);
            SendLocker = new object();
        }

        public bool Connect()
        {
            bool toReturn = false;

            // Connect to Default Chat Group
            TwitchChannel privChannel = new TwitchChannel("irc.chat.twitch.tv", 6667, ChannelType.Default);
            if (privChannel.Connect(UserName, Oauth))
            {
                Channels.Add(privChannel);
            }

            // Set up Read and Send loops here
            ReadMessages();
            SendMessages();

            // Connect to Public Chat Group
//            TwitchChannel pubChannel = new TwitchChannel("", "", ChannelType.Group);
//            if (pubChannel.Connect(UserName, Oauth))
//            {
//                Channels.Add(pubChannel);
//            }
            return toReturn;
        }

        private TwitchChannel GetChannelByType(ChannelType type)
        {
            return Channels.Where(chan => chan.GetChannelType() == type).FirstOrDefault();
        }

        public bool JoinRoom(string roomName)
        {
            bool toReturn = false;
            Channels.FirstOrDefault().JoinRoom(roomName);

            JoinedRoomHandler(roomName);
            return toReturn;
        }

        public bool LeaveRoom(string roomName)
        {
            bool toReturn = false;
            Channels.FirstOrDefault().LeaveRoom(roomName);
            return toReturn;
        }

        private void ReadMessages()
        {
            foreach(TwitchChannel channel in Channels)
            {
                // Thread this
                Thread thread = new Thread(() => ChannelReader(channel));
                thread.Start();
            }
        }

        private void ChannelReader(TwitchChannel channel)
        {
            Message returned = null;

            do
            {
                returned = channel.ReadMessages();
                if (returned != null)
                {
                    if (returned.system)
                    {
                        AddMessagesToSend(new List<Message>() { returned });
                    }
                    else
                    {
                        MessageReceivedHandler(returned);
                    }
                }
            } while (GetChannelByType(ChannelType.Default).IsConnected());

        }

        public void AddMessagesToSend(List<Message> messages)
        {
            if (messages == null) return;

            foreach (Message message in messages)
            {
                if (message.system)
                {
                    GetChannelByType(ChannelType.Default).SendPriorityMessage(message);
                }
                else
                {
                    lock (SendLocker)
                    {
                        PendingMessages.Enqueue(message);
                    }
                }
            }
        }

        private void SendMessages()
        {
            Thread thread = new Thread(() => MessageSender());
            thread.Start();
        }

        private void MessageSender()
        {
            while (GetChannelByType(ChannelType.Default).IsConnected())
            {
                if (PendingMessages.Count > 0)
                {
                    Message toSend;
                    lock (SendLocker)
                    {
                        toSend = PendingMessages.Dequeue();
                    }
                    GetChannelByType(ChannelType.Default).SendPriorityMessage(toSend);
                }
                Thread.Sleep(SenderCooldown);
            }
        }
    }
}
