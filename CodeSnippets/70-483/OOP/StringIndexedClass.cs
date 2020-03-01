using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    class StringIndexedClass
    {
        // Create an array to store the values
        private int[] array = new int[100];

        public int this[string name]
        {
            get
            {
                switch (name)
                {
                    case "zero":
                        return array[0];
                    case "one":
                        return array[1];
                    default:
                        return -1;
                }
            }
            set
            {
                switch (name)
                {
                    case "zero":
                        array[0] = value;
                        break;
                    case "one":
                        array[1] = value;
                        break;
                }
            }
        }

    }
}
