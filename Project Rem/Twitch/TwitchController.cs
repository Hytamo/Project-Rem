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

        public TwitchController(string username, string oauth)
        {
            Channels = new List<TwitchChannel>();
            UserName = username;
            Oauth = oauth;
        }

        public bool Connect()
        {
            bool toReturn = false;

            // Connect to Default Chat Group
            TwitchChannel privChannel = new TwitchChannel("", "", ChannelType.Default);
            if (privChannel.Connect(UserName, Oauth))
            {
                Channels.Add(privChannel);
            }

            // Set up Read and Send loops here
            ReadMessages();
   //         SendMessages();

            // Connect to Public Chat Group
            //           TwitchChannel pubChannel = new TwitchChannel("", "", ChannelType.Group);
            //           if (pubChannel.Connect(UserName, Oauth))
            //           {
            //               Channels.Add(pubChannel);
            //           }


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
                Thread thread = new Thread(() => ReadChannel(channel));
                thread.Start();
            }
        }

        private void ReadChannel(TwitchChannel channel)
        {
            Message returned = null;

            do
            {
                returned = channel.ReadMessages();
                if (returned != null)
                {
                    if (returned.system)
                    {
                        SendMessages(new List<Message>() { returned });
                    }
                    else
                    {
                        MessageReceivedHandler(returned);
                    }
                }
            } while (GetChannelByType(ChannelType.Default).IsConnected());

        }

        private void AddMessageToSend(Message message)
        {
            GetChannelByType(ChannelType.Default).SendPriorityMessage(message);
        }

        public void SendMessages(List<Message> messages)
        {
            if (messages == null) return;

            foreach(Message message in messages)
            {
                if (message.system)
                {
                    GetChannelByType(ChannelType.Default).SendPriorityMessage(message);
                }
                else
                {
                    AddMessageToSend(message);
                }
            }
        }
    }
}
