using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
    //Today events are more frequently used for inter-process communication.
    //Components of a solution that communicate using events are described as loosely coupled.
    //The methods in a delegate are not guaranteed to be called in the order that they were added to the delegate.
    //Delegates added to a published event are called on the same thread as the
    //thread publishing the event. If a delegate blocks this thread, the entire
    //publication mechanism is blocked.This means that a malicious or badly written
    //subscriber has the ability to block the publication of events. This is addressed by
    //the publisher starting an individual task to run each of the event subscribers.
    //If the same subscriber is added more than once to the same publisher, it will
    //be called a corresponding number of times when the event occurs.
    //The word Delegate with an upper-case D is the abstract class that defines the behavior of delegate instances.
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

        //Note that a reference to the same AlarmEventArgs object is passed to each
        //of the subscribers to the OnAlarmRaised event. This means that if one of the
        //subscribers modifies the contents of the event description, subsequent
        //subscribers will see the modified event. This can be useful if subscribers need to
        //signal that a given event has been dealt with, but it can also be a source of
        //unwanted side effects.
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

        delegate int MathFunctions(int a, int b);

        internal Action<int> updateCapturedLocalVariable;
        internal Action showCapturedLocalVariable;

        private int localCounter = 0;

        public int LocalCounter
        {
            get { return localCounter; }
            set { localCounter = value; }
        }


        private void CaptureLocalVariable(int input)
        {
            int j = 0;

            //Lambda expression is defined in function and called later in this function 
            updateCapturedLocalVariable = (x) =>
            {
                j = x;
                LocalCounter++;
            };

            showCapturedLocalVariable = () =>
            {
                Console.WriteLine($"input: [{input}], j: [{j}], LocalCounter: [{LocalCounter}]");
            };

            Console.WriteLine($"Local variable before lambda invocation: {j}");
            updateCapturedLocalVariable(10);
            Console.WriteLine($"Local variable after lambda invocation: {j}");
        }


        public async Task Run()
        {
            // You have a private method in your class and you want to make invocation of the method possible by certain callers.
            //Use a method that returns a delegate to authorized callers
            Alarm alarm2 = new Alarm();
            Alarm.delegateDisplayUserCredentials authorizationDelegate = alarm2.AuthorizeRequest("Welcome123");
            authorizationDelegate.DynamicInvoke("Mohamed");

            //The following example produces a sequence that contains all elements in the numbers array that precede the 9, 
            //because that's the first number in the sequence that doesn't meet the condition:
            int[] numbers2 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var firstNumbersLessThanSix = numbers2.TakeWhile(n => n < 6);
            Console.WriteLine(string.Join(" ", firstNumbersLessThanSix));

            //The following example specifies multiple input parameters by enclosing them in parentheses. 
            //The method returns all the elements in the numbers array until it encounters a number whose 
            //value is less than its ordinal position in the array:
            int[] numbers3 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var firstSmallNumbers = numbers3.TakeWhile((n, index) => n >= index);
            Console.WriteLine(string.Join(" ", firstSmallNumbers));

            List<HockeyTeam> teams1 = new List<HockeyTeam>();
            teams1.AddRange(new HockeyTeam[] { new HockeyTeam("Detroit Red Wings", 1926),
                                         new HockeyTeam("Chicago Blackhawks", 1926),
                                         new HockeyTeam("San Jose Sharks", 1991),
                                         new HockeyTeam("Montreal Canadiens", 1909),
                                         new HockeyTeam("St. Louis Blues", 1967) });
            //Func<HockeyTeam, bool>
            List<HockeyTeam> selectedTeams1 = teams1.Where((hockeyTeam) => hockeyTeam.Founded == 1909 || hockeyTeam.Founded == 1967).ToList();
            Console.WriteLine($"HockeyTeams founded in 1909 or 1967: ");
            foreach (HockeyTeam h in selectedTeams1)
            {
                Console.WriteLine($"{h.Name}, {h.Founded} ");
            }
            
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
            Alarm alarm1 = new Alarm();
            alarm1.OnAlarmRaisedV4 += async (object sender, AlarmEventArgs e) =>
            {
                Console.WriteLine($"async alarm1.OnAlarmRaisedV4: {e.Location}");
                // The following line simulates a task-returning asynchronous process.
                await Task.Delay(1000);
            };

            alarm1.RaiseAlarm();

            int[] numbers1 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            //Predicate to determine which digits to count  
            int oddNumbers = numbers1.Count(n => n % 2 == 1);
            Console.WriteLine($"There are {oddNumbers} odd numbers in {string.Join(" ", numbers1)}");

            //Sometimes it's impossible for the compiler to infer the input types. You can specify the types 
            //explicitly as shown in the following example:
            Func<int, string, bool> isTooLong = (int maxLength, string str) => str.Length > maxLength;
            isTooLong(3, "Welcome");
            
            int[] numbers = { 2, 3, 4, 5 };
            //Each number is processed using anonymous function: Fun<int,int> square = (x) => x*x;    
            int[] squaredNumbers = numbers.Select(x => x * x).ToArray();
            foreach(int i in squaredNumbers)
            {
                Console.WriteLine($"SquaredNumbers: {i}");
            }

            //Closures
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
            //The code in a lambda expression can access variables in the code around it.
            //These are the variables that are in scope in the method that defines the lambda expression, 
            //or in scope in the type that contains the lambda expression.
            //These variables must be available when the lambda expression runs, so the
            //compiler will extend the lifetime of variables used in lambda expressions.         
            CaptureLocalVariable(5);
            //The lasttime CaptureLocalVariable was called, j==10, input==5, LocalCounter==1 these values were captured    
            showCapturedLocalVariable();
            updateCapturedLocalVariable(3);
            //Last call input==5, J==3, LocalCounter==2
            showCapturedLocalVariable();

            //Expression lambda that has an expression as its body:
            //1) (input-parameters) => expression
            //2) (input-parameters) => { <sequence-of-statements> }
            //Use lambda expressions Lambda expressions are a pure way of expressing the “something goes in,
            //something happens and something comes out” part of behaviors.The types of
            //the elements and the result to be returned are inferred from the context in which
            //the lambda expression is used.Consider the following statement.            
            //The operator => is called the lambda operator.
            //Any lambda expression can be converted to a delegate type. 
            MathFunctions AddOperation = (x, y) => x + y;
            //You can also use statements enclosed in a block.
            AddOperation = (int a, int b) =>
            {
                Console.WriteLine($"Add called");
                return a + b;
            };
            Console.WriteLine($"AddOperation: 9 + 6 = {AddOperation(9,6)}");

            Func<int, int, int> AddOperation2 = (x, y) => x + y;
            Action<string> printResult = (result) => Console.WriteLine(result);
            printResult($"AddOperation2: 8 + 11 = {AddOperation2(8, 11)}");

            //Anonymous methods is a lambda expression that is directly used in a context where you just want to 
            //express a particular behavior. The program below uses Task.Run to start a new task.The code
            //performed by the task is expressed directly as a lambda expression, which is
            //given as an argument to the Task.Run method. At no point does this code ever have a name.
            //Task.Run(Action) method to pass the code that should be executed in the background. 
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(50);
                }
            });

            //https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netframework-4.8
            //Predicate<T> Delegate: public delegate bool Predicate<in T>(T obj);
            //Represents the method that defines a set of criteria and determines whether the specified object meets those criteria.
            //Typically, the Predicate<T> delegate is represented by a lambda expression. Because locally scoped variables are available 
            //to the lambda expression, it is easy to test for a condition that is not precisely known at compile time.
            List<HockeyTeam> teams = new List<HockeyTeam>();
            teams.AddRange(new HockeyTeam[] { new HockeyTeam("Detroit Red Wings", 1926),
                                         new HockeyTeam("Chicago Blackhawks", 1926),
                                         new HockeyTeam("San Jose Sharks", 1991),
                                         new HockeyTeam("Montreal Canadiens", 1909),
                                         new HockeyTeam("St. Louis Blues", 1967) });


            int[] years = { 1920, 1930, 1980, 2000 };
            Random rnd = new Random();
            //rnd.Next(0, years.Length), rnd.Next(0,4), generates numbers between 0 and 3, 4 not included   
            int foundedBeforeYear = years[rnd.Next(0, years.Length)];
            Console.WriteLine("Teams founded before {0}:", foundedBeforeYear);
            List<HockeyTeam> teamsSelected = teams.FindAll((team) => team.Founded < foundedBeforeYear);
            foreach(HockeyTeam team in teamsSelected)
            {
                Console.WriteLine($"{team.Name}, {team.Founded}");
            }
             
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
            //The C# compiler will automatically generate the code to create a delegate instance when a method is 
            //assigned to the delegate variable.
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
            //Delegate instances. IntOperation op;
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
