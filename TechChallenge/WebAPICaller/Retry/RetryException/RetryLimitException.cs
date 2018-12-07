using System;

namespace TechChallenge.WebAPICaller.Retry.RetryException
{
    public class RetryLimitException: Exception
    {
        public RetryLimitException(string message) : base(message)
        {
        }
    }
}
