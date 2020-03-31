using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    class PerformanceCounterTypeAverageBaseExample
    {
        public void Run()
        {
            const string category = "AverageCounter64SampleCategory";

            ConsoleTraceListener consoleTraceListener = new ConsoleTraceListener();
            Trace.Listeners.Add(consoleTraceListener);

            // If the category does not exist, create the category and exit.
            // Performance counters should not be created and immediately used.
            // There is a latency time to enable the counters, they should be created
            // prior to executing the application that uses the counters.
            // Execute this sample a second time to use the category.
            //If PerformanceCounterCategory does not exist add it
            if (!PerformanceCounterCategory.Exists(category))
            {
                Trace.TraceInformation("Creating new PerformanceCounterCategory: {0}", category);
                CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();

                // Add the counter.
                CounterCreationData averageCount64 = new CounterCreationData()
                {
                    CounterName = "AverageCounter64Sample",
                    CounterType = PerformanceCounterType.AverageCount64
                };

                // Add the base counter.
                CounterCreationData averageCount64Base = new CounterCreationData()
                {
                    CounterName = "AverageCounter64SampleBase",
                    CounterType = PerformanceCounterType.AverageBase
                };

                counterCreationDataCollection.Add(averageCount64);
                counterCreationDataCollection.Add(averageCount64Base);                
                PerformanceCounterCategory.Create(category,
                    "Demonstrates usage of the AverageCounter64 performance counter type.",
                    PerformanceCounterCategoryType.SingleInstance,
                    counterCreationDataCollection);

                Trace.TraceInformation("Successfully created PerformanceCounterCategory: {0}", category);
                Thread.Sleep(1000);
            }

            //Create Performance Counter
            if (PerformanceCounterCategory.Exists(category))
            {
                Trace.TraceInformation("Create PerformanceCounters.");
                PerformanceCounter avgCounter64Sample = new PerformanceCounter(categoryName: category,
                    counterName: "AverageCounter64Sample", readOnly: false);

                PerformanceCounter avgCounter64SampleBase = new PerformanceCounter(categoryName: category,
                    counterName: "AverageCounter64SampleBase", readOnly: false);

                Trace.TraceInformation("PerformanceCounters created.");

                avgCounter64Sample.RawValue = 0;
                avgCounter64SampleBase.RawValue = 0;

                //Collect Samples
                Random r = new Random(DateTime.Now.Millisecond);

                List<CounterSample> samplesList = new List<CounterSample>();

                // Loop for the samples.
                for (int j = 0; j < 100; j++)
                {
                    int value = r.Next(1, 10);
                    Console.Write(j + " = " + value);

                    avgCounter64Sample.IncrementBy(value);

                    avgCounter64SampleBase.Increment();

                    if ((j % 10) == 9)
                    {
                        CounterSample counterSample = avgCounter64Sample.NextSample();
                        OutputSample(counterSample); // avgCounter64Sample.NextSample());
                        samplesList.Add(counterSample); //avgCounter64Sample.NextSample());
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    System.Threading.Thread.Sleep(50);
                }

                CalculateResults(samplesList);
            }
        }

        // Output information about the counter sample.
        private static void OutputSample(CounterSample s)
        {
            Console.WriteLine("\r\n+++++++++++");
            Console.WriteLine("Sample values - \r\n");
            Console.WriteLine("   BaseValue        = " + s.BaseValue);
            Console.WriteLine("   CounterFrequency = " + s.CounterFrequency);
            Console.WriteLine("   CounterTimeStamp = " + s.CounterTimeStamp);
            Console.WriteLine("   CounterType      = " + s.CounterType);
            Console.WriteLine("   RawValue         = " + s.RawValue);
            Console.WriteLine("   SystemFrequency  = " + s.SystemFrequency);
            Console.WriteLine("   TimeStamp        = " + s.TimeStamp);
            Console.WriteLine("   TimeStamp100nSec = " + s.TimeStamp100nSec);
            Console.WriteLine("++++++++++++++++++++++");
        }

        private static void CalculateResults(List<CounterSample> samplesList)
        {
            for (int i = 0; i < (samplesList.Count - 1); i++)
            {
                // Output the sample.
                OutputSample((CounterSample)samplesList[i]);
                OutputSample((CounterSample)samplesList[i + 1]);

                // Use .NET to calculate the counter value.
                Console.WriteLine(".NET computed counter value = " +
                    CounterSampleCalculator.ComputeCounterValue((CounterSample)samplesList[i],
                    (CounterSample)samplesList[i + 1]));

                // Calculate the counter value manually.
                Console.WriteLine("My computed counter value = " +
                    MyComputeCounterValue((CounterSample)samplesList[i],
                    (CounterSample)samplesList[i + 1]));
            }
        }

        //++++++++//++++++++//++++++++//++++++++//++++++++//++++++++//++++++++//++++++++
        //    Description - This counter type shows how many items are processed, on average,
        //        during an operation. Counters of this type display a ratio of the items 
        //        processed (such as bytes sent) to the number of operations completed. The  
        //        ratio is calculated by comparing the number of items processed during the 
        //        last interval to the number of operations completed during the last interval. 
        // Generic type - Average
        //      Formula - (N1 - N0) / (D1 - D0), where the numerator (N) represents the number 
        //        of items processed during the last sample interval and the denominator (D) 
        //        represents the number of operations completed during the last two sample 
        //        intervals. 
        //    Average (Nx - N0) / (Dx - D0)  
        //    Example PhysicalDisk\ Avg. Disk Bytes/Transfer 
        //++++++++//++++++++//++++++++//++++++++//++++++++//++++++++//++++++++//++++++++
        private static Single MyComputeCounterValue(CounterSample s0, CounterSample s1)
        {
            Single numerator = (Single)s1.RawValue - (Single)s0.RawValue;
            Single denomenator = (Single)s1.BaseValue - (Single)s0.BaseValue;
            Single counterValue = numerator / denomenator;
            return (counterValue);
        }
    }
}
