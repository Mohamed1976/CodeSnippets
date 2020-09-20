using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataLibrary.BlogDemo.Helpers
{
    public class CustomExecutionStrategy : ExecutionStrategy
    {
        public CustomExecutionStrategy(ExecutionStrategyDependencies strategy) :
            base(strategy, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        {
        }

        public CustomExecutionStrategy(ExecutionStrategyDependencies strategy, int maxRetryCount, TimeSpan maxRetryDelay)
            : base(strategy, maxRetryCount, maxRetryDelay)
        {
        }

        protected override bool ShouldRetryOn(Exception exception)
        {
            Console.WriteLine($"CustomExecutionStrategy Exception: " +
                $"{exception.Message}, {exception?.InnerException?.Message}");

            //Add in the specific exceptions that warrant a retry
            //In this case we return for all errors true, which means retry on any error
            //This is done because transient errors are difficult to generate on demand 
            return true;
        }
    }
}
