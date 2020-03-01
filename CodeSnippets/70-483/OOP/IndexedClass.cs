using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    class IndexedClass
    {
        // Create an array to store the values
        private int[] array = new int[100];

        // Declare an indexer property
        public int this[int i]
        {
            get { return array[i]; }
            set { array[i] = value; }
        }


    }
}
