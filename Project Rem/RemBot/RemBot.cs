using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Message> ParseMessage(Message message)
        {
            List<Message> toReturn = new List<Message>();

            toReturn.Add(new Message("Beep boop!1", message.room, BotName));

            return toReturn;
        }
    }
}
