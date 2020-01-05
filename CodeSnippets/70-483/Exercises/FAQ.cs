using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.Exercises
{
    class FAQ
    {
        public FAQ()
        {
                
        }

        public void Run()
        {

        }

        //Q1. Given the difficulties in synchronization and management, is it worth the effort to implement applications 
        //using multiple tasks?
        //A1. Yes, because using multiple tasks allows you to fully utilize the multiple CPUs, Cores to perform calculations.  
        //However, calculations must allow calculations to be performed in paralle, can be split in multiple calculation tasks that
        //can be performed paralle on different CPUs, Cores. Furthermore multiple tasks improve responsiveness of UI.

        //Q2. Is it still worth the effort to use multiple-tasks if you only have one processor in your computer?
        //A2. Yes it can improve responsiveness of UI, for example WPF has one UI thread, if this thread is busy with long
        //calculation task, the UI is not updated and not responsive to user.

        //Q3. What kind of applications benefit the most from the use of multi-tasking applications?
        //A3. Responsiveness of UI applications. Applications that can split calculation into multiple tasks (parallelism),
        //so you can fully utilize the multiple CPUs, Cores to perform calculations.

        //Q4. Are there situations when you really should not use multi-tasking?
        //A4. For example

        //Q5. I need a background process that is going to compress a large number of data files. Should I use a task or a thread?
        //A5.

        //Q6. What is the difference between the WaitAll and WaitAny method when waiting for a large number of tasks to complete?
        //A6. WaitAll blocks/waits until all tasks are complete, WaitAny blocks/waits until one of the tasks is complete.

        //Q7. What is the difference between a continuation task and a child task?
        //A7. A continuation task continues when its parent task is complete. Child tasks are created when
        //one or more tasks are attached to a parent task, the parent task only completes when its child tasks are complete.

        //Q8. What is the difference between the WaitAll and WhenAll methods?
        //A8. WhenAll creates a Task that awaits all the tasks passed to it, the task completes when all the other tasks are completes.  
        //WaitAll

        //Q9. What happens when a method call is awaited?
        //A9. 

        //Q10. What is special about a concurrent collection?
        //A10 Concurrent collection are thread save, this means that you can update and read from the collection using different threads.
        //The collection is managed so that threads access and update the data in a synchronized way, no race conditions. 

        //11Q. Will program errors caused by a poor multithreading implementation always manifest themselves as faults in an application?
        //11A. Poor multithreading implementation results in corrupt data such as corrupt calculations, and deadlocks in the application.     

        //12Q. Does the fact that the processor is suddenly at 100% loading indicate that two processes are stuck in a deadly embrace?
        //12A. When a process is in a deadlock, the process does not have 100% CPU loading.    

        //13Q. Could you make an object thread safe by enclosing the body code of all the methods in lock statements to make all the method actions atomic?
        //13A. Lock statements should enclose code as small as possible else the performance of the application could suffer because
        //other threads could not access the locked code/methods.   

        //14Q. If you’re not sure about potential race conditions, is it best to add lock statements around critical sections of your code “just in case?”
        //14A. No you need to be sure when use lock statements, because this can lead to deadlocks.

        //15Q. Should a task always generate an exception if it is cancelled?
        //15A. No, you can cancel a task using a flag, but if you have continuation tasks that depend on cancelation exception
        //you need to throw a cancelation exception when a task is cancelled.   

        //16Q. Could you make an application that automatically cancelled deadlocked processes?
        //16A. A deadlocked process does not uses CPU, when a process does not use CPU for a longtime
        //you can stop it using taskmanager calls.

        //17Q. Is it necessary to have both the while and the do-while looping construction?
        //17A. The do-while performs the loop at least once and then checks the specified condition, 
        //the while performs the loop depending on the specified condition. So both loop construction are
        //different and needed. But they are not both necessary, you can skip the do-while.

        //18Q. Is a break statement the only way of exiting a loop before it completes?
        //18A.

        //19Q. Can you identify a situation where you would use a for loop to iterate through a collection rather than a foreach loop?
        //19A. If you dont want to iterate through all the collection you can use for loop, in addition in order to use the  
        //foreach loop, the collection you want to iterate must have implemented IEnumerable.

        //20Q. The and(&) logical operator and the or (|) logical operator have “short circuit” versions (&& and ||), 
        //which only evaluate elements until it can be determined whether or not a given expression is true or false. Why does the
        //exclusive-or(^) operator not have a short circuit version?
        //20A. The exclusive-or(^) operator needs to evaluate both conditions, because only one condition must be true.
        //To determine that only one condition is true, you need to evaluate both conditions.

        //21Q. Is it true that each C# operator has a behavior that is the same for every context in which it is used?
        //21A.

        //22Q.Can you always be certain of the precise sequence of operations when an expression is evaluated?
        //22A.

        //23Q. How does a delegate actually work?
        //23A. A delegate is a function pointer, it directs input and output to the linked function.

        //24Q. What happens if a delegate is assigned to a delegate?
        //24A. Only delegates of the same type can be assigned to each other. If this happens the delegate reference is copied.  
        //24A. The delegate passes the input and output to the other delegate, which in turn passes it to the function 

        //25Q. What happens if a delegate is assigned to itself?
        //25A. Nothing, you assign the delegate reference to itself. 

        //26Q. Is there an upper limit for the number of subscribers that a publisher delegate can have?
        //26A. There is an upper limit, 

        //27Q. What does the lambda operator (=>) actually do?
        //27A. It specifies the function that the delegate is pointing to.

        //28Q. Can code in a lambda expression access data from the enclosing code?
        //28A. Yes, lifetime of local variables referenced by lambda expression are prelonged

        //29Q. Is a method receiving a date of “31st of February” something that should cause an exception?
        //29A. This is not valid data from application user or from other system/interface.
        //This error should not cause an exception, instead the application should validate the date
        //and if not a valid date then ignore it and ask for a new date. 

        //30Q. Should I make sure that all exceptions are always caught by my program?
        //30A.

        //31Q. Can you return to the code after an exception has been caught?
        //31A. Yes depending on where the error was caught, but if its a minor error
        //you can continue with the application. In contrast an uncaught exception stops the application.

        //32Q. Why do we need the finally clause? Can’t code to be run after the code in a
        //try clause just be put straight after the end of the catch?
        //32A. Code after the catch statement is not guaranteed to execute in case of exception/error.
        //In contrast, code in a finally statement is guaranteed to execute in case of exception/error.

        //33Q. Should my application always use custom exceptions?
        //33A. No you should only use custom exceptions when standard exceptions cannot meet your requirements       
    }
}
