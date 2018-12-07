using System;
using System.Threading;
using System.Threading.Tasks;
using TechChallenge.WebAPICaller.CircuitBreaker.CircuitBreakerException;

namespace TechChallenge.WebAPICaller.CircuitBreaker
{
    public class CircuitBreakerHandler
    {
        private Thread _closeCircuitThread;

        public int FailureCount { get; private set; }

        public int CallFailLimit { get; private set; }
        
        public int TimeNeedToWaintBeforeCallAgain { get; private set; }

        public CircuitBreakerState State { get; private set; }

        public CircuitBreakerHandler() : this(5, 6000)
        {           
        }

        public CircuitBreakerHandler(int callFailLimit, int timeNeedToWaintBeforeCallAgain)
        {
            if (callFailLimit <= 0)
            {
                throw new ArgumentException("CallFailLimit needs to be greater than 0");
            }

            _closeCircuitThread = new Thread(() =>
            {
                Thread.Sleep(TimeNeedToWaintBeforeCallAgain);
                State = CircuitBreakerState.CLOSED;
            });

            CallFailLimit = callFailLimit;
            TimeNeedToWaintBeforeCallAgain = timeNeedToWaintBeforeCallAgain;
            State = CircuitBreakerState.CLOSED;
        }

        public async Task<TResult> ExecuteFunction<TResult>(Func<Task<TResult>> func)
        {
            if (State == CircuitBreakerState.OPEN)
            {
                throw new CircuitBreakIsOpenException("Circuit breaker is currently open");
            }

            try
            {
                var returnedValue = await func();
                if (State == CircuitBreakerState.HALF_OPEN)
                {
                    State = CircuitBreakerState.CLOSED;
                    _closeCircuitThread.Abort();
                }

                if (FailureCount > 0)
                {
                    FailureCount--;
                }

                return returnedValue;
            }
            catch (Exception)
            {
                if (State == CircuitBreakerState.HALF_OPEN)
                {
                    State = CircuitBreakerState.OPEN;
                    _closeCircuitThread.Start();
                }
                else
                {
                    if (FailureCount <= CallFailLimit)
                    {
                        FailureCount++;
                    }
                    else if (FailureCount >= CallFailLimit)
                    {
                        State = CircuitBreakerState.OPEN;
                    }
                }

                throw new CircuitBreakerOperationFailException("Operation failed");
            }
        }
    }
}
