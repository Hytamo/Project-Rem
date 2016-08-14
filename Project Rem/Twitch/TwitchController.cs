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
            privChannel.ChatLogHandler += RecordToChatLog;
            if (privChannel.Connect(UserName, Oauth))
            {
                Channels.Add(privChannel);
            }

            // Set up Read and Send loops here
            ReadMessages();
            SendMessages();

            return toReturn;
        }

        public void RecordToChatLog(Message message)
        {
            ChatLogHandler(message);
        }

        public TwitchChannel GetChannelByType(ChannelType type)
        {
            return Channels.Where(chan => chan.GetChannelType() == type).FirstOrDefault();
        }

        public bool JoinRoom(string roomName)
        {
            if (Channels.FirstOrDefault().JoinRoom(roomName))
            {
                JoinedRoomHandler(roomName);
                return true;
            }
            return false;
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
                    ChatLogHandler(returned);
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

        public bool IsConnected()
        {
            return GetChannelByType(ChannelType.Default).IsConnected();
        }

        public void AddMessagesToSend(List<Message> messages)
        {
            if (messages == null) return;

            foreach (Message message in messages)
            {
                if (message.system)
                {
                    GetChannelByType(ChannelType.Default).SendPriorityMessage(message);
                    ChatLogHandler(message);
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
            while (GetChannelByType(ChannelType.Default) == null || GetChannelByType(ChannelType.Default).IsConnected())
            {
                if (PendingMessages.Count > 0)
                {
                    Message toSend;
                    lock (SendLocker)
                    {
                        toSend = PendingMessages.Dequeue();
                    }
                    GetChannelByType(ChannelType.Default).SendPriorityMessage(toSend);
                    ChatLogHandler(toSend);
                }
                Thread.Sleep(SenderCooldown);
            }
        }


    }
}
