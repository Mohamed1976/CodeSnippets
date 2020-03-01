using System;
using System.Collections.Generic;
using System.Text;

//The following example defines an abstract base class named Shape that defines two properties: Area and Perimeter.
//In addition to marking the class with the abstract keyword, each instance member is also marked with the abstract keyword.
//In this case, Shape also overrides the Object.ToString method to return the name of the type, rather than its fully 
//qualified name. And it defines two static members, GetArea and GetPerimeter, that allow callers to easily retrieve the 
//area and perimeter of an instance of any derived class. When you pass an instance of a derived class to either of these 
//methods, the runtime calls the method override of the derived class
namespace _70_483.OOP
{
    public abstract class Shape
    {
        public static double GetArea(Shape shape) => shape.Area;

        public static double GetPerimeter(Shape shape) => shape.Perimeter;

        public abstract double Area { get; }

        public abstract double Perimeter { get; }

        public override string ToString() => GetType().Name;

    }

    //You can then derive some classes from Shape that represent specific shapes. The following example defines 
    //three classes, Triangle, Rectangle, and Circle. Each uses a formula unique for that particular shape to 
    //compute the area and perimeter. Some of the derived classes also define properties, such as Rectangle.
    //Diagonal and Circle.Diameter, that are unique to the shape that they represent.
    public class Square : Shape
    {
        public Square(double length)
        {
            Side = length;
        }

        public double Side { get; }

        public override double Area => Math.Pow(Side, 2);

        public override double Perimeter => Side * 4;

        public double Diagonal => Math.Round(Math.Sqrt(2) * Side, 2);
    }

    public class Rectangle : Shape
    {
        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public double Length { get; }

        public double Width { get; }

        public override double Area => Length * Width;

        public override double Perimeter => 2 * Length + 2 * Width;

        public bool IsSquare() => Length == Width;

        public double Diagonal => Math.Round(Math.Sqrt(Math.Pow(Length, 2) + Math.Pow(Width, 2)), 2);
    }

    public class Circle : Shape
    {
        public Circle(double radius)
        {
            Radius = radius;
        }

        public override double Area => Math.Round(Math.PI * Math.Pow(Radius, 2), 2);

        public override double Perimeter => Math.Round(Math.PI * 2 * Radius, 2);

        // Define a circumference, since it's the more familiar term.
        public double Circumference => Perimeter;

        public double Radius { get; }

        public double Diameter => Radius * 2;
    }
}
