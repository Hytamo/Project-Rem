using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Rem.Twitch
{
    class TwitchRoom
    {
        string RoomName;
//        List<TwitchUser> Users;
//        List<string> ChatLog;

        // Room Stats

        private TwitchRoom() { }

        public TwitchRoom(string roomName)
        {
            RoomName = roomName;
        }

        public string GetRoomName()
        {
            return RoomName;
        }

    }
}
