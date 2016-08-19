using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Project_Rem.Core
{
    class RemBot
    {
        private string BotName = "RemuBot";
        private readonly List<string> Nicknames = new List<string>() { "Rem", "Remu", "Remchi" };
        private RemBot() { }

        public RemBot(string botName)
        {
            BotName = botName;
        }

        public string GetBotName()
        {
            return BotName;
        }

        private string BelongsToBot(Message message)
        {
            List<string> broken = message.message.Split(' ').ToList();
            foreach (string nick in Nicknames)
            {
                if (broken.Where(sample => Regex.Replace(sample.ToLowerInvariant(), @"[^\w\s]", "") == (nick.ToLowerInvariant())).ToList().Count > 0)
                {
                    return nick.ToLowerInvariant();
                }
            }
            return null;
        }

        public List<Message> ParseMessage(Message message)
        {
            List<Message> toReturn = new List<Message>();
            if (BelongsToBot(message) != null)
            {
                if (message.message.Contains("!queue"))
                {
                    toReturn.Add(new Message("http://warp.world/q?s=vellhart", message.room, BotName, true));
                }
            }
            return toReturn;
        }
    }
}
