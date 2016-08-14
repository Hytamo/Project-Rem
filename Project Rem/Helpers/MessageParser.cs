using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Rem
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
        public static string ParseRawMessage(string message)
        {
            string rawMessage = null;
            string originroom = ParseOriginRoom(message) + " :";
            rawMessage = message.Split(new string[] { originroom }, StringSplitOptions.None).LastOrDefault();
            rawMessage = rawMessage.Split(new string[] { "\r\n" }, StringSplitOptions.None).FirstOrDefault();
            return rawMessage;
        }

        /// <summary>
        /// Pulls the message sender out of a base message
        /// </summary>
        /// <param name="message">Base message</param>
        /// <returns></returns>
        public static string ParseMessageSender(string message)
        {
            string sender = null;
            sender = message.Split("!".ToCharArray()).FirstOrDefault();
            sender = sender.TrimStart(':');
            return sender;
        }

        /// <summary>
        /// Pulls the origin room from a base message
        /// </summary>
        /// <param name="rooms">List of active rooms we're in</param>
        /// <param name="message">Base message</param>
        /// <returns></returns>
        public static string ParseOriginRoom(string message)
        {
            string room = null;
            room = message.Split("#".ToCharArray(), 2).LastOrDefault().Split(' ').FirstOrDefault();
            return room;
        }

        /// <summary>
        /// Pulls the message type from a base message
        /// </summary>
        /// <param name="message">Base  message</param>
        /// <param name="isWhisper">Is the message type a whisper?</param>
        /// <returns></returns>
        public static bool ParseMessageType(string message, out bool isWhisper)
        {
            isWhisper = false;

            if (message == null)
            {
                isWhisper = true;
            }
            else if (message.Contains("PRIVMSG"))
            {
                isWhisper = false;
                return true;
            }
            else if (message.Contains("WHISPERTYPE"))
            {
                isWhisper = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Pulls the message type from a base message, always returning the specific type found
        /// </summary>
        /// <param name="message">Base message</param>
        /// <returns></returns>
        public static string ParseMessageType(string message)
        {
            if (string.IsNullOrEmpty(message)) return null;

            if (message.Contains("PRIVMSG"))
            {
                return "PRIVMSG";
            }
            else if (message.Contains("WHISPERTYPE"))
            {
                return "WHISPERTYPE";
            }
            else return null;
        }

        /// <summary>
        /// Parses out whether or not the message was a whisper
        /// </summary>
        /// <param name="message">Base message</param>
        /// <returns>True if message was a whisperr, otherwise false</returns>
        public static bool ParseWhisperState(string message)
        {

            return false;
        }

    }
}
