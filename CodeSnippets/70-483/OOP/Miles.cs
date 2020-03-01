using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    class Miles
    {
        //Immutable 
        public Miles(double miles)
        {
            Distance = miles;
        }

        public double Distance { get; }

        public static explicit operator int(Miles t)
        {
            Console.WriteLine("Explicit conversion from miles to int");
            return (int)(t.Distance + 0.5);
        }

        // Conversion operator for implicit converstion to Kilometers
        public static implicit operator Kilometers(Miles t)
        {
            Console.WriteLine("Implicit conversion from miles to kilometers");
            return new Kilometers(t.Distance * 1.6);
        }
    }

    class Kilometers
    {
        public double Distance { get; }
        public Kilometers(double kilometers)
        {
            Distance = kilometers;
        }

        public override string ToString()
        {
            return string.Format("Distance: {0}", Distance.ToString("N2"));
        }
    }
}
