using System;

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
