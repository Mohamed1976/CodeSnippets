using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConsoleWithDI.Exercises
{
    public delegate double Function(double x);

    public class Math
    {

        //The graph of y = ax ^ 2 + bx + c(Algebra 1, Quadratic equations...

        /* Example of using Closures */
        public Function GetQuadraticFunction(double ax2, double bx, double c)
        {
            return (double x) =>
            {
                return ax2 * System.Math.Pow(x, 2) + bx * x + c;
            };
        }

        public Func<double,double> GetQuadraticFunctionV2(double ax2, double bx, double c)
        {
            return (double x) =>
            {
                return ax2 * System.Math.Pow(x, 2) + bx * x + c;
            };
        }


        /* Example of using Closures */
        public Function GetLinearFunction(double ax, double b)
        {
            Function linearFunction = (double x) =>
            {
                return ax * x + b;
            };

            return linearFunction;
        }

        public Func<double,double> GetLinearFunctionV2(double ax, double b)
        {
            Func<double, double> linearFunction = (double x) =>
            {
                return ax * x + b;
            };

            return linearFunction;
        }


        //double format specifiers, for example "0.00" 
        //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings
        public void PlotFunction(Function function, double startPosX, double endPosX, 
            double step, string format = "0.00")
        {
            double Y; 

            while(startPosX < endPosX)
            {
                Y = function(startPosX);

                Console.WriteLine($"{startPosX.ToString(format, CultureInfo.InvariantCulture)}, " +
                    $"{Y.ToString(format, CultureInfo.InvariantCulture)}");

                startPosX += step;
            }
        }

        public void PlotFunctionV2(Func<double,double> function, double startPosX, double endPosX,
            double step, string format = "0.00")
        {
            double Y;

            while (startPosX < endPosX)
            {
                Y = function(startPosX);

                Console.WriteLine($"{startPosX.ToString(format, CultureInfo.InvariantCulture)}, " +
                    $"{Y.ToString(format, CultureInfo.InvariantCulture)}");

                startPosX += step;
            }
        }
    }
}
