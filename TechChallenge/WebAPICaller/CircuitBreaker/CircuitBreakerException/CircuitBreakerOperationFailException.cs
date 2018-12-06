using System;

namespace TechChallenge.WebAPICaller.CircuitBreaker.CircuitBreakerException
{
    public class CircuitBreakerOperationFailException : Exception
    {
        public CircuitBreakerOperationFailException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
