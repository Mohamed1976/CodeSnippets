using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483.Multithreading
{
    class AsyncAwait
    {

        public async void Run()
        {
            await ParentWithChilderen();
            Console.WriteLine("All the attached children are finished.");

            //Exception handling is done when awaiting a Task. 
            try
            {
                //Using a CancellationTokenSource you can specify a timeout after which the Task is canceled 
                CancellationTokenSource cts = new CancellationTokenSource(delay:TimeSpan.FromSeconds(2));
                await TaskContinueWithErrorHandling(cts.Token);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception caught in Try/Catch: {ex.Message}");
            }

            //Preference is to return a Task instead of return await Task, because there is extra context switch when awaiting. 
            await ConsecutiveOperation();
            //Do not use Result as in:  GetStringWithTaskRunAsync("https://www.bbc.com/", taskAction: () => Show()).Result; 
            //Makes the async call synchronous, instead use await as shown below.
            string content = await GetStringWithTaskRunAsync("https://www.bbc.com/", taskAction: () => Show());
            Console.WriteLine($"GetStringWithTaskRunAsync \"https://www.bbc.com/\" page size: {content.Length}.");
            //If you have multiple Tasks, use Task.WhenAll, instead of awaiting each task 
            Task<string>[] tasks = new Task<string>[5];
            tasks[0] = GetStringWithTaskRunAsync("https://www.bbc.com/", taskName:"Task 1", taskAction: () => Show());
            tasks[1] = GetStringWithTaskRunAsync("https://www.cnn.com/", taskName: "Task 2", taskAction: () => Show());
            tasks[2] = GetStringWithTaskRunAsync("https://www.microsoft.com/", taskName: "Task 3", taskAction: () => Show());
            tasks[3] = GetStringWithTaskRunAsync("https://www.google.com/", taskName: "Task 4", taskAction: () => Show());
            tasks[4] = GetStringWithTaskRunAsync("https://www.facebook.com/", taskName: "Task 5", taskAction: () => Show());
            //Big disadvantage Task.WaitAll(tasks) runs synchronously            
            //Task.WaitAll(tasks, timeout: TimeSpan.FromSeconds(3));, you can specify timeout and cancellationtoken
            //Returns when all task completed, canceled, faulted
            await Task.WhenAll(tasks);
            foreach(Task<string> task in tasks)
            {
                Console.WriteLine($"GetStringWithTaskRunAsync page size: {task.Result.Length}.");
            }
        }

        //Code running inside a parent Task can create other tasks, but these “child” tasks
        //will execute independently of the parent in which they were created.Such tasks
        //are called detached child tasks or detached nested tasks. A parent task can create
        //child tasks with a task creation option that specifies that the child task is attached
        //to the parent. The parent class will not complete until all of the attached child tasks have completed.
        //You can create a task without any attached child tasks by specifying the TaskCreationOptions.DenyChildAttach 
        //option when you create the task.Children of such a task will always be created as detached child tasks.Note
        //that tasks created using the Task.Run method have the TaskCreationOptions.DenyChildAttach option set, and therefore
        //can’t have attached child tasks.
        private Task ParentWithChilderen()
        {
            Task parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent starts.");
                for (int i = 0; i < 10; i++ )
                {
                    //This overload of the StartNew method accepts three parameters: the lambda expression giving the behavior 
                    //of the task, a state object that is passed into the task when it is started, and a TaskCreationOption value 
                    //that requests that the new task should be a child task.
                    Task.Factory.StartNew((x) => DoChild(x),
                        i,
                        TaskCreationOptions.AttachedToParent);
                }
                Console.WriteLine("Parent ends.");
            });
            return parent;
        }

        public static void DoChild(object state)
        {
            int i = (int)state;
            Console.WriteLine("Child {0} starting", i);
            Thread.Sleep((int)TimeSpan.FromSeconds(i).TotalMilliseconds);
            Console.WriteLine("Child {0} finished", i);
        }

        private Action exceptionAction = () =>
        {
            throw new Exception("TaskContinueWithErrorHandling error thrown.");
        };

        private Task TaskContinueWithErrorHandling(CancellationToken cancellationToken = default)
        {
            Task sleepTask = Task.Run(() =>
            {
                for(int i = 0; i < 50; i++)
                {
                    Thread.Sleep(100);// (int)TimeSpan.FromSeconds(1).TotalMilliseconds);
                    Console.Write("*");
                    //In order to cancel Task you can use the method and property below 
                    //cancellationToken.ThrowIfCancellationRequested();
                    //cancellationToken.IsCancellationRequested;
                }
                //exceptionAction.Invoke();
            }, cancellationToken);

            sleepTask.ContinueWith((completeGetContentTask) =>
            {
                Console.WriteLine($"Faulted task caught in ContinueWith method: {completeGetContentTask.Exception.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);

            sleepTask.ContinueWith((completeGetContentTask) =>
            {
                Console.WriteLine($"Task Ran To Completion.");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            sleepTask.ContinueWith((completeGetContentTask) =>
            {
                Console.WriteLine($"Task was Canceled.");
            }, TaskContinuationOptions.OnlyOnCanceled);

            return sleepTask;
        }

        private void Show()
        {
            Console.WriteLine("Show() method was called.");
        }

        private Task<string> GetStringWithTaskRunAsync(string url,
            [CallerMemberName] string taskName = default,             
            CancellationToken cancellationToken = default,
            Action taskAction = default)
        {
            Task<string> getUrlContentTask = Task<string>.Run(() =>
            {
                WebClient webClient = new WebClient();
                string content = webClient.DownloadString(url);
                taskAction?.Invoke();
                return content;
            }, cancellationToken: cancellationToken);

            getUrlContentTask.ContinueWith((completedGetUrlContentTask) =>
            {
                if (completedGetUrlContentTask.IsCompleted)
                {
                    Console.WriteLine($"Get web page completed succesfully, called from {taskName}.");
                }
                else if(completedGetUrlContentTask.IsCanceled)
                {
                    Console.WriteLine($"Get web page was canceled, called from {taskName}.");
                }
                else
                {
                    Console.WriteLine($"Get web page has faulted, called from {taskName}.");
                }
            });

            return getUrlContentTask;
        }

        public async Task ConsecutiveOperation()
        {
            ShowThreadInformation("ConsecutiveOperation start.");
            //Using ConfigureAwait(false) avoids returning to the context thread that called this thread (UI main thread), therefore additional work will
            //be done on background thread, UI remains responsive, note when you need to update UI elements from background thread
            //you need to use UI element dispatcher else you would get error
            string content = await GetStringWithTaskRunAsync("https://www.bbc.com/").ConfigureAwait(false);
            ShowThreadInformation("ConsecutiveOperation end.");

            //Simulate CPU intensive operation
            Thread.Sleep((int)TimeSpan.FromSeconds(5).TotalMilliseconds);
            ShowThreadInformation("ConsecutiveOperation after CPU intensive operation.");

            //The code below shows how to update UI elements from background thread  
            //https://docs.microsoft.com/en-us/uwp/api/windows.ui.core.coredispatcher.runasync
            //ResultTextBlock.Dispatcher.RunAsync(CoreDispatcherPriority.Normal
            //await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //  ResultTextBlock.Text = "Result: " + result.ToString();
            //});
        }

        //Thread execution context
        //A Thread instance exposes a range of context information, and some items can
        //be read and others read and set.The information available includes the name of
        //the thread(if any) priority of the thread, whether it is foreground or background,
        //the threads culture(this contains culture specific information in a value of type
        //CultureInfo) and the security context of the thread.The
        //Thread.CurentThread property can be used by a thread to discover this
        //information about itself.
        private static readonly object lockObj = new object();
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
