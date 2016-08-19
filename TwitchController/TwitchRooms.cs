using System.Collections.Generic;

namespace Twitch.Controller
{
    class Room
    {
        public List<string> moderators { get; private set; }
        public List<string> viewers { get; private set; }

        public Room(Dictionary<string, List<string>> list)
        {
            moderators = list["moderators"];
            viewers = list["viewers"];
        }
    }

    public class TwitchRooms
    {
//        Dictionary<string, List<string>> usersbystatus = null;
        Dictionary<string, Room> rooms = null;

        public void ApplyRoomData(Dictionary<string, List<string>> list, string room)
        {
            Room parsedroom = new Room(list);

            rooms[room] = parsedroom;
        }
    }
}
