using _70_483.Multithreading;
using System;
using System.Threading.Tasks;
using _70_483.Exercises;
using _70_483.EventsAndCallbacks;
using _70_483.Exceptions;
using _70_483.ProgramFlow;
using _70_483.GarbageCollector;
using _70_483.String_Manipulation;
using _70_483.OOP;
using _70_483.Reflection;
using _70_483.Validation;
using _70_483.Encryption;
using _70_483.ManageAssemblies;
using _70_483.Debugging;
using _70_483.Diagnostics;
using _70_483.IOoperations;
using _70_483.Encodings;
using _70_483.ConsumeData;
using _70_483.DataCollections;
using _70_483.LinqQuery;
using _70_483.Serialization;

namespace _70_483
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [ General Exercises ]

            try
            {
                GeneralExercises generalExercises = new GeneralExercises();
                //generalExercises.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in GeneralExercises.Run(): {ex.ToString()}");
            }
            #endregion

            #region [ Exam Exercises ]

            try
            {
                _70_483_ExamExercises examExercises = new _70_483_ExamExercises();
                examExercises.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in _70_483_ExamExercises.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Serialization ]

            try
            {
                SerializationExamples serializationExamples = new SerializationExamples();
                //serializationExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in SerializationExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Linq Queries ]

            try
            {
                LinqQueryExamples linqQueryExamples = new LinqQueryExamples();
                //linqQueryExamples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in LinqQueryExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Data Collections ]

            try
            {
                DataCollectionsExamples dataCollectionsExamples = new DataCollectionsExamples();
                //dataCollectionsExamples.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in DataCollectionsExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Consume data ]

            try
            {
                ConsumeDataExamples consumeDataExamples = new ConsumeDataExamples();
                //consumeDataExamples.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in ConsumeDataExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Encoding ]

            try
            {
                EncodingExamples encodingExamples = new EncodingExamples();
                //encodingExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in EncodingExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ IO Operations ]

            try
            {
                IOoperationsExamples IOoperations = new IOoperationsExamples();
                ///IOoperations.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in IOoperationsExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Diagnostics ]

            try
            {
                DiagnosticsExamples diagnosticsExamples = new DiagnosticsExamples();
                //diagnosticsExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in DiagnosticsExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Debugging ] 

            try
            {
                DebuggingExamples debuggingExamples = new DebuggingExamples();
                //debuggingExamples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in DebuggingExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Manage Assemblies ]

            try
            {
                ManageAssembliesExamples manageAssembliesExamples = new ManageAssembliesExamples();
                ///manageAssembliesExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in ManageAssembliesExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Encryption ]

            try
            {
                EncryptionExamples encryptionExamples = new EncryptionExamples();
                //encryptionExamples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in EncryptionExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Validation ]

            try
            {
                ValidationExamples validationExamples = new ValidationExamples();
                //validationExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in ValidationExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Reflection ]

            try
            {
                ReflectionExamples reflectionExamples = new ReflectionExamples();
                //reflectionExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in ReflectionExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Class Hierarchy ]

            try
            {
                ClassHierarchyExamples classHierarchyExamples = new ClassHierarchyExamples();
                //classHierarchyExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in ClassHierarchyExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ String Manipulation ]

            try
            {
                String_Manipulation_Examples string_Manipulation_Examples = new String_Manipulation_Examples();
                //string_Manipulation_Examples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in String_Manipulation_Examples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Garbage Collector ]

            try
            {
                GarbageCollectorExamples garbageCollectorExamples = new GarbageCollectorExamples();
                //garbageCollectorExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in GarbageCollectorExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Multithreading Examples ]

            try
            {
                MultithreadingExamples multithreadingExamples = new MultithreadingExamples();
                //multithreadingExamples.Run().Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in MultithreadingExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Events and Callbacks ]

            try
            {
                DelegatesExamples delegatesExamples = new DelegatesExamples();
                //delegatesExamples.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in DelegatesExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region[ Programm Flow ]

            try
            {
                ProgramFlowExamples programFlowExamples = new ProgramFlowExamples();
                //programFlowExamples.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in ProgramFlowExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region[ Exceptions ]

            try
            {
                ExceptionExamples exceptionExamples = new ExceptionExamples();
                //exceptionExamples.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in DelegatesExamples.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Task Parallel Library (TPL) ] 

            //Parallelism involves taking a certain task and splitting it into a set of related tasks that can be executed concurrently. 
            //This also means that you shouldn’t go through your code to replace all your loops with parallel loops.
            //You should use the Parallel class only when your code doesn’t have to be executed sequentially.
            //Increasing performance with parallel processing happens only when you have a lot of work to be done that can be executed in parallel.
            //For smaller work sets or for work that has to synchronize access to resources, using the Parallel class can hurt performance.
            //The best way to know whether it will work in your situation is to measure the results.
            //Points to Remember while working with Parallel Programming:
            //1)The Tasks must be independent.
            //2)Order of the execution does not matter.
            //TaskParallellibrary taskParallellibrary = new TaskParallellibrary();
            //taskParallellibrary.Run();

            #endregion

            #region [ Parallel LINQ ]

            //Language-Integrated Query, or LINQ, is used to perform queries on items of data in C# programs. 
            //Parallel Language-Integrated Query (PLINQ) can be used to allow elements of a query to execute in parallel.
            ParallelLINQ parallelLINQ = new ParallelLINQ();
            //parallelLINQ.Run();

            #endregion

            #region [ Async Await ]

            AsyncAwait asyncAwait = new AsyncAwait();
            //asyncAwait.Run();

            #endregion

            #region [ ThreadsAndThreadPool]

            //Differences between threads and Tasks 
            //1) Threads are created as foreground processes (although they can be set to run in the background). 
            //The operating system will run a foreground process to completion, which means that an application will 
            //not terminate while it contains an active foreground thread. A foreground process that contains an
            //infinite loop will execute forever, or until it throws an uncaught exception or the operating system terminates it.
            //Tasks are created as background processes.This means that tasks can be terminated before they complete if
            //all the foreground threads in an application complete.

            //2) Threads have a priority property that can be changed during the lifetime of the thread. 
            //It is not possible to set the priority of a task.This gives a thread a higher priority request so a 
            //greater portion of available processor time is allocated.

            //3) A thread cannot deliver a result to another thread.Threads must communicate by using shared variables, 
            //which can introduce synchronization issues.

            //4) It is not possible to create a continuation on a thread. Instead, threads provide a method called a join, 
            //which allows one thread to pause until another completes.

            //5) It is not possible to aggregate exceptions over a number of threads. An exception thrown inside a thread must 
            //be caught and dealt with by the code in that thread. Tasks provide exception aggregation, but threads don’t.

            ThreadsAndThreadPool threadsAndThreadPool = new ThreadsAndThreadPool();
            ///threadsAndThreadPool.Run();

            #endregion

            #region [ Concurrent Collections ]

            //The standard .NET collections (including List, Queue and Dictionary) are not thread safe. The.NET
            //libraries provide thread safe(concurrent) collection classes that you can use when creating multi - tasking applications:
            //BlockingCollection<T>, ConcurrentQueue<T>, ConcurrentStack<T>, ConcurrentBag<T>, ConcurrentDictionary<TKey, TValue>

            ConcurrentCollections concurrentCollections = new ConcurrentCollections();
            //concurrentCollections.Run();

            #endregion

            #region [ Exercises ]

            try
            {
                CSharpExercises exercises = new CSharpExercises();
                //exercises.Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception in CSharpExercises.Run(): {ex.ToString()}");
            }

            #endregion

            #region [ Multithreading ]
            //Switching between threads is called context switching.
            //Windows has to make sure that the whole context of the thread is saved and restored on each switch.
            //Multithreading improves the responsiveness of the system and gives the illusion that one CPU can execute multiple tasks at a time.
            //This way you can create an application that uses parallelism, meaning that it can execute multiple threads on different CPUs in parallel.
            //CPU with multiple cores, Windows ensures that those threads are distributed over your available cores. 
            //One disadvantage of using multiple threads is the associated overhead (context switch) when switching between threads.   
            //The Thread class can be found in the System.Threading namespace. This class enables you to create new treads, manage their priority, and get their status.
            //Helper.MainThreadMethod();

            //Queuing a work item to a thread pool can be useful, but it has its shortcomings. There is no
            //built -in way to know when the operation has finished and what the return value is.
            //This is why the .NET Framework introduces the concept of a Task, which is an object 
            //that represents some work that should be done. The Task can tell you
            //The Task can tell you if the work is completed and if the operation returns a result, the Task gives you the result.
            //A task scheduler is responsible for starting the Task and managing it. 
            //By default, the Task scheduler uses threads from the thread pool to execute the Task.
            //Tasks can be used to make your application more responsive. If the thread that manages the user 
            //interface offloads work to another thread from the thread pool, it can keep processing user events 
            //and ensure that the application can still be used. But it doesn’t help with scalability. If a thread receives a web request and it 
            //would start a new Task, it would just consume another thread from the thread pool while the original thread waits for results 
            //Executing a Task on another thread makes sense only if you want to keep the user interface thread free for other work or if you 
            //want to parallelize your work on to multiple processors.
            //Method returns a Task, and is here awaited 
            //await TasksHelper.LoopMethod();
            //int returnVal = await TasksHelper.ReturnsTaskInt();
            //Console.WriteLine($"\nreturnVal: {returnVal}");
            //Alternatively, Attempting to read the Result property on a Task will force the thread that’s trying to read the result 
            //to wait until the Task is finished before continuing. As long as the Task has not finished, it is impossible to give 
            //the result. If the Task is not finished, this call will block the current thread.
            //Console.WriteLine($"\n{TasksHelper.ReturnsTaskInt().Result}");

            //returnVal = await TasksHelper.ReturnsTaskInt2();
            //Console.WriteLine($"returnVal: {returnVal}");

            #endregion

            Console.ReadLine();
        }
    }
}
