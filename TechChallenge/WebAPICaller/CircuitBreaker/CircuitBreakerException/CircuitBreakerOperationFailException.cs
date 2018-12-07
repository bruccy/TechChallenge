using System;

namespace TechChallenge.WebAPICaller.CircuitBreaker.CircuitBreakerException
{
    public class CircuitBreakerOperationFailException : Exception
    {
        public CircuitBreakerOperationFailException(string message) : base(message)
        {
        }
    }
}
