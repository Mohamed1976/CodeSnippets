using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Net;

namespace _70_483.Multithreading
{
    //The Task.Parallel class in the library provides three methods that can be used to create applications that contain 
    //tasks that execute in parallel (Parallel.Invoke, Parallel.ForEach, Parallel.For).
    //The Task.Parallel class can be found in the System.Threading.Tasks namespace.
    public class TaskParallellibrary
    {
        private static Object lockObj = new Object();

        public TaskParallellibrary()
        {

        }

        public void Run()
        {
            //TPL uses background thread and the thread pool
            //The Parallel.Invoke method accepts a number of Action delegates and creates a Task for each of them.
            //An Action delegate is an encapsulation of a method that accepts no parameters and does not return a result. 
            //It can be replaced with a lamba expression, as shown below, in which two tasks are created.
            //The Parallel.Invoke method can start a large number of tasks at once. You have no control over the order in 
            //which the tasks are started or which processor they are assigned to. The Parallel.Invoke method returns when
            //all of the tasks have completed.
//            Parallel.Invoke(() => Task1(), () => Task2());
            //Alternatively you can use the syntax below
            //Action[] actions = new Action [] { Task1, Task2 };
            //Parallel.Invoke(actions);

            //The Task.Parallel class also provides a ForEeach method that performs a parallel implementation of the foreach 
            //loop construction, in which the WorkOnItem method is called to process each of the items in a list.
            //The Parallel.ForEach method accepts two parameters. The first parameter is an IEnumerable collection(in this case the 
            //list items).The second parameter provides the action to be performed on each item in the list. You can see some of the 
            //output from this program below. Note that the tasks are not completed in the same order that they were started.
            var items = Enumerable.Range(0, 500); // Generate a sequence of integers from 0 to 500 
            Parallel.ForEach<int>(items, (item) =>
            {
//                WorkOnItem(item);
            });

            //The Parallel.For method can be used to parallelize the execution of a for loop, which is governed by a control variable.
            //This implements a counter starting at 0 , the length of the items. The third parameter of the method
            //is a lambda expression, which is passed a variable that provides the counter value for each iteration.
            var itemsArr = Enumerable.Range(0, 500).ToArray(); //Return int []
            Parallel.For(0, itemsArr.Length, (int i) =>
            {
//                WorkOnItem(itemsArr[i]);
            });

            //The iterations can be ended by calling the Stop or Break methods on the ParallelLoopState variable. 
            //Break ensures that all iterations that are currently running will be finished. Stop just terminates everything.
            //Calling Stop will prevent any new iterations with an index value greater than the current index.If Stop is used to stop the
            //loop during the 200th iteration it might be that iterations with an index lower than 200 will not be performed. 
            //If Break is used to end the loop iteration, all the iterations with an index lower than 200 are guaranteed to be completed
            //before the loop is ended.

            ParallelLoopResult result = Parallel.For(0, itemsArr.Count(), (int i, ParallelLoopState loopState) =>
            {
                if (i == 200)
                    loopState.Break();
//                WorkOnItem(itemsArr[i]);
            });

            Console.WriteLine("Completed: " + result.IsCompleted);
            Console.WriteLine("Items: " + result.LowestBreakIteration);

            string[] urls = new string[] { "https://www.google.com/", "https://www.microsoft.com/", "https://edition.cnn.com/", "https://www.bbc.com/" };
            result = Parallel.For(0, urls.Count(), (int i, ParallelLoopState loopState) =>
            {
                DownloadUrl(urls[i]);
            });

            Console.WriteLine("Urls download completed: " + result.IsCompleted);
        }

        static void Task1()
        {
            Console.WriteLine("Task 1 starting");
            ShowThreadInformation("Task 1");
            Thread.Sleep(200);
            Console.WriteLine("Task 1 ending");
        }
        static void Task2()
        {
            Console.WriteLine("Task 2 starting");
            ShowThreadInformation("Task 2");
            Thread.Sleep(100);
            Console.WriteLine("Task 2 ending");
        }

        static void WorkOnItem(object item)
        {
            Console.WriteLine("Started working on: " + item);
            ShowThreadInformation("Item: " + item);
            Thread.Sleep(100);
            Console.WriteLine("Finished working on: " + item);
        }

        static void DownloadUrl(object url)
        {
            var stopwatch = Stopwatch.StartNew();
            WebClient webClient = new WebClient();
            string content = webClient.DownloadString(url as string);
            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"TPL url downloads: {url}, size: {content.Length}, elapsed time, {elapsed}ms");
        }

        private static void ShowThreadInformation(String taskName)
        {
            String msg = null;
            Thread thread = Thread.CurrentThread;
            lock (lockObj)
            {
                msg = String.Format("{0} thread information\n", taskName) +
                      String.Format("   Background: {0}\n", thread.IsBackground) +
                      String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                      String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            }
            Console.WriteLine(msg);
        }
    }
}
