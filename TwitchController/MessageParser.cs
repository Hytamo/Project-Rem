using System;
using System.Linq;

namespace Twitch.Parser
{
    /// <summary>
    /// This is a helper class acting as a singleton.
    /// </summary>
    public static class MessageParser
    {
        /// <summary>
        /// Pulls the raw message out of a base message
        /// </summary>
        /// <param name="rooms">List of active rooms we're in</param>
        /// <param name="message">Base message</param>
        /// <returns></returns>
        public static string GetRawMessage(string message)
        {
            if (message == null) return null;
            string rawmessage = message.Split(new string[] { GetOriginRoom(message) + " :" }, StringSplitOptions.None).LastOrDefault();
            rawmessage = rawmessage.Split(new string[] { "\r\n" }, StringSplitOptions.None).FirstOrDefault();
            return rawmessage;
        }

        /// <summary>
        /// Pulls the message sender out of a base message
        /// </summary>
        /// <param name="message">Base message</param>
        /// <returns></returns>
        public static string GetMessageSender(string message)
        {
            if (message.StartsWith(":"))
                return message.Split("!".ToCharArray()).FirstOrDefault().TrimStart(':');
            return null;
        }

        /// <summary>
        /// Pulls the origin room from a base message
        /// </summary>
        /// <param name="rooms">List of active rooms we're in</param>
        /// <param name="message">Base message</param>
        /// <returns></returns>
        public static string GetOriginRoom(string message)
        {
            string usertype = null;
            if (IsUserMessage(message))
            {
                usertype = message.Split("#".ToCharArray(), 2).LastOrDefault().Split(' ').FirstOrDefault();
            }
            return (usertype == null || usertype.StartsWith(":")) ? null : usertype;
        }

        /// <summary>
        /// Pulls the message type from a base message
        /// </summary>
        /// <param name="message">Base  message</param>
        /// <param name="isWhisper">Is the message type a whisper?</param>
        /// <returns></returns>
        public static bool IsUserMessage(string message)
        {
            string sender = GetMessageSender(message);
            if (message.StartsWith(":" + sender + "!" + sender + "@" + sender + ".tmi.twitch.tv"))
            {
                string messagetype = GetUserType(message);
                if (messagetype == "PRIVMSG" || messagetype == "WHISPER") return true;
            }
            return false;
        }

        public static string GetUserType(string message)
        {
            string sender = GetMessageSender(message);
            if (message.StartsWith(":" + sender + "!" + sender + "@" + sender + ".tmi.twitch.tv"))
            {
                string messagetype = message.Split(new string[] { "tmi.twitch.tv " }, StringSplitOptions.None).LastOrDefault().Split(' ').FirstOrDefault();
                if (messagetype == "PRIVMSG" || messagetype == "WHISPER") return messagetype;
            }
            return null;
        }
    }
}
