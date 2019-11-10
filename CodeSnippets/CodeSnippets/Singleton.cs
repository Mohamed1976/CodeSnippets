using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CodeSnippets
{
    //https://csharpindepth.com/articles/singleton
    public sealed class Singleton
    {
        //This implementation is thread-safe because in this case instance object is initialized in the 
        //static constructor.The CLR already ensures that all static constructors are executed thread-safe.
        //Mutating instance is not a thread-safe operation, therefore the readonly 
        //attribute guarantees immutability after initialization.
        private static readonly Singleton instance = new Singleton();
        const double PI = 3.14;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Singleton()
        {
            Console.WriteLine("Static constructor is called: static Singleton()");
        }

        private Singleton()
        {
            state = 0;
            Console.WriteLine("Private constructor is called: static Singleton()");
        }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }

        public string GetDetails()
        {  
           return "Singleton details";  
        }

        private int state;

        public int GetState()
        {
            return ++state;
        }

        /// <summary>  
        /// Area of Circle is πr2.  
        /// value of pi is 3.14159 and r is the radius of the circle.  
        /// </summary>  
        /// <returns></returns>  
        public double AreaOfCircle(double radius)
        {
            return PI * (radius * radius);
        }

        /// <summary>  
        /// Area of Square is side2.  
        /// Side * Side  
        /// </summary>  
        /// <returns></returns>  
        public double AreaOfSquare(double side)
        {
            return side * side;
        }

        /// <summary>  
        /// Area of Rectangle is L*W.  
        /// L is the length of one side and W is the width of one side  
        /// </summary>  
        /// <returns></returns>  
        public double AreaOfRectangle(double length, double width)
        {
            return length * width;
        }
    }
}
