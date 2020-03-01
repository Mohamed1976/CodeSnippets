using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.GarbageCollector
{
    //The .NET framework that underpins C# programs provides a managed environment for our
    //programs that perform memory management in the form of a garbage collection
    //process that will remove unwanted objects without us having to do anything.
    //Our programs must also deal with “unmanaged” resources, such as handle to a file,
    //database connection. The program must make sure that when this object is destroyed, 
    //any resources connected to the object must be released in a managed way.
    //C# provides a finalization process that allows code in an object to get control as it is being removed from memory.
    //C# also allows an object to implement an IDisposable interface
    //When reference variable goes out of scope or a reference variable has no reference to it, they are potential target for GC.  
    //Garbage collection only occurs when the amount of memory available for new objects falls below a threshold.

    //When an application runs low on memory the garbage collector will search the
    //heap for any objects that are no longer required and remove them.The.NET
    //runtime contains an index of all the objects that have been created since the
    //program started, the job of the garbage collector is to decide which of them are
    //still in use, remove them from memory, and then compact the remaining objects
    //so that the area of free memory is a single large area, rather than a number of smaller, free areas.

    //All managed threads are suspended while the garbage collector is running.
    //This means that your application will stop responding to inputs while the
    //garbage collection is performed.It is possible to invoke the garbage collector
    //manually if there are points in your application when you know a large number
    //of objects have been released.

    //The garbage collector attempts to determine which objects are long lived, and
    //which are short lived(ephemeral). It does this by adding a generation counter to
    //each object on the heap. Objects start at generation 0. If they survive a garbage
    //collection the counter is advanced to generation 1. Surviving a second garbage
    //collection promotes an object to generation 2, the highest generation.The
    //garbage collector will collect different generations, depending on circumstances.
    //A “level 2” garbage collection will involve all objects. A “level 0” garbage
    //collection will target newly created objects.

    //The garbage collector can run in “workstation” or “server” modes, depending
    //on the role of the host system. There is also an option to run garbage collection
    //concurrently on a separate thread. However, this increases the amount of
    //memory used by the garbage collector and the loading on the host processor.

    class GarbageCollectorExamples
    {
        public GarbageCollectorExamples()
        {

        }

        private void CreateObjectOutOfScope()
        {
            //Create unreferenced object, so GC can collect it.   
            DerivedDerivedClass d = new DerivedDerivedClass();
        }

        public void Run()
        {
            //-----------------------------------------------------------------------------------
            // The Abstract Stream class implements the IDisposable interface. 
            // This means that any objects derived from the Stream type must also implement the interface.
            // The disposable procedure is implemented in the AbstractClass DerivedFromAbstractClass
            // This pattern can also be used in base and derived classes. 
            //-----------------------------------------------------------------------------------
            DerivedFromAbstractClass derivedFromAbstractClass = new DerivedFromAbstractClass("MyName");
            derivedFromAbstractClass.Dispose();

            //https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
            //You should only implement a finalizer if you have actual unmanaged resources to dispose.
            //The primary use of this interface is to release unmanaged resources.The garbage collector automatically 
            //releases the memory allocated to a managed object when that object is no longer used.However, 
            //it is not possible to predict when garbage collection will occur.Furthermore, the garbage collector 
            //has no knowledge of unmanaged resources such as window handles, or open files and streams.
            //The dispose pattern is used only for objects that access unmanaged resources, such as file and pipe handles, 
            //registry handles, wait handles, or pointers to blocks of unmanaged memory.This is because the garbage collector 
            //is very efficient at reclaiming unused managed objects, but it is unable to reclaim unmanaged objects.
            CreateObjectOutOfScope();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            DerivedDerivedClass derivedDerivedClass = new DerivedDerivedClass();
            derivedDerivedClass.Dispose();
            
            DerivedClass derivedClass = new DerivedClass();
            derivedClass.Dispose();
            
            //There are two mechanisms that we can use that allow us to get control at the
            //point an object is being destroyed and tidy up any resources that the object may
            //be using. They are finalization (similar to destructor) and disposable.
            //Problems with finalization
            //When the garbage collector is about to remove an object, it checks to see if the
            //object contains a finalizer method.If there is a finalizer method present, the
            //garbage collector adds the object to a queue of objects waiting to be finalized.
            //Once all of these objects have been identified, the garbage collector starts a
            //thread to execute all the finalizer methods and waits for the thread to complete.
            //Once the finalization methods are complete the garbage collector performs
            //another garbage collection to remove the finalized objects.There are no
            //guarantees as to when the finalizer thread will run.Objects waiting to be
            //finalized will remain in memory until all of the finalizer methods have
            //completed and the garbage collector has made another garbage collection pass to
            //remove them. A slow - running finalizer can seriously impair the garbage collection process.
            //Another problem with finalization is that there is no guarantee that the
            //finalizer method will ever be called. If the program never runs short of memory,
            //it might not need to initiate garbage collection. This means that an object waiting
            //for deletion may remain in memory until the program completes.
            //Implement IDisposable
            //An object can implement the IDisposable interface, which
            //means it must provide a Dispose method that can be called within the
            //application to request that the object to release any resources that it has
            //allocated.Note that the Dispose method does not cause the object to be
            //deleted from memory, nor does it mark the object for deletion by the garbage
            //collector in any way.Only objects that have no references to them are deleted.
            //Once Dispose has been called on an object, that object can no longer be used in an application.
            ClassWithFinalizer classWithFinalizer = new ClassWithFinalizer();
            classWithFinalizer.Dispose();

            //Note that the using statement ensures that Dispose is called on an object
            //in the event of exceptions being thrown.If you don’t use the using statement to
            //manage calls of Dispose in your objects, make sure that your application calls
            //Dispose appropriately.The dispose pattern above results in a disposal
            //behavior that is tolerant of multiple calls of Dispose.
            using(ClassWithFinalizer classWithFinalizer2 = new ClassWithFinalizer())
            {
                //Do something
                //classWithFinalizer2
            }

            //You have seen that an application can force a garbage collection to take place by
            //calling the Collect method.After the collection has been performed, a program
            //can then be made to wait until all the finalizer methods have completed:
            Console.WriteLine("Creating objects that will be removed by GC, finalizer method call.");
            for(int i = 0; i<5; i++)
            {
                //creating accessible object so that objects can be collected by GC 
                ClassWithFinalizer c = new ClassWithFinalizer();
            }

            //Waiting for the GC to call Finalizers
            //Overloads of the Collect method allow you to specify which generation of
            //objects to garbage collect and set other garbage collection options.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //Program below deliberately creates a large number of inaccessible objects.
            //This causes the garbage collector to trigger when available memory falls below a threshold.  
            //Running the for loop below you would see increase of memory usage by the process,
            //at a certain memory treshold the GC kicks in and removes not accessible person objects thereby freeing memory.
            //The storage graph displayed in VS shows the size of the heap. The heap is the area
            //of memory where an application stores objects that are referred to by reference.
            //The contents of value types are stored on the stack. The stack automatically
            //grows and contracts as programs run.Upon entry to a new block the.NET
            //runtime will allocate space on the stack for values that are declared local to that
            //block.When the program leaves the block the.NET runtime will automatically
            //contract the stack space, which removes the memory allocated for those variables.

            //for (long i = 0; i < 100000000000; i++)
            //{
            //    Student t = new Student();
            //}

            //If you find that the garbage collection process is becoming intrusive you can
            //force a garbage collection by calling the Collect method on the garbage collector as shown below.
            //The enforced garbage collection can be performed at points in your
            //application where you know large objects have just been released, for example at
            //the end of a transaction or upon exit from a large and complex user interface
            //dialog. However, under normal circumstances I would strongly advise you to let
            //the garbage collector look after itself.It is rather good at doing that.
            GC.Collect();

            //You have seen that the garbage collector can be ordered to ignore the finalizer on
            //an object.The statement below prevents finalization from being called on the
            //object referred to by t.
            Student t = new Student();
            GC.SuppressFinalize(t);
            //To later re-enable finalization you can use the ReRegisterforFinalize method:
            GC.ReRegisterForFinalize(t);
        }

        private class Student
        {
            long[] personArray = new long[1000000];
        }

    }
}
