using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Rem
{
    public class Message
    {
        public string message;
        public string room;
        public bool whisper;
        public string whisperTarget;
        public bool system;
        public string sender;
        private Message() { }
        public Message(string mes, string roomname, string originalsender, bool whis = false, string tar = null, bool sys = false)
        {
            message = mes;
            system = sys;
            whisper = whis;
            room = roomname == null ? null : roomname.Replace("#", "").ToLowerInvariant();
            whisperTarget = tar == null ? null : tar.ToLowerInvariant();
            sender = originalsender == null ? null : originalsender.ToLowerInvariant();
        }
    }

}
