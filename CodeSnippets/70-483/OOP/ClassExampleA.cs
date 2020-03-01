using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    class ClassExampleA
    {
        public ClassExampleA()
        {
            Console.WriteLine("Constructor ClassExampleA() executed.");
        }

        private int value = 10;

        public class ClassExampleB : ClassExampleA
        {
            public ClassExampleB() : base() 
            {
                Console.WriteLine("Constructor ClassExampleB() executed.");
            }

            public void DisplayValue()
            {
                Console.WriteLine("Class value: {0}", value);
            }
        }
    }
}
