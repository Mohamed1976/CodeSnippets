using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.performancecountertype?redirectedfrom=MSDN&view=netframework-4.8

//When your application is running on a production server, it’s sometimes impossible to attach
//a debugger because of security restrictions or the nature of the application.If the application
//runs on multiple servers in a distributed environment, such as Windows Azure, a regular
//debugger won’t always help you find the error.
//Because of this, it’s important that you implement a logging and tracing strategy right
//from the start. Tracing is a way for you to monitor the execution of your application while it’s
//232 CHAPTER 3 Debug applications and implement security
//running.Tracing information can be detailed; it can show which methods are entered, decisions 
//are made, and errors or warnings happen while the application is running.
//Tracing can generate a huge amount of information and it’s something that 
//you enable when you need to investigate an issue in a production application.
//Logging is always enabled and is used for error reporting.You can configure 
//your logging to collect the data in some centralized way. Maybe you want an e-mail 
//or text message when there is a serious issue.Other errors can be logged to a file or a database.
//The.NET Framework offers classes that can help you with logging and tracing in the 
//System.Diagnostics namespace.One such class is the Debug class, which can, as its 
//name suggests, be used only in a debug build.This is because the ConditionalAttribute 
//with a value of DEBUG is applied to the Debug class. You can use it for basic logging 
//and executing assertions on your code.Listing 3-45 shows an example of using the Debug class.

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    class PerformancecounterExercises
    {
        private const string categoryName = "Demo Processing";

        private PerformanceCounter NrOfImagesProcessedCounter = null;
        private PerformanceCounter ImagesProcessedPerSecondCounter = null;
        private object _lock = new object();

        private void UpdatePerformanceCounters()
        {
            for(;;)
            {
                lock(_lock)
                {
                    NrOfImagesProcessedCounter?.Increment();
                    ImagesProcessedPerSecondCounter?.Increment();
                }
                Thread.Sleep(500);
            }
        }

        private void PrintPerformanceCounters()
        {
            for(;;)
            {
                lock(_lock)
                {
                    Console.WriteLine("Nr Of Images Processed: {0}, Speed: {1}", 
                        NrOfImagesProcessedCounter.NextValue(),
                        ImagesProcessedPerSecondCounter.NextValue());

                }
                Thread.Sleep(500);
            }
        }

        public void WriteEntry(string message, EventLogEntryType eventLogEntryType)
        {
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource("MySource", "Demo Processing log");
            }

            EventLog eventLog = new EventLog();
            eventLog.Source = "MySource";
            eventLog.WriteEntry(message, eventLogEntryType);
        }

        TraceSource ts = default;
        public void DoWork()
        {
            Guid originalID = default;
            Guid guid = default;

            try
            {
                originalID = Trace.CorrelationManager.ActivityId;
                guid = Guid.NewGuid();
                ts.TraceTransfer(1, "Changing activity", guid);
                Trace.CorrelationManager.ActivityId = guid;
                ts.TraceEvent(TraceEventType.Start, 0, "Start");    
            }
            finally
            {
                ts.TraceTransfer(1, "Changing activity", originalID);
                Trace.CorrelationManager.ActivityId = originalID;
                ts.TraceEvent(TraceEventType.Stop, 0, "Stop");
            }
        }

        public void Run()
        {
            //-----------------------------------------------------------------------------------
            // Custom TraceSource object named ts and a method named DoWork()
            // Collect Trace information when DoWork() method executes 
            // Group all traces for as single execution of DoWork method as an activity that
            // can be viewed in the WCF Service Trace Viewer Tool  
            //-----------------------------------------------------------------------------------
            ts = new TraceSource("Contoso", SourceLevels.ActivityTracing);
            ConsoleTraceListener consoleTraceListener1 = new ConsoleTraceListener();
            ts.Listeners.Add(consoleTraceListener1);
            DoWork();

            //-----------------------------------------------------------------------------------
            // You are developing a method named CreateCounters that will create performance counters for an
            // application as shown below.
            // You need to ensure that Counter1 is available for use in Windows Performance Monitor (PerfMon).
            //
            // PerformanceCounterType.SampleBase - A base counter that stores the number of sampling interrupts
            // taken and is used as a denominator in the sampling fraction.The sampling fraction is the number of
            // samples that were 1(or true) for a sample interrupt.Check that this value is greater than zero before using
            // it as the denominator in a calculation of SampleFraction.
            // PerformanceCounterType.SampleFraction - A percentage counter that shows the average ratio of hits to all
            // operations during the last two sample intervals. Formula: ((N 1 - N 0) / (D 1 - D 0)) x 100, where the
            // numerator represents the number of successful operations during the last sample interval, and the
            // denominator represents the change in the number of all operations(of the type measured) completed
            // during the sample interval, using counters of type SampleBase. Counters of this type include Cache\Pin
            // Read Hits %.
            //
            // References: http://msdn.microsoft.com/en-us/library/system.diagnostics.performancecountertype.aspx
            //-----------------------------------------------------------------------------------
            const string categoryName = "Contos";

            PerformanceCounterCategory.Delete(categoryName);
            Thread.Sleep(5000);

            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                var CounterDC = new CounterCreationDataCollection();
                var IOTDataRate = new CounterCreationData();
                IOTDataRate.CounterName = "Data Trans/Sec";
                IOTDataRate.CounterHelp = "Data transactions per second.";
                //RateOfCountsPerSecond64
                //A difference counter that shows the average number of operations completed during 
                //each second of the sample interval. Counters of this type measure time in ticks of 
                //the system clock.This counter type is the same as the RateOfCountsPerSecond32 type, 
                //but it uses larger fields to accommodate larger values to track a high-volume number 
                //of items or operations per second, such as a byte-transmission rate.Counters of this 
                //type include System\ File Read Bytes / sec.
                IOTDataRate.CounterType = PerformanceCounterType.RateOfCountsPerSecond64;

                CounterDC.Add(IOTDataRate);
                PerformanceCounterCategory.Create(categoryName,
                    "Contos category for IOT data.",
                    PerformanceCounterCategoryType.SingleInstance,
                    CounterDC);

                Thread.Sleep(1000);
            }

            if(PerformanceCounterCategory.Exists(categoryName))
            {
                PerformanceCounter performanceCounter1 =
                    new PerformanceCounter(categoryName, "Data Trans/Sec", readOnly:false);

                long counter = 0;
                for (;;)
                {
                    performanceCounter1.Increment();
                    counter++;
                    Thread.Sleep(50);
                    if(counter % 20 == 0 )
                        Console.WriteLine("Data Trans/Sec: {0}",performanceCounter1.NextValue());
                    if (Console.KeyAvailable)
                        break;
                }
            }

            //-----------------------------------------------------------------------------------
            // You are developing an application by using C#. The application will write events to an event log. You plan to
            // deploy the application to a server. You create an event source named MySource and a custom log named MyLog on the server.
            // You need to write events to the custom log.
            //-----------------------------------------------------------------------------------
            WriteEntry("Write message to EventLog.", EventLogEntryType.Warning);

            //-----------------------------------------------------------------------------------
            // You need to ensure that if an exception occurs, the exception will be logged.
            //-----------------------------------------------------------------------------------
            try
            {
                throw new Exception("An unexpected error, example.");
            }
            catch (Exception ex)
            {
                using (XmlWriterTraceListener xmlWriterTraceListener = new XmlWriterTraceListener("Exception.log"))
                {
                    xmlWriterTraceListener.TraceEvent(new TraceEventCache(), ex.Message, TraceEventType.Error, ex.HResult);
                }

                EventLog eventLog4 = new EventLog();
                eventLog4.Source = "Application";
                eventLog4.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

            try
            {
                //Instead of using config file we can also manually add TraceListeners
                //Trace.Listeners.Clear();
                //EventLogTraceListener myTraceListener = new EventLogTraceListener("myEventLogSource");
                //Trace.Listeners.Add(myTraceListener);

                Trace.TraceInformation("Trace Information message.");
                Trace.WriteLine("Second message.");
                EventLog eventLog4 = new EventLog();
                eventLog4.Source = "Application";
                eventLog4.WriteEntry("Message to EventLog.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Trace exception: {0}", ex.Message);
                throw;
            }
            
            EventLog eventLog3 = new EventLog("Application", ".", "testEventLogEvent");
            eventLog3.EntryWritten += (sender, arg) =>
            {
                Console.WriteLine(arg.Entry.Message);
            };
            eventLog3.EnableRaisingEvents = true;
            eventLog3.WriteEntry("Message to Corona.", EventLogEntryType.Information);
            
            //You can also read programmatically from the event log.You do this by getting an EventLogEntry 
            //from the Entries property of the EventLog.
            EventLog eventLog2 = new EventLog("Demo Processing log");
            Console.WriteLine("Total entries: " + eventLog2.Entries.Count);
            foreach (EventLogEntry entry in eventLog2.Entries)
            {
                Console.WriteLine("Index: {0}, Source: {1}, Type: {2}, Time: {3}, Message: {4}",
                    entry.Index,
                    entry.Source,
                    entry.EntryType,
                    entry.TimeWritten,
                    entry.Message);
            }
            
            //Next to writing trace information to a file or database, you can also write events to the 
            //Windows Event Log. You do this by using the EventLog class in the System.Diagnostics namespace. 
            //To use the EventLog class, you need to run with an account that has the appropriate permissions 
            //to create event logs. When running it from Visual Studio, you have to run Visual Studio as an administrator.
            if (!EventLog.SourceExists("MySource"))
            {
                Console.WriteLine("CreateEventSource (MySource, Demo Processing log)");
                EventLog.CreateEventSource("MySource", "Demo Processing log");
            }

            EventLog eventLog1 = new EventLog();
            eventLog1.Source = "Application";
            eventLog1.WriteEntry("Message to application log.", EventLogEntryType.Warning);            
            eventLog1.WriteEntry("2nd message to application log.", EventLogEntryType.Warning);

            EventLog eventLog = new EventLog();
            eventLog.Source = "MySource";
            eventLog.EnableRaisingEvents = true;
            eventLog.EntryWritten += (sender, e) =>
            {
                Console.WriteLine("EventLog: {0}", e.Entry.Message);
            };

            eventLog.WriteEntry("My messag to log 19-03-2020.", EventLogEntryType.Warning, 103, 5);

            foreach (EventLog _eventLog in EventLog.GetEventLogs())
            {
                Console.WriteLine("Log: {0}, Source: {1}, LogDisplayName: {2}, MachineName: {3}", 
                    _eventLog.Log, 
                    _eventLog.Source,
                    _eventLog.LogDisplayName,
                    _eventLog.MachineName);
            }
            
            //-----------------------------------------------------------------------------------
            // TraceSource example
            //-----------------------------------------------------------------------------------
            TraceSource traceSource = new TraceSource("MyTraceSource", SourceLevels.All);
            ConsoleTraceListener consoleTraceListener = new ConsoleTraceListener();
            //traceSource.Listeners.Clear(); //If you want to remove the DefaultTraceListener
            traceSource.Listeners.Add(consoleTraceListener);
            FileStream fileStream = new FileStream("myLogger.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //TextWriterTraceListener textWriterTraceListener = new TextWriterTraceListener(fileStream);
            //traceSource.Listeners.Add(textWriterTraceListener);
            SourceSwitch sourceSwitch = new SourceSwitch("MySourceSwitch");
            sourceSwitch.Level = SourceLevels.Error;
            traceSource.Switch = sourceSwitch;

            //The second argument to the trace methods is the event ID number. 
            //This number does not have any predefined meaning; it’s just another 
            //way to group your events together. You could, for example, group your 
            //database calls as numbers 10000–10999 and your web service calls as 
            //11000–11999 to more easily tell what area of your application a trace entry is related.

            //As you can see, you can pass a parameter of type TraceEventType to the trace methods.
            //You use this to specify the severity of the event that is happening. This information 
            //is later used by the TraceSource to determine which information should be output.

            //Writing all information to the Output window can be useful during debug sessions, 
            //but not in a production environment.To change this behavior, both the Debug and 
            //TraceSource classes have a Listeners property. This property holds a collection of 
            //TraceListeners, which process the information from the Write, Fail, and Trace methods.
            //Out of the box, both the Debug and the TraceSource class use an instance of the 
            //DefaultTraceListener class. The DefaultTraceListener writes to the Output window 
            //and shows the message box when assertion fails.

            //If you don’t want the DefaultTraceListener to be active, you need to clear the current 
            //listeners collection. You can add as many listeners as you want. 
            //The DefaultTraceListener is removed, and a TextWriteTraceListener is configured. 
            //After running this code, an output file is created named Tracefile.txt that contains the output of the trace.

            traceSource.TraceEvent(TraceEventType.Warning, 100, "Warning message");
            traceSource.TraceData(TraceEventType.Error, 2001, "Error message");

            //-----------------------------------------------------------------------------------
            // Create a Performancecounter
            // Performance counters can be used to constantly monitor the health of your applications.
            //
            //All performance counters are part of a category, and within that category they have a
            //unique name. To access the performance counters, your application has to run under full
            //trust, or the account that it’s running under should be an administrator or be a part of the
            //Performance Monitor Users group.
            //All performance counters implement IDisposable because they access unmanaged resources.
            //After you’re done with the performance counter, it’s best to immediately dispose of it.
            //-----------------------------------------------------------------------------------
            PerformanceCounter performanceCounter = new PerformanceCounter(
                categoryName: "Memory",
                counterName: "Available Bytes");

            for (; ; )
            {
                Console.WriteLine("Memory, Available Bytes: {0:0.00}", performanceCounter.NextValue());
                Thread.Sleep(400);
            }

            //-----------------------------------------------------------------------------------
            // Create a Performancecounter
            //-----------------------------------------------------------------------------------
            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                CounterCreationDataCollection counterCollection = new CounterCreationDataCollection()
                {
                    //Performance counters come in several different types. The type definition determines how
                    //the counter interacts with the monitoring applications. Some types that can be useful are the following:
                    //=> NumberOfItems32/NumberOfItems64 These types can be used for counting the
                    //number of operations or items.NumberOfItems64 is the same as NumberOfItems32,
                    //except that it uses a larger field to accommodate for larger values.

                    //=> RateOfCountsPerSecond32 / RateOfCountsPerSecond64 These types can be used to
                    //calculate the amount per second of an item or operation.RateOfCountsPerSecond64
                    //is the same as RateOfCountsPerSecond32, except that it uses larger fields to accommodate
                    //for larger values.

                    //=> AvergateTimer32 Calculates the average time to perform a process or process an item.
                    new CounterCreationData(counterName:"# of images processed",
                                counterHelp:"number of images resized",
                                counterType:PerformanceCounterType.NumberOfItems64),

                    new CounterCreationData(counterName: "# images processed per second",
                                counterHelp:"number of images processed per second",
                                counterType:PerformanceCounterType.RateOfCountsPerSecond32)

                };

                PerformanceCounterCategory.Create(categoryName: categoryName,
                    categoryHelp: "Image processing information",
                    categoryType: PerformanceCounterCategoryType.SingleInstance,
                    counterData: counterCollection);
                Thread.Sleep(500);
            }

            if(PerformanceCounterCategory.Exists(categoryName))
            {
                NrOfImagesProcessedCounter = new PerformanceCounter(categoryName: categoryName,
                    counterName: "# of images processed", readOnly: false);

                ImagesProcessedPerSecondCounter = new PerformanceCounter(categoryName: categoryName,
                    counterName: "# images processed per second", readOnly: false);

                //Create two threads one that reads and one that writes
                Thread thread = new Thread(new ThreadStart(UpdatePerformanceCounters));
                Thread thread1 = new Thread(new ThreadStart(PrintPerformanceCounters));
                thread.Start();
                thread1.Start();
                thread.Join();
                thread1.Join();
            }

            //-----------------------------------------------------------------------------------
            // Add existing system Performancecounter
            //-----------------------------------------------------------------------------------
            PerformanceCounter processor = new PerformanceCounter(
                categoryName: "Processor Information",
                counterName: "% Processor Time",
                instanceName: "_Total");

            for(;;)
            {
                Console.WriteLine("% Processor Time: {0}", processor.NextValue());
                Thread.Sleep(500);
                if(Console.KeyAvailable)
                {
                    break;
                }
            }
        }
    }
}
