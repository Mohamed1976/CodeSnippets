using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483.Exercises
{
    public class CSharpExercises
    {
        public const int cmToMeters = 100; //Compile time constant, must be defined when variable is declared during compile time
        public readonly double PI = 3.14;  //Runtime constant, can be declared runtime for example in constructor  

        public CSharpExercises()
        {
            //Runtime constant can be set in the constructor
            PI = 3.14;
            //Install NuGet package System.Configuration.ConfigurationManager 4.7.0.
            //TODO check how to read value from configuration file
            //double PI = Convert.ToDouble(ConfigurationSettings.AppSettings[0]);
            //double PI = Convert.ToDouble(ConfigurationManager.AppSettings.Get("PI"));
        }

        public async Task Run()
        {            
            //Sum range in main thread
            var stopwatch = Stopwatch.StartNew();
            addRangeOfValues(0, items.Length);
            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("The sharedTotal is: {0}, elapsed time {1}(ms)", sharedTotal, elapsed);

            //Sum range using multiple Tasks
            stopwatch = Stopwatch.StartNew();
            sharedTotal = 0;
            List<Task> tasks = new List<Task>();
            int rangeSize = 1000;
            int rangeStart = 0;
            while (rangeStart < items.Length)
            {
                int rangeEnd = rangeStart + rangeSize;
                if (rangeEnd > items.Length)
                    rangeEnd = items.Length;

                // create local copies of the parameters
                int rs = rangeStart;
                int re = rangeEnd;

                tasks.Add(Task.Run(() => addRangeOfValues(rs, re)));
                rangeStart = rangeEnd;
            }

            await Task.WhenAll(tasks);
            elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Multiple Tasks, the sharedTotal is: {0}, elapsed time {1}(ms)", sharedTotal, elapsed);

            //Sum range using Task Parallel library
            stopwatch = Stopwatch.StartNew();
            long sumTotal = 0;
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-parallel-for-loop-with-thread-local-variables
            Parallel.For<long>(0, items.Length, () => 0, (j, loop, subtotal) =>
            {
                subtotal += items[j];
                return subtotal;
            },
                (x) => Interlocked.Add(ref sumTotal, x)
            );
            elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("TPL For loop, the sumTotal is: {0}, elapsed time {1}(ms)", sumTotal, elapsed);
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-parallel-foreach-loop-with-partition-local-variables
            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/lambda-expressions-in-plinq-and-tpl
            sumTotal = 0;
            try
            {
                //Sum range using Task Parallel library
                stopwatch = Stopwatch.StartNew();

                Parallel.ForEach<int,long>(items, // source collection
                    () => 0,                            // thread local initializer
                    (n, loopState, localSum) =>     // body
                {
                    localSum += n;
                    return localSum;
                },
                    (localSum) => Interlocked.Add(ref sumTotal, localSum) // thread local aggregator
                );
                elapsed = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("TPL ForEach loop, the sumTotal is: {0}, elapsed time {1}(ms)", sumTotal, elapsed);
            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch (AggregateException ex)
            {
                Console.WriteLine("Parallel.ForEach has thrown an exception. THIS WAS NOT EXPECTED.\n{0}", ex);
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine($"Exception message: {exception.Message}");
                }
            }
            
            Console.WriteLine($"CalculateTotal using TPL.ForEach(): {CalculateTotal()}");
            /* Use the multi-core CPU, compute Pi using Task Parallel Library, compute sum using parallel processing. */
            stopwatch = Stopwatch.StartNew();
            var Pi = ComputePiUsingTPL();
            elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Calculating Pi using TPL: {string.Format("{0:0.000000}", Pi)}, elapsed time, {elapsed}ms");
            /* Call async method that returns Task<double>. */
            stopwatch = Stopwatch.StartNew();
            Pi = await AsyncComputePi();
            elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Calculating Pi using Task: {string.Format("{0:0.000000}", Pi)}, elapsed time, {elapsed}ms");
        }

        //These initializations of variables could also be done in the constructor.
        private object sharedTotalLock = new object();
        private long sharedTotal = 0;
        // make an array that holds the values 0 to 5000000
        private int[] items = Enumerable.Range(0, 500001).ToArray();

        private void addRangeOfValues(int start, int end)
        {
            long subTotal = 0;
            while (start < end)
            {
                subTotal = subTotal + items[start];
                start++;
            }

            lock (sharedTotalLock)
            {
                sharedTotal = sharedTotal + subTotal;
            }
        }

        /* public static System.Threading.Tasks.ParallelLoopResult For<TLocal> (long fromInclusive, long toExclusive, 
         * Func<TLocal> localInit, Func<long,System.Threading.Tasks.ParallelLoopState,TLocal,TLocal> body, 
         * Action<TLocal> localFinally);
         * https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-parallel-for-loop-with-thread-local-variables
         */
        private double ComputePiUsingTPL()
        {
            object lockObj = new object();
            //The number of the iterations
            const int N = 1000000000;            
            double step = 1.0 / N;
            double sum = 0.0;

            // Use type parameter to make subtotal a long, not an int
            Parallel.For<double>(0, N, () => 0.0, (i, loopState, threadSum) =>
            {
                var x = (i + 0.5) * step;
                return threadSum + 4.0 / (1.0 + x * x);
            }, (resultThreadSum) =>
            {
                lock (lockObj)
                {
                    sum += resultThreadSum;
                }
            });

            return sum * step;
        }

        private Task<double> AsyncComputePi()
        {
            return Task<double>.Run(() =>
            {
                return ComputePi();
            });
        }

        private double ComputePi()
        {
            //This is the number of the iterations
            const int N = 1000000000;

            //the sum
            var sum = 0.0;
            //the step size which is the inverse of N
            var step = 1.0 / N;

            for (var i = 0; i < N; i++)
            {
                var x = (i + 0.5) * step;
                sum = sum + 4.0 / (1.0 + x * x);
            }
            return sum * step;
        }

        //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-parallel-foreach-loop-with-partition-local-variables
        private long CalculateTotal()
        {
            int[] nums = Enumerable.Range(0, 1000000).ToArray();
            long total = 0;

            // First type parameter is the type of the source elements
            // Second type parameter is the type of the thread-local variable (partition subtotal)
            Parallel.ForEach<int, long>(nums, // source collection
                () => 0, // method to initialize the local variable
                (j, loop, subtotal) => // method invoked by the loop on each iteration
            {
                subtotal += j; //modify local variable
                return subtotal; // value to be passed to next iteration

                // Method to be executed when each partition has completed.
                // finalResult is the final value of subtotal for a particular partition.
            }, (finalResult) =>
            {
                Interlocked.Add(ref total, finalResult);
            });

            return total;
        }

    }
}
