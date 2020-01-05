using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _70_483.Multithreading
{
    public static class Helper
    {


        //The Console class synchronizes the use of the output stream for you so you can write to it from multiple threads. 
        //Synchronization is the mechanism of ensuring that two threads don’t execute a specific portion of your program at the same time. 
        //In the case of a console application, this means that no two threads can write data to the screen at the exact same time. 
        //If one thread is working with the output stream, other threads will have to wait before it’s finished.

        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = Thread.CurrentThread;
                Console.WriteLine($"ThreadMethod: Thread Pool: {thread.IsThreadPoolThread}, Thread ID: {thread.ManagedThreadId}, priority:{thread.Priority}, IsBackground: {thread.IsBackground}.");
                Thread.Sleep(0);
            }
        }

        public static void MainThreadMethod()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.Start();

            for (int i = 0; i < 4; i++)
            {
                //Both your process and your thread have a priority.
                //A higher-priority thread should be used only when it’s absolutely necessary.
                //A new thread is assigned a priority of Normal, which is okay for almost all scenarios.
                //Another thing that’s important to know about threads is the difference between foreground and background threads. 
                //Foreground threads can be used to keep an application alive. Only when all foreground threads end does the 
                //common language runtime (CLR) shut down your application. Background threads are then terminated.
                Thread thread = Thread.CurrentThread;
                Console.WriteLine($"MainThreadMethod: Thread Pool: {thread.IsThreadPoolThread}, Thread ID: {thread.ManagedThreadId}, priority:{thread.Priority}, IsBackground: {thread.IsBackground}.");
                //Why the Thread.Sleep(0)? It is used to signal to Windows that this thread is finished. 
                //Instead of waiting for the whole time-slice of the thread to finish, it will immediately switch to another thread.
                Thread.Sleep(0);
            }

            //The Thread.Join method is called on the main thread to let it wait until the other thread finishes.
            t.Join();
        }
    }
}
