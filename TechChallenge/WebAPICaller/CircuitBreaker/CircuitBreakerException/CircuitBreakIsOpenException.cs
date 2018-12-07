using System;

namespace TechChallenge.WebAPICaller.CircuitBreaker.CircuitBreakerException
{
    public class CircuitBreakIsOpenException : SystemException
    {
        public CircuitBreakIsOpenException(string message) : base(message)
        {
        }
    }
}
