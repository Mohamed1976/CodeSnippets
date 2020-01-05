using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _70_483.Multithreading
{
    class ThreadsAndThreadPool
    {
        public void Run()
        {
            //Thread pools
            //Note that there are some situations when using the ThreadPool is not a good idea:
            //1) If you create a large number of threads that may be idle for a very long time,
            //this may block the ThreadPool, because the ThreadPool only contains a
            //finite number of threads.
            //2) You cannot manage the priority of threads in the ThreadPool.
            //3) Threads in the ThreadPool have background priority. You cannot obtain
            //a thread with foreground priority from the ThreadPool.
            //4) Local state variables are not cleared when a ThreadPool thread is reused.
            //They therefore should not be used.

            //Threads, like everything else in C#, are managed as objects. If an application
            //creates a large number of threads, each of these will require an object to be
            //created and then destroyed when the thread completes.A thread pool stores a
            //collection of reusable thread objects. Rather than creating a new Thread
            //instance, an application can instead request that a process execute on a thread
            //from the thread pool.When the thread completes, the thread is returned to the
            //pool for use by another process. The ThreadPool provides a method QueueUserWorkItem, 
            //which allocates a thread to run the supplied item of work. The item of work is supplied 
            //as a WaitCallback delegate.There are two versions of this delegate 
            //(one that accepts a state object, and one without a state).
            //A state object provide state information to the thread to be started.
            WaitCallback waitCallback = (object state) =>
            {
                Console.WriteLine("Doing work: {0}", (int)state );
                Thread.Sleep(500);
                Console.WriteLine("Work finished: {0}", (int)state);
            };

            //The ThreadPool restricts the number of
            //active threads and maintains a queue of threads waiting to execute.A program
            //that creates a large number of individual threads can easily overwhelm a device.
            //However, this does not happen if a ThreadPool is used.The extra threads are
            //placed in the queue.
            for (int i = 0; i < 50; i++)
            {
                int stateNumber = i;
                ThreadPool.QueueUserWorkItem(waitCallback, i);
            }
            
            //ThreadStart is currently not necessary, but you may see it used in older programs.
            ThreadStart threadStart = new ThreadStart(ThreadExample1);
            Thread thread = new Thread(threadStart);
            thread.Start(); //Start thread
            //The Thread.Join method is called on the main thread to let it wait until the other thread finishes.
            thread.Join();
            Console.WriteLine("Parent thread 1 in control again.");
            //Pass parameter to Thread. Note that the data to be passed into the thread is always passed as an object
            //reference.This means that there is no way to be sure at compile time that thread
            //initialization is being performed with a particular type of data.
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ThreadExample2);
            Thread thread1 = new Thread(parameterizedThreadStart);
            thread1.Start("Msg to thread");
            thread1.Join();
            Console.WriteLine("Parent thread 2 in control again.");
            Thread thread3 = new Thread((object data) =>
            {
                Console.WriteLine("Thread 3 started: " + data as string);
                Thread.Sleep(1000);
                Console.WriteLine("Thread 3 Finished");
            });
            thread3.Start("My message send to thread.");
            thread3.Join();
            Console.WriteLine("Thread 3 finished from control thread.");
            //Abort a thread, A Thread object exposes an Abort method, which can be called on the thread
            //to abort it. The thread is terminated instantly. When a thread is aborted it is instantly stopped. 
            //This might mean that it leaves the program in an ambiguous state, with files open and resources assigned. 
            //A better way to abort a thread is to use a shared flag variable.
            Thread thread2 = new Thread(() =>
            {
                ThreadExample3();
            });
            thread2.Start();
            Thread.Sleep((int)TimeSpan.FromSeconds(1).TotalMilliseconds);
            //+		$exception	{"Thread abort is not supported on this platform."}	System.PlatformNotSupportedException
            //thread2.Abort();
            //Example below, the thread is stopped using static flag.
            threadRunning = false;
            thread2.Join();

            Thread t1 = new Thread(() =>
            {
                Console.WriteLine("Thread starting t1.");
                Thread.Sleep(2000);
                Console.WriteLine("Thread done t1.");
            });

            Thread t2 = new Thread(() =>
            {
                Console.WriteLine("Thread starting t2.");
                Thread.Sleep(3000);
                Console.WriteLine("Thread done t2.");
            });

            //Starting the two threads and next awaiting them.
            t1.Start();
            t2.Start();
            t1.Join(); //wait here until t1 has terminated...
            t2.Join(); //wait here until t2 has terminated...

            //Thread data storage and ThreadLocal
            //If you want each thread to have its own copy of a particular variable, you can
            //use the ThreadStatic attribute to specify that the given variable should be created for each thread.
            // Thread-Local variable that yields a name for a thread
            ThreadLocal<string> ThreadName = new ThreadLocal<string>(() =>
            {
                return "Thread" + Thread.CurrentThread.ManagedThreadId;
            });

            // Action that prints out ThreadName for the current thread
            Action action = () =>
            {
                // If ThreadName.IsValueCreated is true, it means that we are not the first action to run on this thread.
                bool repeat = ThreadName.IsValueCreated;

                Console.WriteLine("ThreadName = {0} {1}", ThreadName.Value, repeat ? "(repeat)" : "");
            };

            // Launch eight of them.  On 4 cores or less, you should see some repeat ThreadNames
            Parallel.Invoke(action, action, action, action, action, action, action, action);

            // Dispose when you are done
            ThreadName.Dispose();

            ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() =>
            {
                return new Random(2);
            });

            Thread t3 = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("RandomGenerator t3: {0}", RandomGenerator.Value.Next());
                    Thread.Sleep(50);
                }
            });

            Thread t4 = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("RandomGenerator t4: {0}", RandomGenerator.Value.Next());
                    Thread.Sleep(1000);
                }
            });

            t3.Start();
            t4.Start();
            t3.Join();
            t4.Join();

        }

        private void ThreadExample1()
        {
            Console.WriteLine("ThreadExample1 started.");
            Thread.Sleep((int)TimeSpan.FromSeconds(2).TotalMilliseconds);
            Console.WriteLine("ThreadExample1 Finished.");
        }

        private void ThreadExample2(object msg)
        {
            Console.WriteLine($"ThreadExample2 started, msg: {msg as string}.");
            Thread.Sleep((int)TimeSpan.FromSeconds(2).TotalMilliseconds);
            Console.WriteLine($"ThreadExample2 Finished, msg: {msg as string}.");
        }

        static bool threadRunning = true; // flag variable
        private void ThreadExample3()
        {
            Console.WriteLine("Thread 3 started.");
            while(threadRunning)
            {
                Console.Write("*");
                Thread.Sleep(50);
            }
            Console.WriteLine("\nThread 3 finished.");
        }
    }
}
