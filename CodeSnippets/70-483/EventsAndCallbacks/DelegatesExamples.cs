using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _70_483.EventsAndCallbacks
{
    //Func delegate
    //A Func delegate encapsulates a method that returns a value.In a Func signature, the last or rightmost type parameter 
    //always specifies the return type. One common cause of compiler errors is to attempt to pass in two input parameters 
    //to a System.Func<T, TResult>; in fact this type takes only one input parameter. The Framework Class Library defines 
    //17 versions of Func: System.Func<TResult>, System.Func<T, TResult>, System.Func<T1, T2, TResult>, and so on up through 
    //System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>.

    //Action Delegate
    //Action Delegate A System.Action delegate encapsulates a method (Sub in Visual Basic) that does not return a value, 
    //or returns void. In an Action type signature, the type parameters represent only input parameters.Like Func, the 
    //Framework Class Library defines 17 versions of Action, from a version that has no type parameters up through a version 
    //that has 16 type parameters.

    //You can take advantage of delegatese in C# to implement events and call back methods. A multicast delegate is one that 
    //can point to one or more methods that have identical signatures. A delegate is a type that safely encapsulates a method, 
    //similar to a function pointer in C and C++. Unlike C function pointers, delegates are object-oriented, type safe, and secure. 
    //The type of a delegate is defined by the name of the delegate. The following example declares a delegate named Del that can 
    //encapsulate a method that takes a string as an argument and returns void: public delegate void Del(string message);
    //Delegate types are derived from the Delegate class in the.NET Framework.Delegate types are sealed—they cannot be 
    //derived from— and it is not possible to derive custom classes from Delegate.
    //The general delegate format: access-modifier delegate result-type identifier ([parameters])

    class DelegatesExamples
    {
        public DelegatesExamples()
        {
        }

        //delegate to function that takes in two int arguments and return an int 
        private delegate int IntOperation(int a, int b);

        //Delegate definition to method returning int and void argument
        private delegate int GetValue();
        private GetValue getLocalInt = default;

        private void SetLocalInt()
        {
            // Local variable set to 99
            int localInt = 99;
            // Set delegate getLocalInt to a lambda expression that returns the value of localInt
            getLocalInt = () => localInt;
        }

        private void AlarmListener1()
        {
            Console.WriteLine("Alarm listener 1 called");
        }

        private void AlarmListener2()
        {
            Console.WriteLine("Alarm listener 2 called");
        }

        private void AlarmListener3(object sender, EventArgs e)
        {
            Console.WriteLine("Alarm listener 3 called");
        }

        private void AlarmListener4(object sender, EventArgs e)
        {
            Console.WriteLine("Alarm listener 4 called");
        }

        //Note that a reference to the same AlarmEventArgs object is passed to each
        //of the subscribers to the OnAlarmRaised event. This means that if one of the
        //subscribers modifies the contents of the event description, subsequent
        //subscribers will see the modified event. This can be useful if subscribers need to
        //signal that a given event has been dealt with, but it can also be a source of unwanted side effects.
        private void AlarmListener5(object sender, AlarmEventArgs e)
        {
            Console.WriteLine($"Alarm listener 5 called, Location: {e.Location}");
        }

        private void AlarmListener6(object sender, AlarmEventArgs e)
        {
            Console.WriteLine($"Alarm listener 6 called, Location: {e.Location}");
        }

        private void AlarmListener7(object source, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 1 called");
            Console.WriteLine("Alarm in {0}", args.Location);
            //throw new Exception("Bang");
        }

        private void AlarmListener8(object source, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 2 called");
            Console.WriteLine("Alarm in {0}", args.Location);
            //throw new Exception("Boom");
        }

        private delegate void myWay();
        private delegate void anotherWay();

        private void Method1()
        {
            Console.WriteLine("Inside Method1...");
        }

        private void Method2()
        {
            Console.WriteLine("Inside Method2...");
        }

        private delegate int MathOperation(int a, int b);

        private Func<int, int, int> AddOperation = (x, y) => x + y;

        private T[] FiterCollection<T>(T[] collection, Predicate<T> predicate)
        {
            T[] results = Array.FindAll(collection, predicate);
            return results;
        }

        private static int certificationYear = 2020;


        public static int CertificationYear
        {
            get { return certificationYear; }
            set { certificationYear = value; }
        }

        private Action PrintCertificationYear = () => Console.WriteLine($"CertificationYear: {CertificationYear}");

        private static int counter = 0;
        private Action ClosureExample()
        {            
            return delegate
            {
                counter++;
                Console.WriteLine("Delegate closure example using static counter, counter={0}", counter);
            };
        }

        private Action ClosureExample2()
        {
            int localCounter = 0;
            return delegate
            {
                localCounter++;
                Console.WriteLine("Delegate closure example using local counter, counter={0}", localCounter);
            };
        }

        public static void DelegateMethod(string message)
        {
            Console.WriteLine($"DelegateMethod(string message) message{message}");
        }

        private delegate void printMsgDel(string msg);

        public void Run()
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/using-delegates
            //At this point allMethodsDelegate contains three methods in its invocation list—Method1, Method2, and DelegateMethod. 
            //The original three delegates, d1, d2, and d3, remain unchanged. When allMethodsDelegate is invoked, all three methods 
            //are called in order.If the delegate uses reference parameters, the reference is passed sequentially to each of the 
            //three methods in turn, and any changes by one method are visible to the next method. When any of the methods throws 
            //an exception that is not caught within the method, that exception is passed to the caller of the delegate and no 
            //subsequent methods in the invocation list are called.If the delegate has a return value and / or out parameters, 
            //it returns the return value and parameters of the last method invoked.To remove a method from the invocation list, 
            //use the subtraction or subtraction assignment operators(-or -=). //remove Method1 (allMethodsDelegate -= d1);
            // copy AllMethodsDelegate while removing d2 (Del oneMethodDelegate = allMethodsDelegate - d2);
            DelegateExampleMethods obj = new DelegateExampleMethods("Delegate ref obj.method");
            printMsgDel printMsgDel1 = obj.Method1;
            printMsgDel printMsgDel2 = obj.Method2;
            printMsgDel printMsgDel3 = DelegateMethod;

            printMsgDel allMethodsDelegate = printMsgDel1 + printMsgDel2 + printMsgDel3;
            Delegate [] delegates = allMethodsDelegate.GetInvocationList();
            foreach(Delegate del in delegates)
            {
                Console.WriteLine($"Delegate Method.Name {del.Method.Name} ");
            }

            allMethodsDelegate("Aanroep door delegate.");

            return;
            //Closure Example
            //The code in a lambda expression can access variables in the code around it.
            //These variables must be available when the lambda expression runs, so the
            //compiler will extend the lifetime of variables used in lambda expressions.
            Action del1 = ClosureExample();
            Action del2 = ClosureExample();
            Action del3 = ClosureExample();
            //References a static counter that is incremented
            del1(); //prints 1
            del2(); //prints 2
            del3(); //prints 3

            Action del4 = ClosureExample2();
            Action del5 = ClosureExample2();
            Action del6 = ClosureExample2();
            //References a local counter that is incremented
            del4(); //prints 1
            del5(); //prints 1
            del6(); //prints 1

            //When a delegate is constructed to wrap an instance method, the delegate references both the instance and the method.
            //A delegate has no knowledge of the instance type aside from the method it wraps, so a delegate can refer to any type 
            //of object as long as there is a method on that object that matches the delegate signature. When a delegate is 
            //constructed to wrap a static method, it only references the method. You cannot reference CertificationYear if it is not
            //a static method. 
            PrintCertificationYear();
            CertificationYear = 2021;
            PrintCertificationYear();

            //Because the instantiated delegate is an object, it can be passed as a parameter, or assigned to a property.
            //This allows a method to accept a delegate as a parameter, and call the delegate at some later time. 
            //This is known as an asynchronous callback, and is a common method of notifying a caller when a long 
            //process has completed. When a delegate is used in this fashion, the code using the delegate does not 
            //need any knowledge of the implementation of the method being used.The functionality is similar to the 
            //encapsulation interfaces provide. Another common use of callbacks is defining a custom comparison method 
            //and passing that delegate to a sort method. It allows the caller's code to become part of the sort algorithm. 
            //The following example method uses the predicate delegate type as a parameter:
            Predicate<Point> filter = (point) => point.X * point.Y > 100000;
            Point[] points1 = 
            { 
                new Point(100, 200),
                new Point(150, 250), 
                new Point(250, 375),
                new Point(275, 395), 
                new Point(295, 450) 
            };

            Point[] points2 = FiterCollection<Point>(points1, filter);

            foreach(Point p in points2)
            {
                Console.WriteLine($"FiterCollection: X[{p.X}], Y[{p.Y}]"); 
            }

            myWay d1 = Method1;
            myWay d2 = new myWay(Method2);
            //A delegate can call more than one method when invoked. This is referred to as multicasting.
            //To add an extra method to the delegate's list of methods—the invocation list—simply requires 
            //adding two delegates using the addition or addition assignment operators (' + ' or ' += '). For example:
            //Combining two delegates 
            myWay multicastDelegate = (myWay)Delegate.Combine(d1, d2); //the same as: myWay multicastDelegate = d1 + d2;

            //Calling the two methods;
            multicastDelegate();
            //https://stackoverflow.com/questions/4467412/cannot-assign-a-delegate-of-one-type-to-another-even-though-signature-matches
            //You can't cast from one delegate-type to another simply because their declarations are similar.
            //Delegate types in C# are name equivalent, not structurally equivalent. Specifically, two different 
            //delegate types that have the same parameter lists and return type are considered different delegate types.
            //anotherWay d3 = (anotherWay)d2; //THIS DOES NOT WORK, see comment above
            anotherWay d3 = new anotherWay(Method2); ;
            d3();
            Console.WriteLine("AddOperation 4 + 5  = {0}", AddOperation(4, 5));
            //Defining the MathOperation delegate with a lambda expression calling another delegate (Func)  
            MathOperation mathOperation1 = (x, y) => AddOperation(x, y);
            Console.WriteLine("MathOperation 4 + 5  = {0}", mathOperation1(4, 5));
            //Assigning AddOperation Func to MathOperation delegate;   
            MathOperation mathOperation2 = new MathOperation(AddOperation);
            Console.WriteLine("MathOperation(AddOperation) 4 + 5  = {0}", mathOperation2(4, 5));

            //Represents the method that defines a set of criteria and determines whether the specified object meets those criteria.
            //public delegate bool Predicate<in T>(T obj);
            // Create an array of Point structures.
            Point[] points = { new Point(100, 200),
                         new Point(150, 250), new Point(250, 375),
                         new Point(275, 395), new Point(295, 450) };

            Predicate<Point> predicate = (Point point) =>
            {
                return point.X * point.Y > 100000;
            };

            // Find the first Point structure for which X times Y is greater than 100000. 
            Point[] results = Array.FindAll(points, predicate);
            foreach(Point point in results)
            {
                Console.WriteLine($"Point, x: {point.X}, y: {point.Y}");
            }
            
            //C# provides events as mechanism by which one component can send a message to another.
            //In the days before async and await were added to the C# language, a
            //program would be forced to use events to manage asynchronous operations.
            //Before initiating an asynchronous task, such as fetching a web page from a
            //server, a program would need to bind a method to an event that would be
            //generated when the action was complete.
            //Simplest Action delegate 
            Action action = () =>
            {
                Console.WriteLine("Action delegate is called.");
            };
            action();
            //Example below shows how an Action delegate can be used to create an event publisher.
            // Create a new alarm
            Alarm alarm = new Alarm();

            //Subscribers bind to a publisher by using the += operator. The += operator is
            //overloaded to apply between a delegate and a behavior.It means “add this
            //behavior to the ones for this delegate.” The methods in a delegate are not
            //guaranteed to be called in the order that they were added to the delegate.
            // Connect the two listener methods
            alarm.OnAlarmRaised += AlarmListener1;
            alarm.OnAlarmRaised += AlarmListener2;
            //You’ve seen that the += operator has been overloaded to allow methods to bind
            //to events. The -= method is used to unsubscribe from events.
            alarm.RaiseAlarm();
            Console.WriteLine("Alarm raised.");

            //The Alarm object that we’ve created is not particularly secure.The
            //OnAlarmRaised delegate has been made public so that subscribers can
            //connect to it.However, this means that code external to the Alarm object can
            //raise the alarm by directly calling the OnAlarmRaised delegate. External
            //code can overwrite the value of OnAlarmRaised, potentially removing
            //subscribers. C# provides an event construction that allows a delegate to be specified as an event.
            //Remove event handlers  
            alarm.OnAlarmRaised -= AlarmListener1;
            alarm.OnAlarmRaised -= AlarmListener2;
            //Add event handlers
            alarm.OnAlarmRaisedV2 += AlarmListener1;
            alarm.OnAlarmRaisedV2 += AlarmListener2;
            alarm.RaiseAlarm();
            Console.WriteLine("Alarm raised.");

            alarm.OnAlarmRaisedV2 -= AlarmListener1;
            alarm.OnAlarmRaisedV2 -= AlarmListener2;
            alarm.OnAlarmRaisedV3 += AlarmListener3;
            alarm.OnAlarmRaisedV3 += AlarmListener4;
            alarm.RaiseAlarm();
            alarm.OnAlarmRaisedV3 -= AlarmListener3;
            alarm.OnAlarmRaisedV3 -= AlarmListener4;
            alarm.OnAlarmRaisedV4 += AlarmListener5;
            alarm.OnAlarmRaisedV4 += AlarmListener6;
            alarm.RaiseAlarm();

            alarm.OnAlarmRaisedV4 -= AlarmListener5;
            alarm.OnAlarmRaisedV4 -= AlarmListener6;
            alarm.OnAlarmRaisedV4 += AlarmListener7;
            alarm.OnAlarmRaisedV4 += AlarmListener8;
            try
            {
                alarm.RaiseAlarm();
            }
            catch(AggregateException agg)
            {
                foreach (Exception ex in agg.InnerExceptions)
                    Console.WriteLine($"Exception {ex.Message}");
            }

            //Closures
            //The code in a lambda expression can access variables in the code around it.
            //These variables must be available when the lambda expression runs, so the
            //compiler will extend the lifetime of variables used in lambda expressions.
            //The call to function SetLocalInt() sets the getLocalInt int delegate to point to local variable localInt.
            //The lifetime of the local variable localInt is extended  
            SetLocalInt();
            Console.WriteLine($" getLocalInt = {getLocalInt()}");

            //If the lambda expression doesn’t return a result, you can use the Action type
            Action<string> logMessage = (string msg) =>
            {
                Console.WriteLine($"From Action<string> {msg}");
            };
            logMessage("Log message");

            //The Predicate built in delegate type lets you create code that takes a value
            //of a particular type and returns true or false.
            Predicate<int> dividesByThree = (int number) =>
            {
                return number % 3 == 0;
            };

            Console.WriteLine($"9 % 3 == 0 {dividesByThree(9)}");
            
            //Use lambda expressions (anonymous methods)
            //Delegates allow a program to treat behaviors(methods in objects) as items of
            //data.A delegate is an item of data that serves as a reference to a method in an
            //object.This adds a tremendous amount of flexibility for programmers.However,
            //delegates are hard work to use.The actual delegate type must first be declared
            //and then made to refer to a particular method containing the code that describes
            //the action to be performed.
            //Lambda expressions are a pure way of expressing the “something goes in,
            //something happens and something comes out” part of behaviors.The types of
            //the elements and the result to be returned are inferred from the context in which
            //the lambda expression is used. The operator => is called the lambda operator.
            IntOperation addLambda = (int x, int y) =>
            {
                return x + y; 
            };
            //Could be simplified to: IntOperation addLambda = (a, b) => a + b;
            Console.WriteLine($" using lambda 7 + 5 = {addLambda(7, 5)}");

            Func<int, int, int> addFunc = (a, b) =>
            {
                Console.WriteLine("Add called");
                return a + b;
            };

            Console.WriteLine($" local Func del 7 + 5 = {addFunc(7, 5)}");

            //Delegates can be used in exactly the same way as any other variable. You can
            //have lists and dictionaries that contain delegates and you can also use them as parameters to methods.
            //There are several pre-defined delegates such as Func<>, Action, Predicate, EventHandler 
            IntOperation mathOperation = new IntOperation(Add); //Delegate refers to Add function
            Console.WriteLine($" 7 + 5 = {mathOperation(7,5)}");
            mathOperation = Subtract;
            Console.WriteLine($" 7 - 5 = {mathOperation(7, 5)}");

            //It is important to understand the difference between delegate (with a lowercase d) 
            //and Delegate(with an upper-case D). The word delegate with a lower -case d is the keyword 
            //used in a C# program that tells the compiler to create a delegate type. 
            //The word Delegate with an upper-case D is the abstract class that defines the behavior of delegate instances.
            //Once the delegate keyword has been used to create a delegate type, objects of that delegate type will be realized as
            //Delegate instances.
            IntOperation op;
            //This statement creates an IntOperation value called op. The variable op
            //is an instance of the System.MultiCastDelegate type, which is a child of
            //the Delegate class. A program can use the variable op to either hold a
            //collection of subscribers or to refer to a single method.
        }

        private int Add (int x, int y)
        {
            return x + y;
        }

        private int Subtract(int x, int y)
        {
            return x - y;
        }
    }
}
