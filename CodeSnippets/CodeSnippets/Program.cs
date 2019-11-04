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
            {                   //Index from start      //Index from end
                "The",          //0                     //^10    
                "quick",        //1                     //^9
                "brown",        //2                     //^8
                "fox",          //3                     //^7
                "jumped",       //4                     //^6
                "over",         //5                     //^5
                "the",          //6                     //^4
                "lazy",         //7                     //^3
                "dog",          //8                     //^2
                "Ten"           //9                     //^1
            };
            var allWords = digits[..]; //Contains all the content
            var firstPhrases = digits[..4]; //Contains digits[0]..digits[3]
            var lastPhrases = digits[6..]; //Contains digits[6]..digits[9]
            Index index = ^4;
            Console.WriteLine($"{index}: {digits[index]}");
            //The last element is not included in the range
            Range range = 1..4;
            var list = digits[range];
            var _numbers = Enumerable.Range(0, 100).ToArray();
            int x = 12;
            int y = 25;
            int z = 36;
            Console.WriteLine($"{_numbers[^x]} is the same as {_numbers[_numbers.Length - x]}");
            Console.WriteLine($"{_numbers[x..y].Length} is the same as {y - x}");
            Span<int> spanXY = _numbers[x..y];
            Span<int> spanYZ = _numbers[y..z];
            Console.WriteLine($"spanXY and spanYZ are consecutive.");
            foreach (int i in spanXY)
            {
                Console.WriteLine($"spanXY: {i}"); 
            }

            foreach (int i in spanYZ)
            {
                Console.WriteLine($"spanYZ: {i}");
            }
            
            Console.WriteLine($"_numbers[x..^x] removes x elements from both ends.");
            Span<int> spanXX = _numbers[x..^x];
            Console.WriteLine($"spanXX[0] {spanXX[0]}, spanXX[^1] {spanXX[^1]}.");

            Console.WriteLine($"_numbers[..x] means _numbers[0..x] and means _numbers[x..] means _numbers[x..^0]");
            Span<int> spanX = _numbers[..x];
            Span<int> span0X = _numbers[0..x];
            Console.WriteLine($"\tspanX vs span0X: {spanX[0]}..{spanX[^1]} is the same as {span0X[0]}..{span0X[^1]}");
            Span<int> spanZ = _numbers[z..];
            Span<int> spanZEnd = _numbers[z..^0];
            Console.WriteLine($"\tspanZ vs spanZEnd: {spanZ[0]}..{spanZ[^1]} is the same as {spanZEnd[0]}..{spanZEnd[^1]}");

            //New range indices allow you to easily iterate through arrays.     
            IndicesExample();
            //You can use a static function within a static function.
            StaticLocalFunc();

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
            int? intNumber = null; 
            numbers.AddRange(new int[] { intNumber ??= 5, intNumber ??= 9, 11, 17 });
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

            //Interpolated verbatim strings, $@"" == @$"", the @ and $ order doesn't matter  

            //Using interfaces
            LoggerFactory loggerFactory = new LoggerFactory();
            var logger = loggerFactory.GetLogger(LoggerType.Database);
            logger.Log("Log message");

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

            #region [ Tuples ]

            //https://docs.microsoft.com/en-us/dotnet/csharp/tuples
            TupleExample();

            #endregion

            Console.ReadLine();
        }

        #region [ Tuple methods ]

        //To access tuple elements use Item1-Item8 properties.
        //Tuples are created using generic types Tuple<T1>-Tuple<T1, T2, T3, T4, T5, T6, T7, T8>. 
        //Each of the types represents a tuple containing 1 to 8 elements.Elements can be of different types.
        public static void TupleExample()
        {
            //Unnamed tuples, you can access the properties using [Item1..ItemX]
            var tuple = new Tuple<string, int, bool, Car>("foo", 123, true, new Car() { Passengers=6 });
            string firstProp = tuple.Item1;
            int secondProp = tuple.Item2;
            bool thirdProp = tuple.Item3;
            Car fourthProp = tuple.Item4;
            Console.WriteLine($"Tuple content: {firstProp}, {secondProp}, {thirdProp}, {fourthProp.GetType()}, {fourthProp.Passengers}   ");
            //Tuples can also be created using static Tuple.Create methods. In this case, the types of the elements 
            //are inferred by the C# Compiler.
            var tuple1 = Tuple.Create<string, int, bool, Car>("foo", 123, true, new Car() { Passengers = 6 });
            Console.WriteLine($"Tuple1 content: {tuple1.Item1}, {tuple1.Item2}, {tuple1.Item3}, {tuple1.Item4.GetType()}, {tuple1.Item4.Passengers}   ");
            //Since C# 7.0, Tuples can be easily created using ValueTuple.
            var tuple2 = ("foo", 123, true, new Car() { Passengers = 6 });
            Console.WriteLine($"Tuple2 content: {tuple2.Item1}, {tuple2.Item2}, {tuple2.Item3}, {tuple2.Item4.GetType()}, {tuple2.Item4.Passengers}   ");
            //Elements can be named for easier decomposition, named tuples
            (string str, int number, bool flag, Car instance) tuple3 = ("foo", 123, true, new Car() { Passengers = 6 });
            Console.WriteLine($"Tuple3 content: {tuple3.str}, {tuple3.number}, {tuple3.flag}, {tuple3.instance.GetType()}, {tuple3.instance.Passengers}   ");
            //Tuples can be compared based on their elements.
            //As an example, an enumerable whose elements are of type Tuple can be sorted based on comparisons operators
            //defined on a specified element:
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            list.Add(new Tuple<int, string>(6, "foo"));
            list.Add(new Tuple<int, string>(5, "bar"));
            list.Add(new Tuple<int, string>(2, "qux"));
            list.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            foreach (var element in list)
            {
                Console.WriteLine($"Sorted tuple ascending {element}");
            }

            list.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            foreach (var element in list)
            {
                Console.WriteLine($"Sorted tuple descending {element}");
            }
            //Return multiple values from a method
            var result = AddMultiply(25, 28);
            Console.WriteLine($"Tuple x + y = {result.Item1}");
            Console.WriteLine($"Tuple x * y = {result.Item2}");
            //Equality and tuples
            //Beginning with C# 7.3, tuple types support the == and != operators. These operators work by
            //comparing each member of the left argument to each member of the right argument in order.
            var left = (a: 5, b: 10);
            var right = (a: 5, b: 10);
            Console.WriteLine($"Tuples are equal {left == right}");
            double stdev = StandardDeviation(new double[] { 12.34, 6.78, 5.67, 7.89, 11.14 });
            Console.WriteLine($"Calculation stdev {string.Format("{0:0.00}", stdev)}");
        }

        //Delegate as alternative for function specification
        public static Tuple<int, int> AddMultiply(int x, int y) =>
            new Tuple<int, int>(x + y, x * y);

        //For one, the Tuple classes named their properties Item1, Item2, and so on.Those names carry no semantic information.Using these Tuple types does not enable communicating the meaning of each of the properties.The new language features enable you to declare and use semantically meaningful names for the elements in a tuple.
        //The Tuple classes cause more performance concerns because they are reference types.
        public static double StandardDeviation(IEnumerable<double> sequence)
        {
            var computation = ComputeSumAndSumOfSquares(sequence);
            //Equal calls
            //(int count, double sum, double sumOfSquares) = ComputeSumAndSumOfSquares(sequence);
            //var (sum, sumOfSquares, count) = ComputeSumAndSumOfSquares(sequence);
            //(double sum, var sumOfSquares, var count) = ComputeSumAndSumOfSquares(sequence);
            var variance = computation.SumOfSquares - computation.Sum * computation.Sum / computation.Count;
            return Math.Sqrt(variance / computation.Count);
        }

        //You can remove the field names from the return value declaration and return an unnamed tuple.
        //The fields of this return tuple are named Item1, Item2, and Item3. 
        //It's recommended that you provide semantic names to the elements of tuples returned from methods.
        //private static (int, double, double) ComputeSumAndSumOfSquares(IEnumerable<double> sequence)
        private static (int Count, double Sum, double SumOfSquares) ComputeSumAndSumOfSquares(IEnumerable<double> sequence)
        {
            double sum = 0;
            double sumOfSquares = 0;
            int count = 0;

            foreach (var item in sequence)
            {
                count++;
                sum += item;
                sumOfSquares += item * item;
            }

            return (count, sum, sumOfSquares);
        }

        #endregion

        #region [ What is new in C# 8.0 ]

        public static void IndicesExample()
        {
            int[] sequence = Enumerable.Range(0, 1000).Select(x => (int)(Math.Sqrt(x) * 100)).ToArray(); 
            
            for(int i = 0; i < sequence.Length; i += 100)
            {
                Range range = i..(i + 10);
                var (min, max, average) = MovingAverage(sequence, range);
                Console.WriteLine($"Forward: Range.Start[{range.Start}], " +
                    $"Range.End[{range.End}] Calculated (min[{min}], max[{max}], average[{average}])  ");
            }

            for (int i = 0; i < sequence.Length; i += 100)
            {
                Range range = ^(i + 10)..^i;
                var (min, max, average) = MovingAverage(sequence, range);
                Console.WriteLine($"Backward: Range.Start[{range.Start}], " +
                    $"Range.End[{range.End}] Calculated (min[{min}], max[{max}], average[{average}])  ");
            }

            //Local lambda function to calculate moving average, returning Tuple. 
            (int min, int max, double average) MovingAverage(int[] sequence, Range range) =>
                (
                    sequence[range].Min(),
                    sequence[range].Max(),
                    sequence[range].Average()
                );
        }

        public static void StaticLocalFunc()
        {
            foreach(int i in Counter(0, 10))
            {
                Console.WriteLine($"StaticLocalFunc() i:[{i}]");
            }
        }

        //Local functions were added in c# 7.0
        //Static local function were added in c# 8.0
        //By using a static local function, you know that the local static function 
        //is not using variables in its outer scope, the Just-In-Time (JIT) compiler can then make some optimizations     
        public static IEnumerable<int> Counter(int start, int end)
        {
            if (start > end)
                throw new ArgumentOutOfRangeException(nameof(start), "Start index should be less than end index.");

            return LocalCounter(start, end);

            static IEnumerable<int> LocalCounter(int start, int end)
            {
                for (int i = start; i < end; i++)
                {
                    Console.WriteLine($"yield return i:[{i}]");
                    yield return i;
                }                
            }
        }


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
