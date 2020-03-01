using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace _70_483.DataCollections
{
    public class CompassCollection : ICollection //Implements ICollection interface 
    {
        // Array containing values in this collection
        string[] compassPoints = { "North", "South", "East", "West" };

        // Count property to return the length of the collection
        public int Count => compassPoints.Length;

        // Returns true if the collection is thread safe
        // This collection is not
        public bool IsSynchronized => false;

        // Returns an object that can be used to synchronise
        // access to this object
        //TODO should this not be: private object _syncObject = new object(); 
        public object SyncRoot => this; //throw new NotImplementedException();

        public void CopyTo(Array array, int index)
        {
            //We can also use foreach loop
            foreach(string direction in compassPoints)
            {
                array.SetValue(direction, index);
                index++;
            }

            //You can also simply use CopyTo as shown below 
            //compassPoints.CopyTo(array, index);
        }

        //Note, that if you want the new collection type to be used with LINQ queries it
        //must implement the IEnumerable<type> interface. This means that the type
        //must contain a GetEnumerator<string>() method.
        public IEnumerator GetEnumerator()
        {
            return compassPoints.GetEnumerator();
        }
    }
}
