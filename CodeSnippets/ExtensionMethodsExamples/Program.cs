using ExtensionMethodsExamples.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethodsExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [ Example 1]

            //Extension methods extend and add behavior to existing types.
            //An extension method is created by adding a static method to a static class 
            //which is distinct from the original type being extended.The static class 
            //holding the extension method is often created for the sole purpose of holding extension methods.
            //This first parameter is decorated with the keyword this.
            //This calls method String.ToUpper()
            var message = "Hello World.".ToUpper();
            // This calls the extension method StringExtensions.Shorten()
            message.Shorten(5).Print();
            message.Excite().Print();
            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };
            // You need to include the System.Linq namespace in order to use Where extension method. 
            //IEnumerable<int> results = ints.Where(x => x > 4);
            int[] results = ints.Where(x => x > 4).ToArray();
            foreach (int i in results)
            {
                Console.WriteLine(i);
            }
            //Create person object and fill properties
            Person person = new Person();
            person.Fill().Print();
            double number = 3;
            Console.WriteLine(number.Add(7).Subtract(5).Multiply(4).DivideBy(2));

            #endregion

            Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        const char period = '.';
        const char exclamtionPoint = '!'; 

        public static string Shorten(this string text, int length)
        {
            return text.Substring(0, length);
        }

        public static void Print(this string text)
        {
            Console.WriteLine(text);
        }

        public static string Excite(this string text)
        {
            return text.Replace(period, exclamtionPoint);
        }
    }

    public static class DoubleExtensions
    {
        public static double Add(this double value, double newValue)
        {
            return value + newValue;
        }
        public static double Subtract(this double value, double newValue)
        {
            return value - newValue;
        }

        public static double Multiply(this double value, double newValue)
        {
            return value * newValue;
        }

        public static double DivideBy(this double value, double newValue)
        {
            return value / newValue;
        }
    }
}
