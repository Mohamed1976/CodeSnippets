using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace _70_483.Exercises
{
    public class SimpleDictionary<TKey,TValue>
    {
        private const int defaultCapacity = 100;
        private KeyValuePair<TKey, TValue>[] buffer;

        public SimpleDictionary()
        {
            Count = 0;
            Capacity = defaultCapacity;            
            buffer = new KeyValuePair<TKey, TValue>[defaultCapacity];
        }

        public SimpleDictionary(int capacity)
        {
            Count = 0;
            Capacity = capacity;
            buffer = new KeyValuePair<TKey, TValue>[capacity];
        }

        private int Capacity { get; set; }

        public int Count { get; private set; }

        public void Add(TKey key, TValue value)
        {
            //Check if key is present in the list 
            int index = Array.FindIndex(buffer, (pair) => pair.Key.Equals(key));
            if (index >= 0)
            {
                throw new ArgumentException("Key is already present in dictionary.");
            }

            //Or we can resize the array 
            if(Count >= (Capacity -1))
            {
                throw new OutOfMemoryException("Dictionary is full."); 
            }

            buffer[Count] = new KeyValuePair<TKey, TValue>(key, value);
            Count++;
        }

        public bool Remove(TKey key)
        {
            bool success = false;

            int index = Array.FindIndex(buffer, (pair) => pair.Key.Equals(key));
            if (index >= 0)
            {
                //Shrink array
                buffer[index] = buffer[Count - 1];
                Count--;
                success = true;
            }

            return success;
        }

        public TValue this[TKey key]
        {
            get
            {
                //TValue value = default;
                //We can use foreach loop or LINQ query to retrieve value corresponding to key 
                //for (int index = 0; index < Count; index++)
                //{
                //    if(buffer[index].Key.Equals(key))
                //    {
                //        value = buffer[index].Value;
                //        break;
                //    }
                //}

                //Or we can use a while loop
                //int index = 0;
                //while (index < Count && !buffer[index].Key.Equals(key)) { index++; }
                //if(index < Count)
                //{
                //    value = buffer[index].Value;
                //}

                TValue valueFound = (from pair in buffer
                                     where pair.Key.Equals(key)
                                     select pair.Value).FirstOrDefault();

                return valueFound;
            }

            set
            {
                int index = Array.FindIndex(buffer, (pair) => pair.Key.Equals(key));
                if(index >= 0)
                {
                    buffer[index] = new KeyValuePair<TKey, TValue>(key, value); 
                }
            }
        }

        public void Print()
        {
            for(int index = 0; index < Count; index++)
            {
                Console.WriteLine("{0}, {1}", buffer[index].Key, buffer[index].Value);
            }
        }
    }
}
