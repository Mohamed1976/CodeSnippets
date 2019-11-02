using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VehicleRegistration;

namespace CodeSnippets
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [ What is new in C# 8.0 ]
            //https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
            //readonly method
            Rectangle rectangle = new Rectangle(10, 10);
            Console.WriteLine("Rectangle area: [{0}]", rectangle.Area());
            //Indices and ranges
            string[] digits = new string[]
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"
            };
            //Display last digit "Ten"
            Console.WriteLine($"digits[^1]: {digits[^1]}");
            //Display "Nine"
            Console.WriteLine($"digits[^2]: {digits[^2]}");
            //Displays the complete list similar to foreach(string digit in digits) 
            foreach (string digit in digits[0..^0])
            {
                Console.WriteLine($"String digit in digits[0..^0]: {digit}");
            }
            //Displays "Two" .. "Nine"
            foreach (string digit in digits[1..^1])
            {
                Console.WriteLine($"String digit in digits[1..^1]: {digit}");
            }
            //Displays Index=2, 3, 4(not included) "Three", "Four"
            foreach (string digit in digits[2..4])
            {
                Console.WriteLine($"String digit in digits[2..4]: {digit}");
            }
            //Using declarations, no brackets ({}) needed to define scope of using statement
            //The dispose method is called when using statement gets out of scope
            /*
            using var inputFile = new StreamReader(@"c:\inputFile.txt");
            using var outputFile = new StreamWriter(@"c:\outputFile.txt");
            string line; 
            while((line = inputFile.ReadLine()) != null)
            {
                outputFile.WriteLine(line);
            }*/

            //using switch expressions instead of switch statement
            Console.WriteLine($"7 + 3 = {Calculator(7, 3, MathOperation.Add)}");
            //The IShoppingCart has an optional method that is not mandatory to implement.
            IShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.CalculateTotal();
            //null coalescing assignment operator
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            List<int> numbers = null;
            //If the list is not allocated, allocate new list 
            numbers ??= new List<int>();
            numbers.AddRange(new int[] { 5, 9, 11, 17 });
            Console.WriteLine($"After new list is allocated, items [{string.Join(" ", numbers)}]");
            int indexOfSetToSum = 1;
            double sumResult = 0;
            List<double[]> numberArrays = null;
            sumResult = numberArrays?[indexOfSetToSum]?.Sum() ?? double.NaN;
            Console.WriteLine($"Should be NaN: {sumResult}");
            numberArrays = new List<double[]>();
            numberArrays.Add(new double[] { 6, 8, 9, 11, 13 });
            numberArrays.Add(new double[] { 16, 28, 39, 311, 613 });
            numberArrays.Add(new double[] { 45, 85, 32, 324, 97 });
            sumResult = numberArrays?[indexOfSetToSum]?.Sum() ?? double.NaN;
            Console.WriteLine($"Should be {16 + 28 + 39 + 311 + 613}: {sumResult}");
            //In past do asynchronous call and wait for all the data to be available.
            //Using IAsyncEnumerable, As the data is available you yield it and return it. 
            //Gets Data and returns it immediately, then waits for data await async Task.. etc
            //Async iterators are useful when you have to get some data, process the data and get some more. 
            /* Uncomment to see behaviour
            AsyncIterator();
            */

            try
            {
                decimal toll = CalculateToll(new Car { Passengers = 3 }, DateTime.Now.AddDays(3), inbound:true);
                Console.WriteLine($"CalculateToll {toll}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            #endregion

            #region [ Yield keyword ]

            //When using the Yield method, after each yield, control is returned to the calling method
            List<string> stringList = new List<string>(new string[] { "Alfa", "Beta", "Gamma", "Een", "Delta", "Ywe" });
            foreach (string str in Filter(stringList))
            {
                Console.WriteLine($"Yield keyword calling method: {str}");
            }

            List<string> results = Filter(stringList).ToList();
            foreach (string str in results)
            {
                Console.WriteLine($"Yield keyword after results received: {str}");
            }

            //The Yield method variables retain their value between function calls
            foreach (int i in RunningTotal())
            {
                Console.WriteLine($"Yield keyword RunningTotal(): {i}");
            }

            #endregion

            Console.ReadLine();
        }

        #region [ Example01 ]

        public static double Calculator(double x, double y, MathOperation operation)
        {
            double result = 0;

            //Switch Expressions
            result = operation switch
            {
                MathOperation.Add => x + y,
                MathOperation.Subtract => x - y,
                MathOperation.Multiply => x * y,
                MathOperation.Divide => x / y,
                _ => throw new NotImplementedException("Math operation not implemented.")
            };

            return result;

            //The switch statement
            /*switch (operation)
            {
                case MathOperation.Add:
                    result = x + y;
                    break;
                case MathOperation.Subtract:
                    result = x - y;
                    break;
                case MathOperation.Multiply:
                    result = x * y;
                    break;
                case MathOperation.Divide:
                    result = x / y;
                    break;
                default:
                    throw new NotImplementedException("Math operation not implemented.");
            }

            return result;*/
        }

        public enum MathOperation
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        public struct Rectangle
        {
            public Rectangle(double height, double width) : this()
            {
                Height = height;
                Width = width;
            }

            private double _height;

            public double Height
            {
                readonly get { return _height; }
                private set { _height = value; }
            }

            private double _width;

            public double Width
            {
                readonly get { return _width; }
                private set { _width = value; }
            }

            //By marking the method as readonly, no defensive copy of the method is made, improves performance.
            //Note the get properties used by the method must also be readonly.  
            //A struct is a value type, its memory will be allocated on the stack.
            //When you pass a struct to a method,  a copy of the struct will be passed.
            //A class is a reference type, its memory will be allocated on the heap.
            public readonly double Area()
            {
                return Width * Height;
            }

        }

        //Note async methods should always return Task or use FireAndForget extension method 
        public static async void AsyncIterator()
        {
            AsyncIterators asyncIterators = new AsyncIterators();
            await asyncIterators.AsyncIterator();
        }

        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }

        public static decimal CalculateToll(object vehicle, DateTime timeOfToll, bool inbound)
        {
            bool isWeekDay = timeOfToll.DayOfWeek switch
            {
                /*
                DayOfWeek.Monday => true,
                DayOfWeek.Tuesday => true,
                DayOfWeek.Wednesday => true,
                DayOfWeek.Thursday => true,
                DayOfWeek.Friday => true,
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => throw new ArgumentException(message: "Not known DayOfWeek", paramName: nameof(timeOfToll))
                */

                //Above can be simplified to this 
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true 
            };

            TimeBand timeBand = TimeBand.Overnight;
            int hours = timeOfToll.Hour;
            if(hours < 6)
            {
                timeBand = TimeBand.Overnight;
            }
            else if(hours < 10)
            {
                timeBand = TimeBand.MorningRush;
            }
            else if (hours < 16)
            {
                timeBand = TimeBand.Daytime;
            }
            else if (hours < 20)
            {
                timeBand = TimeBand.EveningRush;
            }

            var peakTimePremium = (isWeekDay, timeBand, inbound) switch
            {
                (true, TimeBand.Overnight, _) => 0.75m,
                (true, TimeBand.Daytime, _) => 1.50m,
                (true, TimeBand.MorningRush, true) => 2.50m,
                (true, TimeBand.EveningRush, false) => 2.50m,
                //All other possibilities
                (_, _, _) => 1.00m,
            };

            //Switch on vehicle type  
            var toll = vehicle switch
            {
                Car { Passengers:3 } => 1.00m,
                Car { Passengers:2  } => 1.50m,
                Car _ => 2.00m,
                Taxi _ => 4.50m,
                Bus _ => 5.50m,
                Truck _ => 7.50m,
                //If some other type {}
                { } => throw new ArgumentException(message: "Not known vehicle type", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))
            };

            return toll + peakTimePremium;
        }

        #endregion

    #region [ Yield keyword ]

    static IEnumerable<string> Filter(List<string> stringList)
        {
            foreach(string str in stringList)
            {
                if(str.Length > 3)
                {
                    Console.WriteLine($"Yield keyword method CALLED: {str}");
                    yield return str;
                }
            }
        }

        static IEnumerable<int> RunningTotal()
        {
            int total = 0;

            for(int i = 0; i < 3; i++)
            {
                total++;
                yield return total;
            }
        }

        #endregion
        }
    }

namespace VehicleRegistration
{
    public class Car
    {
        public int Passengers { get; set; }
    }

    public class Taxi
    {
        public int Fares { get; set; }
    }

    public class Bus
    {
        public int Capacity { get; set; }
        public int Riders { get; set; }
    }

    public class Truck
    {
        public int Weight { get; set; }
    }
}
