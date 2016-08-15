using System.Collections.Generic;

namespace Project_Rem.Twitch
{
    class TwitchRoom
    {
        string RoomName;
        int MaxMessageStorageSize;
        List<Message> ChatLog;
        List<TwitchUser> Users;

        // Room Stats

        private TwitchRoom() { }

        public TwitchRoom(string roomName)
        {
            roomName = roomName.Replace("#", "").ToLowerInvariant();
            RoomName = roomName;
            ChatLog = new List<Message>();
            MaxMessageStorageSize = 2000;
            Users = new List<TwitchUser>();
        }

        public void LogMessage(Message message)
        {
            if (ChatLog.Count >= MaxMessageStorageSize)
            {
                ChatLog.RemoveAt(0);
            }

            ChatLog.Add(message);
        }

        public string GetRoomName()
        {
            return RoomName;
        }

        public List<Message> GetChatLogs()
        {
            return ChatLog;
        }
    }
}
