using System;

namespace TechChallenge.WebAPICaller.Retry.RetryException
{
    public class RetryAttemptNotAllowedException : Exception
    {
        public RetryAttemptNotAllowedException(string message) : base(message)
        {
        }
    }
}
