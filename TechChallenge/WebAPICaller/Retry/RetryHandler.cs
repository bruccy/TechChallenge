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
        public static async Task<TResult> Execute<TResult>(Func<Task<TResult>> func, int attempts, int waintingTimeInSeconds)
        {
            var circuitBreaker = new CircuitBreakerHandler();
            while(attempts > 0)
            {
                try
                {
                    return await circuitBreaker.ExecuteFunction(func);
                }
                catch (CircuitBreakerOperationFailException)
                {
                    Thread.Sleep(waintingTimeInSeconds * 1000);
                }
                catch (CircuitBreakIsOpenException)
                {
                    throw new RetryAttemptNotAllowedException("Retry attempt not allowed now.");
                }

                attempts--;
            }

            throw new RetryLimitException("Retry limits reached");
        }
    }
}
