using System;
using System.Threading;
using System.Threading.Tasks;
using TechChallenge.WebAPICaller.CircuitBreaker;
using TechChallenge.WebAPICaller.CircuitBreaker.CircuitBreakerException;
using TechChallenge.WebAPICaller.Retry.RetryException;

namespace TechChallenge.WebAPICaller.Retry
{
    public static class RetryHandler
    {
        public static async Task<TResult> Execute<TResult>(Func<TResult> func, int attempts, int waintingTime)
        {
            var circuitBreaker = new CircuitBreakerHandler();
            while(attempts > 0)
            {
                try
                {
                    return circuitBreaker.ExecuteFunction(func);
                }
                catch (CircuitBreakIsOpenException)
                {
                    Thread.Sleep(waintingTime);
                }

                attempts--;
            }

            throw new RetryLimitException("Retry limits reached");
        }
    }
}
