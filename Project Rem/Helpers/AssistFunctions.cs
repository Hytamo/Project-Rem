using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Rem
{
    public static class AssistFunctions
    {
        private delegate bool ToRetryFunc();

        public static bool DoRetryable(Func<bool> p, TimeSpan delayBetweenRetries, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}
