using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Rem.Core
{
    class RemBot
    {
        public RemBot()
        {

        }

        public List<Message> ParseMessage(Message message)
        {
            List<Message> toReturn = new List<Message>();

            toReturn.Add(new Message("Beep boop!1", message.room, false, null));

            return toReturn;
        }
    }
}
