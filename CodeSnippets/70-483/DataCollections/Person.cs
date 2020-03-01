using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=netframework-4.8
namespace _70_483.DataCollections
{
    public class Person
    {
        public Person(string fName, string lName)
        {
            this.firstName = fName;
            this.lastName = lName;
        }

        public string firstName;
        public string lastName;
    }

    // Collection of Person objects. This class
    // implements IEnumerable so that it can be used
    // with ForEach syntax.
    public class People : IEnumerable<Person> //IEnumerable, 
    {
        private Person[] _people = null;

        public People(Person[] pArray)
        {
            _people = new Person[pArray.Length];
            //Copy references to local array
            pArray.CopyTo(_people, 0);
        }

        public IEnumerator<Person> GetEnumerator()
        {
            return new PeopleEnumerator(_people);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class PeopleEnumerator : IEnumerator<Person> //IEnumerator,
    {
        public Person[] _people;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PeopleEnumerator(Person[] list)
        {
            _people = list;
        }


        //public Person Current => throw new NotImplementedException();
        public Person Current
        {
            get
            {
                try
                {
                    return _people[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            position++;
            return (position < _people.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
