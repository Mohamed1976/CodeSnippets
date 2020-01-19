using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483.Multithreading
{
    class MultithreadingExamples
    {
        //If one task is delayed for some reason, perhaps because it is waiting for some data to
        //arrive from a mass storage device, then the processor running that task can move onto a different task.
        //This ability of a computer system to execute multiple processes at the same
        //time(concurrency) is not provided by the C# language itself. It is the underlying
        //operating system that controls which programs are active at any instant.The
        //.NET framework provides classes to represent items of work to be performed,
        //and in this section you learn how to use these classes.
        //It is not possible for a developer to make any assumptions concerning which
        //processes are active at any one time, how much processing time a given process
        //has, or when a given operation will be completed.

        public MultithreadingExamples()
        {                
        }

        public static void TaskDoWork()
        {
            Console.WriteLine($"TaskDoWork() starting, " +
                $"ThreadId[{Thread.CurrentThread.ManagedThreadId}], " +
                $"isBackground[{Thread.CurrentThread.IsBackground}]");
            Thread.Sleep(250);
            Console.WriteLine($"TaskDoWork() finished");
        }

        //Task.Run(someAction);
        //Is exactly equivalent to:
        //Task.Factory.StartNew(someAction, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);        
        //Note threadpool should not be used for long running operations (which are IDLE during long time) 
        public async Task Run()
        {
            Console.WriteLine($"Parent thread, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
            var _stopwatch = Stopwatch.StartNew();

            //You can create a task without any attached child tasks by specifying the
            //TaskCreationOptions.DenyChildAttach option when you create the
            //task.Children of such a task will always be created as detached child tasks. Note
            //that tasks created using the Task.Run method have the
            //TaskCreationOptions.DenyChildAttach option set, and therefore can’t have attached child tasks.
            Task task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Task.Factory.StartNew started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
                for (int i = 0; i < 10; i++)
                {
                    Task.Factory.StartNew((x) => /// lambda expression
                    {
                        Console.WriteLine($"CHILD{x} started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
                        Thread.Sleep(1000);
                        Console.WriteLine($"CHILD{x} Finished");
                    }, 
                    i, // state object 
                    TaskCreationOptions.AttachedToParent);
                }
            });
            await task2;
            var _elapsed = _stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Just returned to parent thread, elapsed[{_elapsed}ms] ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");

            //Task example
            //Task task = new Task(() => TaskDoWork(), TaskCreationOptions.DenyChildAttach);
            Task task = new Task(new Action(TaskDoWork), TaskCreationOptions.DenyChildAttach); //Same as above
            task.Start();
            await task;
            //The same can be written as
            //await Task.Run(() => TaskDoWork());

            var stopwatch = Stopwatch.StartNew();
            Task<int> task1 = Task<int>.Run(async() =>
            {
                Console.WriteLine($"Task<int>.Run started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
                await Task.Delay(1000);
                Console.WriteLine($"Task<int>.Run finished");
                return 99;
            });
            //Using task1.Result or task1.Wait() blocks the calling thread, better to use await 
            await task1;
            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Just returned to parent thread, elapsed[{elapsed}ms] ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
            
            //The Task Parallel Library(TPL) provides a range of resources that allow you
            //to use tasks in an application. The Task.Parallel class in the library
            //provides three methods that can be used to create applications that contain tasks
            //that execute in parallel.
            //1) Parallel.Invoke
            //2) Parallel.ForEach, Note that the tasks are not completed in the same order that they were started.
            //3) Parallel.For

            //The Task.Parallel class can be found in the System.Threading.Tasks namespace. The Parallel.Invoke method
            //accepts a number of Action delegates (Can be replaced with a lamba) and creates a Task for each of them.
            //Executes each of the provided actions, possibly in parallel.
            //No guarantees are made about the order in which the operations execute or whether they execute in parallel.
            //This method does not return until each of the provided operations has completed, regardless of whether completion 
            //occurs due to normal or exceptional termination.
            //The Parallel.Invoke method can start a large number of tasks at once.
            //You have no control over the order in which the tasks are started or which
            //processor they are assigned to. The Parallel.Invoke method returns when
            //all of the tasks have completed.
            try
            {
                Parallel.Invoke(Task1,
                    Task2,
                    //Use Lambda expression//Anonymous function instead of Action  
                    () =>
                    {
                        Console.WriteLine($"Task3 started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
                        Thread.Sleep(250);
                        Console.WriteLine("Task3 complete.");
                    },
                    Task4);
            }
            //The exception that is thrown when any action in the actions array throws an exception.
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch (AggregateException e)
            {
                Console.WriteLine("An action has thrown an exception. THIS WAS UNEXPECTED.\n{0}", e.InnerException.ToString());
                foreach(Exception ex in e.InnerExceptions)
                {
                    Console.WriteLine($"e.InnerExceptions, {ex.Message}");
                }
            }

            //For more advanced Parallel.For and Parallel.Foreach, see example in Exercises 
            //The Parallel.ForEach method accepts two parameters.The first
            //parameter is an IEnumerable collection(in this case the list items).The
            //second parameter provides the action to be performed on each item in the list.
            //You can see some of the output from this program below. Note that the tasks are
            //not completed in the same order that they were started.
            //Note the for loop creates about ten thread, this avoids overwhelming the system with new threads
            //Note when throwing error in WorkOnItem(item) method, not all items are iterated. 
            var items = Enumerable.Range(0, 500);
            Console.WriteLine($"items.Count: [{items.Count()}]");
            try
            {
                ParallelLoopResult result = Parallel.ForEach(items, (int item, ParallelLoopState loop) =>
                {
                    //If Break is used to end the loop iteration, all the iterations with an index 
                    //lower than 10 are guaranteed to be completed before the loop is ended.
                    if (item == 10)
                        loop.Break();

                    WorkOnItem(item);
                });

                Console.WriteLine("ForEach loop completed: " + result.IsCompleted);
                Console.WriteLine("ForEach loop Items: " + result.LowestBreakIteration);

            }
            catch (AggregateException ae)
            {
                Console.WriteLine("An action has thrown an exception. THIS WAS UNEXPECTED.\n{0}", ae.InnerException.ToString());
                foreach (Exception ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"e.InnerExceptions, {ex.Message}");
                }
            }

            CancellationTokenSource ctx = new CancellationTokenSource();
            CancellationToken ct = ctx.Token;
            Console.WriteLine($"Starting For loop iteration.");
            ParallelLoopResult _result = Parallel.For(0, items.Count(),
                //MaxDegreeOfParallelism sets the maximum number of concurrent tasks enabled by this ParallelOptions instance.
                //In this case only one thread is used.
                new ParallelOptions { MaxDegreeOfParallelism = 1, CancellationToken = ct },
                (int i, ParallelLoopState loop) =>
            {
                //Calling Stop will prevent any new iterationswith an index value greater than the current index.
                //If Stop is used to stop the loop during the 200th iteration it might be that iterations with an index lower
                //than 200 will not be performed. Note also that this doesn’t mean that work items with a
                //number greater than 200 will never run, because there is no guarantee that the
                //work item with number 200(which triggers the stop) will run before work items with higher numbers.
                if (i == 10)
                {
                    loop.Stop();
                }

                WorkOnItem(items.ElementAt(i));
            });

            Console.WriteLine("Completed: " + _result.IsCompleted);
            Console.WriteLine("Items: " + _result.LowestBreakIteration);

            Console.WriteLine($"Run finished");
        }

        static void WorkOnItem(object item)
        {
            if(item.GetType() != typeof(int))
            {
                throw new NotSupportedException("WorkOnItem(object item) only support int type.");
            }

            //if((int)item == 128 || (int)item == 300)
            //{
            //    throw new Exception("Something happend in here, Task["+item+"]");
            //}

            Console.WriteLine("Started working on: " + item + ", ThreadId["+ Thread.CurrentThread.ManagedThreadId + "], isBackground["+Thread.CurrentThread.IsBackground+"].");
            Thread.Sleep(100);
            Console.WriteLine("Finished working on: " + item);
        }

        private void Task1()
        {            
            Console.WriteLine($"Task1 started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
            Thread.Sleep(250);
            Console.WriteLine("Task1 complete.");
        }

        private void Task2()
        {
            Console.WriteLine($"Task2 started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
            Thread.Sleep(250);
            Console.WriteLine("Task2 complete.");
            //throw new Exception("Something happen in here, Task2()."); 
        }

        private void Task4()
        {
            Console.WriteLine($"Task4 started, ThreadId[{Thread.CurrentThread.ManagedThreadId}], isBackground[{Thread.CurrentThread.IsBackground}].");
            Thread.Sleep(250);
            Console.WriteLine("Task4 complete.");
            //throw new Exception("Something happen in here, Task4().");
        }
    }
}
