using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Threads run on the foreground. Tasks run on the background 
//Application wont be stopped when it has foreground threads,
//Application that dont have foreground threads and 
//only background threads are stopped by OS.  

namespace _70_483.Exercises
{
    public class GeneralExercises
    {
        public GeneralExercises()
        {

        }

        const double PI = 3.1415;
        public double CalculateArea(int r)
        {
            return PI * r * r;
        }

        private void LongRunningTask(CallBack callBack)
        {
            for (int i = 0; i < 5; i++) 
            {
                callBack(i);
            };            
        }

        private void CallBackMethod(int counter)
        {
            Console.WriteLine("CallBackMethod called: {0}", counter);
        }

        private delegate double CalcAreaDel(int r);
        private delegate void CallBack(int counter);

        public async Task Run()
        {
            //-----------------------------------------------------------------------------------
            //Examples of delegates
            //-----------------------------------------------------------------------------------
            CalcAreaDel calcAreaDel = new CalcAreaDel(CalculateArea);
            //We can also define pointer to function as below; 
            CalcAreaDel calcAreaDel1 = CalculateArea;
            CalcAreaDel calcAreaDel2 = (r) => PI * r * r; ;
            //You can also use the Invoke method to call the function.
            Console.WriteLine("Area r=10: {0:N2}", calcAreaDel.Invoke(10));
            Console.WriteLine("Area r=10: {0:N2}", calcAreaDel1(10));
            Console.WriteLine("Area r=10: {0:N2}", calcAreaDel2(10));
            Func<int, double> func = (r) => PI * r * r;
            //Method can also be written as below.
            //Func<int, double> func = (r) =>
            // {
            //     return PI * r * r;
            // };
            Console.WriteLine("Area r=10: {0:N2}", func(10));
            Action<string> printMessage = (msg) => Console.WriteLine(msg);
            printMessage("Message to the Console.");
            Predicate<object> IsIntegerType = (obj) =>
            {
                if (obj.GetType() == typeof(int))
                    return true;
                else
                    return false;
            };

            Console.WriteLine("Is integer: {0},{1}", IsIntegerType("Number"), IsIntegerType(33));

            List<object> list = new List<object>() { 2020, "Dog", 2010, "Fox", "Bald Eagle" };
            List<object> digits = list.FindAll(IsIntegerType);
            foreach (object digit in digits)
                Console.WriteLine("Digit: {0}", (int)digit);
            //LongRunningTask(new CallBack(CallBackMethod));
            LongRunningTask(CallBackMethod);

            //-----------------------------------------------------------------------------------
            // Example of using interfaces, interfaces can inherit from other interfaces
            // Note because Math interface is explicitly implemented, interface methods are only visible
            // when you cast the mathLibrary instance to the IAdvancedMath interface.     
            //-----------------------------------------------------------------------------------
            IAdvancedMath mathLibrary = MathLibrary.Create();
            IMath math = MathLibrary.Create();

            //-----------------------------------------------------------------------------------
            //Use a simple extension method to add integers.
            //-----------------------------------------------------------------------------------
            int number = 17;
            Console.WriteLine("17 + 78 = {0}", number.Add(78));
            
            //-----------------------------------------------------------------------------------
            //Simple polymorphism example
            //-----------------------------------------------------------------------------------
            BaseClass baseClass = new DerivedClass("Message in a bottle");
            baseClass.Print(); //Override Print() method in DerivedClass will be called.  

            //-----------------------------------------------------------------------------------
            // Create a simple thread that calls a function.   
            // public delegate void ParameterizedThreadStart(object? obj);
            // Note delegate must point to method that accepts an object parameter. 
            //-----------------------------------------------------------------------------------
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(Function1);
            Thread thread = new Thread(parameterizedThreadStart);

            ParameterizedThreadStart parameterizedThreadStart1 = new ParameterizedThreadStart(Function2);
            Thread thread1 = new Thread(parameterizedThreadStart1);
            
            thread1.Start(10);
            thread.Start(10);

            thread.Join();
            thread1.Join();
        }

        private void Function1(object loops)
        {
            if (loops.GetType() != typeof(int))
                throw new ArgumentException("Not int");

            int maxLoop = (int)loops;

            for (int counter = 0; counter < maxLoop; counter++)
            {
                Console.WriteLine("Function 1 executed: {0}", counter);
            }
            Thread.Sleep(500);
        }

        private void Function2(object loops)
        {
            if (loops.GetType() != typeof(int))
                throw new ArgumentException("Not int");

            int maxLoop = (int)loops;

            for (int counter = 0; counter < maxLoop; counter++)
            {
                Console.WriteLine("Function 2 executed: {0}", counter);
            }
            Thread.Sleep(500);
        }
    }

    public class BaseClass
    {
        public BaseClass(string message)
        {
            Console.WriteLine("BaseClass: Constructor, message: " + message);
        }

        public virtual void Print()
        {
            Console.WriteLine("BaseClass: public virtual void Print()");
        }
    }

    public class DerivedClass : BaseClass
    {
        public DerivedClass(string message): base(message)
        {
            Console.WriteLine("DerivedClass: Constructor, message: " + message);
        }

        public override void Print()
        {
            Console.WriteLine("DerivedClass: public virtual void Print()");
        }
    }

    public static class MathExtension
    {
        public static int Add(this int x, int y)
        {
            return x + y;
        }

        public static int Subtract(this int x, int y)
        {
            return x - y;
        }
    }

    public class MathLibrary : IAdvancedMath
    {
        public MathLibrary()
        {
        }

        int IMath.Add(int x, int y)
        {
            throw new NotImplementedException();
        }

        int IAdvancedMath.Pow(int x, int y)
        {
            throw new NotImplementedException();
        }

        double IAdvancedMath.Sqrt(int x)
        {
            throw new NotImplementedException();
        }

        int IMath.Sub(int x, int y)
        {
            throw new NotImplementedException();
        }

        public static IAdvancedMath Create()
        {
            return new MathLibrary();
        }
    }

    public interface IMath
    {
        int Add(int x, int y);
        int Sub(int x, int y);
    }

    public interface IAdvancedMath : IMath
    {
        int Pow(int x, int y);
        double Sqrt(int x);
    }

}
