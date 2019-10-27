using System;

namespace DelegateExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [ Example01 ]
            
            //To reference a method to a delegate, the signature of both the delegate and method must 
            //match completely so:
            //1) Return types must be the same.
            //2) Input parameters must also be the same (and in matching order).
            MathDelegate mathDelegate = Add;
            Console.WriteLine($"7.12 + 6.57 = {string.Format("{0:0.00}", mathDelegate(7.12, 6.57))}. ");
            mathDelegate = Subtract;
            Console.WriteLine($"7.12 - 6.57 =  {string.Format("{0:0.00}", mathDelegate(7.12, 6.57))}. ");

            #endregion

            #region [ Example02 ]

            //Multicast Delegates In C#, delegates can be combined. 
            //You can use the +or += operator to add another method to the invocation 
            //list of an existing delegate instance.Similarly, you can also remove a method 
            //from an invocation list by using the decrement assignment operator (-or -=).
            //This feature forms the base for events in C#.
            //All this is possible because delegates inherit from the System.MulticastDelegate 
            //class that in turn inherits from System.Delegate.Because of this, you can use the 
            //members that are defined in those base classes on your delegates.
            //Note that delegate instances are immutable.So, when you combine them or 
            //subtract one delegate instance from the list, a new delegate instance is 
            //created to represent the updated or new list of the targets or methods to be invoked.
            MessageDelegate a, b, c, d, e;
            a = Message1;
            b = Message2;
            c = Message3;
            d = a + b + c;
            e = d - b;

            //Using an delegate array
            MessageDelegate[] delegates = new MessageDelegate[] 
            {
                new MessageDelegate(Message3),
                new MessageDelegate(Message2),
                new MessageDelegate(Message1)
            };

            MessageDelegate chainedDelegates = (MessageDelegate)Delegate.Combine(delegates);

            //If a multicast delegate has a nonvoid return type, the caller receives the return value 
            //from the last method to be invoked.The preceding methods are still called, but their 
            //return values are discarded.
            int invocationCount = d.GetInvocationList().GetLength(0);
            Console.WriteLine($"D delegate, {invocationCount} methods are called, retVal[{d("Mohamed")}].");
            invocationCount = e.GetInvocationList().GetLength(0);
            Console.WriteLine($"E delegate, {invocationCount} methods are called, retVal[{e("Mohamed")}].");
            invocationCount = chainedDelegates.GetInvocationList().GetLength(0);
            //Note you can also use the Invoke method to invoke 
            Console.WriteLine($"ChainedDelegates, {invocationCount} methods are called, retVal[{chainedDelegates.Invoke("Mohamed")}].");

            #endregion

            #region [ Example03 ]

            //Lambda Expressions In C#
            //Sometimes the whole signature of a method can be more code than the body of a method.
            //There are also situations in which you need to create an entire method only to use it in a delegate.
            //The lambda function has no specific name as the methods.Because of this, lambda 
            //functions are called anonymous functions. You also don’t have to specify a return type explicitly.
            //The compiler infers this automatically from your lambda. And in the case of the above example, 
            //the types of parameters x and y are also not specified explicitly.
            MathFuncDelegate mathFuncDelegate = (x, y) => x + y;
            Console.WriteLine($"Lambda expression 12.50 + 2.50 = { mathFuncDelegate(12.50 , 2.50) } ");
            //You can create lambdas that span multiple statements. You can do this by adding 
            //curly braces around the statements that form the lambda as below example shows.
            mathFuncDelegate = (x, y) =>
            {
                var result = x - y;
                return result; 
            };
            Console.WriteLine($"Lambda expression 6.75 - 1.25 = { mathFuncDelegate(6.75, 1.25) } ");

            #endregion

            #region [ Example04 ]

            //The.NET Framework has a couple of built-in delegates types that you can use when declaring delegates.
            //Built in delegates in C#:  
            //  Action<TParam1,.., TParam16>, Delegates that don't return a value and take 0 to 16 parameters.
            //  Func<TParam1, .., TReturn>, Represent delegates that return a value and take 0 to 16 parameters.
            //  For Func the Return type is mandatory but input parameter is not.
            //  Predicate, public delegate bool Predicate<in T>(T obj);
            //  Predicate represents a method that contains a set of criteria and checks whether the passed 
            //  parameter meets those criteria or not. A predicate delegate methods must take one input 
            //  parameter and return a boolean - true or false. 
            Func<int, int, int> funcDelegate = (x, y) =>
            {
                return x + y;
            };

            Console.WriteLine($"Calling funcDelegate 8 + 9 = {funcDelegate(8 , 9)}.");

            funcDelegate = (x, y) =>
            {
                return x - y;
            };

            Console.WriteLine($"Calling funcDelegate 12 - 1 = {funcDelegate(12, 1)}.");

            Action<int,int> actionDelegate = (x, y) =>
            {
                Console.WriteLine($"ActionDelegate {x} + {y} = {x + y}");
            };

            actionDelegate(45, 8);

            actionDelegate = (x, y) =>
            {
                Console.WriteLine($"ActionDelegate {x} - {y} = {x - y}");
            };

            actionDelegate(19, 6);

            //Action delegate with anonymous method
            actionDelegate = delegate (int x, int y)
            {
                Console.WriteLine($"ActionDelegate using anonymous method {x} - {y} = {x - y}");
            };

            actionDelegate(7,3);

            //Input parameters are not mandatory 
            Action actionDelegateNoParam = () =>
            {
                Console.WriteLine($"ActionDelegateNoParam");
            };

            actionDelegateNoParam.Invoke();

            //Predicate delegate with lambda
            Predicate<string> predicateIsUpper = (str) =>
            {
                return str.Equals(str.ToUpper());
            };

            Console.WriteLine($"PredicateIsUpper lambda: {predicateIsUpper("HELLO")}");

            //Predicate delegate with anonymous method
            predicateIsUpper = delegate (string s) 
            { 
                return s.Equals(s.ToUpper()); 
            };

            Console.WriteLine($"PredicateIsUpper anonymous method: {predicateIsUpper("Welcome")}");

            #endregion

            Console.ReadLine();
        }

        #region [ Example01 ]

        public delegate double MathDelegate(double value1, double value2);

        public static double Add(double value1, double value2)
        {
            return value1 + value2;
        }
        public static double Subtract(double value1, double value2)
        {
            return value1 - value2;
        }

        #endregion

        #region [ Example02 ]

        delegate int MessageDelegate(string s);

        static int Message1(string s)
        {
            Console.WriteLine("  Hello, {0}!", s);
            return 1;
        }

        static int Message2(string s)
        {
            Console.WriteLine("  Goodbye, {0}!", s);
            return 2;
        }

        static int Message3(string s)
        {
            Console.WriteLine("  Welcome, {0}!", s);
            return 3;
        }

        #endregion

        #region [ Example03 ]

        public delegate double MathFuncDelegate(double value1, double value2);

        #endregion

    }
}
