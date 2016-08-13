using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Rem
{
//    static class Parameters
//    {
//        /// <summary>
//        /// Thread-safe locker
//        /// </summary>
//        static object locker = new object();
//
//        /// <summary>
//        /// Name of our Bot
//        /// </summary>
//        public static string BotName
//        {
//            get
//            {
//                return BotName;
//            }
//            set
//            {
//                lock (locker)
//                {
//                    BotName = value;
//                }
//            }
//        }
//    }

    public class Message
    {
        public string message;
        public string room;
        public bool whisper;
        public string whisperTarget;
        public bool system;
        private Message() { }
        public Message(string mes, string roomname, bool whis, string tar, bool sys = false)
        { message = mes; room = roomname; whisper = whis; whisperTarget = tar; system = sys; }
    }

}
