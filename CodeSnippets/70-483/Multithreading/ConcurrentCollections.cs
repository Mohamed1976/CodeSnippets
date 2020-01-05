using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _70_483.Multithreading
{
    class ConcurrentCollections
    {

        public async void Run()
        {
            //A dictionary provides a data store indexed by a key. A
            //ConcurrentDictionary can be used by multiple concurrent tasks.Actions
            //on the dictionary are performed in an atomic manner.
            ConcurrentDictionary<string, int> ages = new ConcurrentDictionary<string, int> ();
            if (ages.TryAdd("Rob", 21))
                Console.WriteLine("Rob added successfully.");
            Console.WriteLine("Rob's age: {0}", ages["Rob"]);
            // Set Rob's age to 22 if it is 21
            if (ages.TryUpdate("Rob", 22, 21))
                Console.WriteLine("Age updated successfully");
            Console.WriteLine("Rob's new age: {0}", ages["Rob"]);
            // Increment Rob's age atomically using factory method
            Console.WriteLine("Rob's age updated to: {0}", ages.AddOrUpdate("Rob", 1, (name, age) => age = age + 1));
            Console.WriteLine("Rob's new age: {0}", ages["Rob"]);

            //ConcurrentBag
            //You can use a ConcurrentBag to store items when the order in which they
            //are added or removed isn’t important. The Add items puts things into the bag,
            //and the TryTake method removes them.There is also a TryPeek method, but
            //this is less useful in a ConcurrentBag because it is possible that a following
            //TryTake method returns a different item from the bag.
            ConcurrentBag<string> bag = new ConcurrentBag<string>();
            bag.Add("Rob");
            bag.Add("Miles");
            bag.Add("Hull");
            string bagItem;
            //TryPeek method returns one item from the bag but it does not remove item from the bag.
            //It returns true if it successfully retrieve item otherwise returns false. 
            //It returns the item in the out parameter as shown in below example.
            if (bag.TryPeek(out bagItem))
                Console.WriteLine("Peek: {0}", bagItem);
            if (bag.TryTake(out bagItem))
                Console.WriteLine("Take: {0}", bagItem);

            //The ConcurrentStack class provides support for concurrent stacks. The
            //Push method adds items onto the stack and the TryPop method removes them.
            //There are also methods, PushRange and TryPopRange, which can be used to push or pop a number of items.
            ConcurrentStack<string> stack = new ConcurrentStack<string>();
            stack.Push("Rob");
            stack.Push("Miles");
            string name;
            if (stack.TryPeek(out name))
                Console.WriteLine("Peek: {0}", name);
            if (stack.TryPop(out name))
                Console.WriteLine("Pop: {0}", name);

            //Concurrent Queue
            //The Enqueue method adds items into the queue and the TryDequeue method
            //removes them. Note that while the Enqueue method is guaranteed to work
            //(queues can be of infinite length) the TryDequeue method will return false if
            //the dequeue fails. A third method, TryPeek, allows a program to inspect the
            //element at the start of the queue without removing it. Note that even if the
            //TryPeek method returns an item, a subsequent call of the TryDequeue
            //method in the same task removing that item from the queue would fail if the
            //item is removed by another task.
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            queue.Enqueue("Rob");
            queue.Enqueue("Miles");
            //It’s possible for a task to enumerate a concurrent queue(a program can use the
            //foreach construction to work through each item in the queue). At the start of
            //the enumeration a concurrent queue will provide a snapshot of the queue contents.
            foreach (string item in queue)
                Console.WriteLine("foreach item in queue: {0}", item);

            string str;
            if (queue.TryPeek(out str))
                Console.WriteLine("Peek: {0}", str);
            if (queue.TryDequeue(out str))
                Console.WriteLine("Dequeue: {0}", str);

            //The BlockingCollection<T> class is designed to be used in situations
            //where you have some tasks producing data and other tasks consuming data.It
            //provides a thread safe means of adding and removing items to a data store.It is
            //called a blocking collection because a Take action will block a task if there are
            //no items to be taken.A developer can set an upper limit for the size of the
            //collection.Attempts to Add items to a full collection are also blocked.
            //The BlockingCollection class provides the methods TryAdd and TryTake
            //that can be used to attempt an action.Each returns true if the action succeeded.
            //They can be used with timeout values and cancellation tokens.
            Task[] tasks = new Task[2];
            tasks[0] = Task.Run(() =>
            {
                CollectionProducer();
            });
            
            tasks[1] = Task.Run(() =>
            {
                CollectionConsumer();
            });

            await Task.WhenAll(tasks);
            Console.WriteLine("Task.WhenAll has finished.");
        }

        //The BlockingCollection class can act as a wrapper around other concurrent collection classes, including ConcurrentQueue,
        //ConcurrentStack, and ConcurrentBag. Blocking collection that can hold 5 items
        //If you don’t provide a collection class the BlockingCollection class uses a ConcurrentQueue, which operates
        //on a "first in-first out" basis. The ConcurrentBag class stores items in an unordered collection.
        BlockingCollection<int> data = new BlockingCollection<int>(5);
        //BlockingCollection<int> data = new BlockingCollection<int> (new ConcurrentStack<int>(), 5);
        private void CollectionProducer()
        {
            // attempt to add 10 items to the collection - blocks after 5th
            for (int i = 0; i < 11; i++)
            {
                //A developer can set an upper limit for the size of the collection.Attempts to Add items to a full collection 
                //are also blocked.
                data.Add(i);
                Console.WriteLine("Data {0} added sucessfully.", i);
            }
            //The adding task calls the CompleteAdding on the collection when it has
            //added the last item. This prevents any more items from being added to the
            //collection.The task taking from the collection uses the IsCompleted property
            //of the collection to determine when to stop taking items from it.The
            //IsCompleted property returns true when the collection is empty and
            //CompleteAdding has been called.
            data.CompleteAdding();
            Console.WriteLine("CollectionProducer() finished.");
        }

        private void CollectionConsumer()
        {
            while (!data.IsCompleted)
            {
                //Note that the Take operation is performed
                //inside try–catch construction.The Take method can throw an exception if the following sequence occurs:
                //1. The taking task checks the IsCompleted flag and finds that it is false.
                //2. The adding task (which is running at the same time as the taking task) then
                //calls the CompleteAdding method on the collection.
                //3. The taking task then tries to perform a Take from a collection which has been marked as complete.

                try
                {
                    //Take action will block a task if there are no items to be taken.
                    int v = data.Take();
                    Console.WriteLine("Data {0} taken sucessfully.", v);
                }
                catch (InvalidOperationException) { }
            }
            Console.WriteLine("CollectionConsumer() finished.");
        }
    }
}
