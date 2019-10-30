using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeSnippets
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [ Example01 ]

            //C# 8.0 language
            //.Net Core 3.0 
            //readonly method
            Rectangle rectangle = new Rectangle(10,10);
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

        #endregion
    }
}
