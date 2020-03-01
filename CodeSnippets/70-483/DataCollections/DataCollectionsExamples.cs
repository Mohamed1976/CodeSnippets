using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;

//There are two things that are important when dealing
//with collections.The first is that extending a parent collection type is a great
//way to add custom behaviors.The second is that you should remember that it is
//possible to perform LINQ queries on collections, which can save you a lot of
//work writing code to search through them and select items.

namespace _70_483.DataCollections
{
    public class DataCollectionsExamples
    {
        public DataCollectionsExamples()
        {

        }

        const string gutenbergUrl = @"http://www.gutenberg.org/files/54700/54700-0.txt";

        private Dictionary<string, int> dictionary = new Dictionary<string, int>()
        {
            { "Accounting", 1 },
            { "Marketing", 2 },
            { "Operations", 3 }
        };

        private bool? FindInList(string searchTerm, int value)
        {
            return dictionary.Contains(new KeyValuePair<string, int>(searchTerm, value));
        }

        //The array class does not provide any methods that can add or remove elements.
        //The size of an array is fixed when the array is created.The only way to modify
        //the size of an existing array is to create a new array of the required type and then
        //copy the elements from one to the other.The array class provides a CopyTo
        //method that will copy the contents of an array into another array.The first
        //parameter of CopyTo is the destination array. The second parameter is the start
        //position in the destination array for the copied values. The method below shows how
        //this can be used to migrate an array into a larger one.The new array has a new size
        //Returns resized array of type T 
        private T [] ResizeArray<T>(T[] source, int newSize)
        {
            if (source.Length > newSize)
                throw new ArgumentOutOfRangeException("New array size is smaller than source array size.");

            T[] newArray = new T[newSize];

            source.CopyTo(newArray, 0);
            return newArray;
        }

        private static void Display(IEnumerable<Person> collection)
        {
            foreach (Person p in collection)
                Console.WriteLine("Linq result: " + p.firstName + " " + p.lastName);

        }

        public async Task Run()
        {
            string [] animals = new string[] { "cat", "dog", "bird", "Cheetah" };
            //IEnumerable is an interface defining a single method GetEnumerator() that returns an 
            //IEnumerator interface. It is the base interface for all non-generic collections that can be enumerated.
            /*
            public interface IEnunmerable
            {
                IEnumerator GetEnumerator();
            }
           
            public interface IEnumerator
            {
                bool MoveNext();
                object Current { get; }
                void Reset();
            } 
            */
            WordDictionary wordDictionary = new WordDictionary(animals);
            foreach(string word in wordDictionary)
            {
                Console.WriteLine(word);
            }

            //Note, that if you want the new collection type to be used with LINQ queries it
            //must implement the IEnumerable<type> interface. This means that the type
            //must contain a GetEnumerator<string>() method.
            var wordsFound = from word in wordDictionary
                               where word == "cat"
                               select word;

            foreach(string word in wordsFound)
                Console.WriteLine("Linq result: {0}", word);

            Person[] peopleArray = new Person[3]
            {
                new Person("John", "Smith"),
                new Person("Jim", "Johnson"),
                new Person("Sue", "Rabon"),
            };

            People peopleList = new People(peopleArray);
            foreach (Person p in peopleList)
                Console.WriteLine(p.firstName + " " + p.lastName);

            IEnumerator<Person> PersonList = peopleList.GetEnumerator();
            while(PersonList.MoveNext())
            {
                Console.WriteLine(PersonList.Current.firstName + " " + PersonList.Current.lastName);
            }

            //Note, that if you want the new collection type to be used with LINQ queries it
            //must implement the IEnumerable<type> interface. This means that the type
            //must contain a GetEnumerator<string>() method.
            var selection = from person in peopleList
                            where person.firstName == "John"
                            select person;

            Display(selection);
            
            //The IEnumerable interface allows you to create objects that can be
            //enumerated within your programs, for example by the foreach loop construction.
            //Collection classes, and results returned by LINQ queries implement this interface.
            //It creates a class called EnumeratorObject that implements both the IEnumerable interface
            //(meaning it can be enumerated) and the IEenumerator<int> interface
            //(meaning that it contains a call of GetEnumerator to get an enumerator from
            //it). An EnumeratorObject instance performs an iteration up to a limit that
            //was set when it was created. Note that the EnumeratorObject class contains the
            //Current property and the MoveNext behavior.
            EnumeratorObject enumeratorObject = new EnumeratorObject(20);
            //You can use an EnumeratorObject instance in a foreach loop:
            foreach (int item in enumeratorObject)
            {
                Console.WriteLine(item);
            }

            //TODO
            //IEnumerator<int> results = enumeratorObject.GetEnumerator();
            //var results = from item in enumeratorObject
            //              where item > 10 && item < 13
            //              select item;
            //foreach (int item in results)
            //{
            //    Console.WriteLine(item);
            //}

            //To make it easier to create iterators C# includes the yield keyword.
            //The keyword yield is followed by the return keyword and precedes the value to be returned for the
            //current iteration.The C# compiler generates all the Current and MoveNext
            //behaviors that make the iteration work, and also records the state of the iterator
            //method so that the iterator method resumes at the statement following the
            //yield statement when the next iteration is requested.
            EnumeratorObjectV2 enumeratorObjectV2 = new EnumeratorObjectV2(10);
            foreach(var item in enumeratorObjectV2)
            {
                Console.WriteLine(item);
            }

            //The yield keyword does two things. It specifies the value to be returned for
            //a given iteration, and it also returns control to the iterating method.You can
            //express an iterator that returns the values 1, 2, 3, as follows.
            //When the first yield is reached the enumerator returns the value 1.The next
            //time that the enumerator is called(in other words the next time round the loop)
            //the enumerator resumes at the statement following the first yield.This is
            //another yield that returns 2.This continues, with the value 3 being returned by
            //the third yield.When the enumerator method ends this has the effect of ending the loop.
            /*
            public IEnumerator<int> GetEnumerator()
            {
                yield return 1;
                yield return 2;
                yield return 3;
            }
            */

            //Programs spend a lot of time consuming lists and other collections of items.This
            //is called iterating or enumerating. Any C# object can implement the
            //IEnumerable interface that allows other programs to get an enumerator from
            //that object. The enumerator object can then be used to enumerate(or iterate) on the object.
            //The string type supports enumeration, and so a program can call the GetEnumerator method on a
            //string instance to get an enumerator. The enumerator exposes the method
            //MoveNext(), which returns the value true if it was able to move onto
            //another item in the enumeration. The enumerator also exposes a property called
            //Current, which is a reference to the currently selected item in the enumerator.
            // Get an enumerator that can iterate through a string
            IEnumerator stringEnumerator = "Hello world".GetEnumerator();
            while(stringEnumerator.MoveNext())
            {
                Console.WriteLine(stringEnumerator.Current);
            }

            //C# makes life easier by providing the foreach
            //construction, which automatically gets the enumerator from the object and the works through it.
            foreach(char ch in "Hello world")
            {
                Console.WriteLine(ch);
            }
            
            //The behavior of a collection type is expressed by the ICollection interface.
            //The ICollection interface is a child of the IEnumerator interface.
            //Interface hierarchies work in exactly the same way as class hierarchies, in that a
            //child of a parent interface contains all of the methods that are described in the
            //parent.This means that a type that implements the ICollection interface is
            //capable of being enumerated.
            CompassCollection compassCollection = new CompassCollection();
            foreach(var item in compassCollection) //Iterates through the compass array
            {
                Console.WriteLine(item);
            }
            
            string[] directions = new string[10];
            compassCollection.CopyTo(directions, 5);

            foreach(string dir in directions)
            {
                if (string.IsNullOrEmpty(dir))
                    Console.WriteLine("-");
                else
                    Console.WriteLine(dir);
            }

            //A custom collection is a collection that you create for a specific purpose that has
            //behaviors that you need in your application.One way to make a custom
            //collection is to create a new type that implements the ICollection interface.
            //This can then be used in the same way as any other collection, such as with a
            //program iterating through your collection using a foreach construction.We
            //will describe how to implement a collection interface in the next section.
            //Another way to create a custom collection is to use an existing collection class
            //as the base (parent) class of a new collection type.You can then add new
            //behaviors to your new collection and, because it is based on an existing
            //collection type, your collection can be used in the same way as any other collection.
            TrackStore tracks = TrackStore.GetTestTrackStore();
            Console.WriteLine(tracks + "\n");
            int removed = tracks.RemoveArtist("Immy Brown");
            Console.WriteLine("Records removed: " + removed + "\n" + tracks);

            //List: Easy to remove a value from the middle of a list or insert an extra value.
            //ArrayList: Does not provide type safety.
            //array: Quick access, store two-dimensional data 
            //dictionary: Store objects that are indexed on unique value for example (account number, bank object) 
            //A dictionary is less useful if you need to locate a
            //data value based on different elements, such as needing to find a customer based
            //on their customer ID, name, or address. In that case, put the data in a List and
            //then use LINQ queries on the list to locate items.
            //Sets can be useful when working with tags. Their built-in operations are much
            //easier to use than writing your own code to match elements together.Queues and
            //stacks are used when the needs of the application require FIFO or LIFO behavior.
            //The array class does not provide any methods that can add or remove elements.
            //The size of an array is fixed when the array is created.The only way to modify
            //the size of an existing array is to create a new array of the required type and then
            //copy the elements from one to the other.The array class provides a CopyTo
            //method that will copy the contents of an array into another array.
            int[] dataArray = { 1, 2, 3, 4, 2020 };
            int[] result = ResizeArray<int>(dataArray, 20);
            Console.WriteLine("Result array.Length: {0}", result.Length);
            foreach(int val in result)
            {
                Console.WriteLine(val);
            }

            //A queue provides a short term storage for data items. It is organized as a first - infirst -
            //out (FIFO)collection.Items can be added to the queue using the Enqueue
            //method and read from the queue using the Dequeue method.There is also a
            //Peek method that allows a program to look at an item at the top of the queue
            //without removing it from the queue.A program can iterate through the items in a
            //queue and a queue also provides a Count property that will give the number of items in the queue.
            //One potential use of a queue is for passing work items from
            //one thread to another. If you are going to do this you should take a look at the ConcurrentQueue
            Queue<string> demoQueue = new Queue<string>();
            demoQueue.Enqueue("Rob Miles");
            demoQueue.Enqueue("Immy Brown");
            Console.WriteLine("Peek: {0}", demoQueue.Peek());
            Console.WriteLine("Queue Dequeue: {0}", demoQueue.Dequeue());

            //A stack is very similar in use to a queue. The most important difference is that a
            //stack is organized as last -in-first -out (LIFO).A program can use the Push
            //method to push items onto the top of the stack and the Pop method to remove
            //items from the stack. There is a ConcurrentStack
            //implementation that should be used if different Tasks are using the same stack.
            Stack<string> demoStack = new Stack<string>();
            demoStack.Push("Rob Miles");
            demoStack.Push("Immy Brown");
            Console.WriteLine("Peek: {0}", demoStack.Peek());
            Console.WriteLine("Stack Pop: {0}", demoStack.Pop());

            //ArrayList stores data in a dynamic structure that grows as more items are added to it.
            //The items in an ArrayList can be accessed with a subscript in
            //exactly the same way as elements in an array. The ArrayList provides a
            //Count property that can be used to count how many items are present.
            //The ArrayList provides an Add method that adds items to the end of the
            //list.There is also an Insert method that can be used to insert items in the list
            //and a Remove method that removes items. Items in an ArrayList are managed by reference 
            //and the reference that is used is of type object.This means that an ArrayList can hold references to
            //any type of object, since the object type is the base type of all of the types in C#.
            //Note that if you are confused to see the value type int being used in an ArrayList, the int value is boxed. 
            ArrayList arrayListInit = new ArrayList { 1, "Rob Miles", new ArrayList() { "Alfa",  "Beta"} };
            //check-the-type-of-object-in-arraylist
            //Another difficultly caused by the untyped storage provided by the
            //ArrayList is that all of the references in the list are references to objects.
            //When a program removes an item from an ArrayList it must cast the item
            //into its proper type before it can be used.
            if (arrayListInit[0].GetType() == typeof(int))
            {
                Console.WriteLine("typeof(int): {0}, is int: {1}", (int)arrayListInit[0], arrayListInit[0] is int);
            }

            if (arrayListInit[1].GetType() == typeof(string))
            {
                //Console.WriteLine("typeof(string): {0}, is string: {1}", (string)arrayListInit[1], arrayListInit[1] is string);
                Console.WriteLine("typeof(string): {0}, is string: {1}", arrayListInit[1] as string, arrayListInit[1] is string);
            }

            if (arrayListInit[2].GetType() == typeof(ArrayList))
            {
                //as below returns null when casting is not possible 
                ArrayList arrayList = arrayListInit[2] as ArrayList;
                Console.WriteLine("typeof(ArrayList): {0}, {1}, is ArrayList: {2}", 
                    arrayList[0], arrayList[1], arrayListInit[2] is ArrayList);
            }

            List<int> lst = new List<int>() { 1, 2, 3, 4 };
            Dictionary<int, string> dic2 = new Dictionary<int, string>()
            {
                {1, "Rob" },
                {2, "Immy" }
            };

            HashSet<string> setInit = new HashSet<string>() { "Electronic", "Disco", "Fast" };

            Queue<string> queueInit = new Queue<string>(new string[] { "Rob", "Immy" });

            Stack<string> stackInit = new Stack<string>(new string[] { "Rob", "Immy" });

            //The List type makes use of the “generics” features of C#.
            //Only references of the specified type can be added to the list,
            //and values obtained from the list are of the specified type.
            //The List type implements the IEnumerable, ICollection and IList interfaces.
            List<string> names = new List<string>();
            names.Add("Rob Miles");
            names.Add("Immy Brown");
            for (int i = 0; i < names.Count; i++)
                Console.WriteLine(names[i]);
            names[0] = "Fred Bloggs";
            foreach (string name in names)
                Console.WriteLine(name);

            List<string> list = new List<string>();
            list.Add("add to end of list"); // add to the end of the list
            list.Insert(0, "insert at start"); // insert an item at the start
            list.Insert(1, "insert new item 1"); // insert at position
            list.InsertRange(2, new string[] { "Rob", "Immy" }); // insert a range
            list.Remove("Rob"); // remove first occurrence of "Rob"
            list.RemoveAt(0); // remove element at the start
            list.RemoveRange(1, 2); // remove two elements
            list.Clear(); // clear entire list

            Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
            dictionary1.Add(1, "Rob Miles"); // add an entry
            dictionary1.Remove(1); // remove the entry with the given key

            //The Set type provides Add, Remove and RemoveWhere methods. 
            //The RemoveWhere function is given a predicate
            //(a behavior that generates either true or false) to determine which elements are to
            //be removed. In the listing the predicate is a lambda expression that evaluates to
            //true if the element in the set starts with the character ‘R.’
            HashSet<string> hashSet = new HashSet<string>();
            hashSet.Add("Rob Miles"); // add an item
            //hashSet.Remove("Rob Miles"); // remove an item
            hashSet.RemoveWhere(name => name.StartsWith('R'));

            //HashSet class in c# can be used to implement sets.
            //A set is an unordered collection of items.Each of the items in a set will be
            //present only once.You can use a set to contain tags or attributes that might be
            //applied to a data item. For example, information about a MusicTrack can
            //include a set of style attributes. A track can be “Electronic,” “Disco,” and “Fast.”
            //Another track can be “Orchestral,” “Classical,” and “Fast.” A given track is
            //associated with a set that contains all of the attributes that apply to it. A music
            //application can use set operations to select all of the music that meets particular
            //criteria, for example you can find tracks that are both “Electronic” and “Disco.”
            HashSet<string> track1Styles = new HashSet<string>();
            track1Styles.Add("Electronic");
            track1Styles.Add("Disco");
            track1Styles.Add("Fast");

            HashSet<string> track2Styles = new HashSet<string>();
            track2Styles.Add("Orchestral");
            track2Styles.Add("Classical");
            track2Styles.Add("Fast");

            //The HashSet class provides methods that implement set operations.The IsSubSetOf
            //method returns true if the given set is a subset of another.
            HashSet<string> search = new HashSet<string>();
            search.Add("Fast");
            search.Add("Disco");

            //Another set methods can be used to combine set values to produce unions,
            //differences, and to test supersets and subsets.
            if (search.IsSubsetOf(track1Styles))
                Console.WriteLine("All search styles present in track1Styles");

            if (search.IsSubsetOf(track2Styles))
                Console.WriteLine("All search styles present in track2Styles");
            
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/multidimensional-arrays
            //Any array that uses a single index to access the elements in the array is called
            //a one dimensional array.It is analogous to a list of items. Arrays can have more
            //than one dimension. An array with two dimensions is analogous to a table of data that is made up of
            //rows and columns. An array with three dimensions is analogous to a book
            //containing a number of pages, with a table on each page.
            //Note the use of the comma between the brackets in the declaration
            //of the array variable. This denotes that the array has multiple dimensions.
            string[,] compass = new string[3, 3] 
            {
                { "NW","N","NE" },
                {"W", "C", "E" },
                { "SW", "S", "SE" }
            };

            //string[,] compass = new string[NrOfRows, NrOfColumns]
            Console.WriteLine(compass[0,1]); //Row=0, Column=1, returns "N"
            Console.WriteLine(compass[2, 0]); //Row=0, Column=1, returns "SW"
            Console.WriteLine("Nr of Rows: {0}, Nr of Columns: {1}", compass.GetLength(0), compass.GetLength(1));

            string[,] array2d = new string[,] //new string[3,2]
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" }
            };

            Console.WriteLine("Nr of Rows: {0}, Nr of Columns: {1}", array2d.GetLength(0), array2d.GetLength(1));

            //Jagged arrays You can view a two - dimensional array as an array of one dimensional arrays.A
            //“jagged array” is a two - dimensional array in which each of the rows are a different length.
            int[][] arrayOfArrays = new int[][]
            {
                new int[] {1,2,3,4 },
                new int[] {5,6,7},
                new int[] {11,12}
            };

            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/jagged-arrays
            Console.WriteLine("Jagged array length: {0}", arrayOfArrays.Length); //Should return 3
            //We can iterate through all the arrays
            for(int index = 0; index < arrayOfArrays.Length; index++)
            {
                //Iterate through each array
                for(int arrayIndex = 0; arrayIndex < arrayOfArrays[index].Length; arrayIndex++)
                {
                    System.Console.Write("{0}{1}", arrayOfArrays[index][arrayIndex],
                        arrayIndex == (arrayOfArrays[index].Length - 1) ? "\n" : " ");
                }                
            }

            //An array is the simplest way to create a collection of items of a particular type.
            //An array is assigned a size when it is created and the elements in the array are
            //accessed using an index or subscript value.An array instance is a C# object that
            //is managed by reference.
            //An array of value types(for example an array of integers) holds the values
            //themselves within the array, whereas for an array of reference types(for example
            //an array of objects) each element in the array holds a reference to the object.
            //When an array is created, each element in the array is initialized to the default
            //value for that type. Numeric elements are initialized to 0, reference elements to
            //null, and Boolean elements to false.
            int[] intArray = null; //Array pointer 
            intArray = new int[] { 4,5,6,7}; //Create array with four digits
            
            //Arrays implement the IEnumerable interface, so they can be enumerated
            //using the foreach construction.
            foreach(int digit in intArray)
            {
                Console.WriteLine("{0}", digit);
            }

            //Once created, an array has a fixed length that cannot be changed, but an array
            //reference can be made to refer to a new array object.An array can be initialized
            //when it is created.An array provides a Length property that a program can use
            //to determine the length of the array. You can assigne a new array to array variable/pointer
            intArray = new int[10];
            for(int index = 0; index < intArray.Length; index++)
            {
                intArray[index] = index;
            }
            
            foreach (int digit in intArray)
            {
                Console.WriteLine("{0}", digit);
            }

            //A Dictionary allows you to access data using a key.
            BankAccount a1 = new BankAccount { AccountNo = 1, Name = "Rob Miles" };
            BankAccount a2 = new BankAccount { AccountNo = 2, Name = "Immy Brown" };
            Dictionary<int, BankAccount> accounts = new Dictionary<int, BankAccount>();
            accounts.Add(a1.AccountNo, a1);
            accounts.Add(a2.AccountNo, a2);
            Console.WriteLine(accounts[1]); //Access BankAccount using Key value, NOTE NOT INDEX AS WITH ARRAYS   
            if (accounts.ContainsKey(2))
                Console.WriteLine("Account located");

            try
            {
                //We get an exception when we try to add a key value pair that already exists  
                dictionary.Add("Accounting", 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while adding key to dictionary: {0}.", ex.Message);
            }
            
            bool? isInList = FindInList("Finance", 0);
            Console.WriteLine("Dictionary(Finance, 0) is in list: [{0}]", isInList == null ? "null" : isInList.ToString());
            isInList = FindInList("Accounting", 2);
            Console.WriteLine("Dictionary(Accounting, 2) is in list: [{0}]", isInList == null ? "null" : isInList.ToString());
            isInList = FindInList("Accounting", 1);
            Console.WriteLine("Dictionary(Accounting, 1) is in list: [{0}]", isInList == null ? "null" : isInList.ToString());

            //Example on how to count words occurring in a text.
            //The best way to do this is to use a dictionary of integers indexed on a string.
            //The integer in the dictionary holds the count of that word, and the index is
            //the word itself.When the document is being processed, each word is
            //isolated in turn.If the word is not present in the dictionary, it is added to the
            //dictionary, and the count for that word is set to 1.If the word is present in
            //the dictionary, the count for that word is incremented.
            //https://docs.microsoft.com/vi-vn/dotnet/standard/parallel-programming/how-to-use-parallel-invoke-to-execute-parallel-operations
            // Retrieve Goncharov's "Oblomov" from Gutenberg.org.
            //string[] words = await CreateWordArray(gutenbergUrl);
            string[] words = exampleText.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
            //Arrays implement the IEnumerable interface and hence can be iterated using foreach loop  
            //int index = 0;
            //foreach(string word in words)
            //{
            //    Console.WriteLine("[{0}], {1}",index++, word);
            //}
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (string word in words)
            {
                string wordLower = word.ToLower();
                //Check if Dictionary contains the current word.  
                if (dic.ContainsKey(wordLower))
                { //Dictionary contains the current word, increment counter.
                    dic[wordLower]++;
                }
                else
                { ////Dictionary does not contain the current word, add it to dictionary
                    dic.Add(wordLower, 1);
                }
            }

            //List and array instances provide a Sort method that can be used to sort their
            //contents.Unfortunately, the Dictionary class does not provide a sort
            //behavior.However, you can use a LINQ query on a dictionary to produce a
            //sorted iteration of the dictionary contents.This can be used by a foreach loop
            //to generate sorted output. Order the dictionary by word occurence
            var orderedDictionary = from keyValuePair in dic
                                   orderby keyValuePair.Value descending
                                   select keyValuePair;

            foreach (KeyValuePair<string,int> keyValue in orderedDictionary)
            {
                Console.WriteLine("{0}, {1}", keyValue.Key, keyValue.Value);
            }
        }

        private async Task<string[]> CreateWordArray(string url)
        {
            string[] wordList = null;

            using (WebClient webClient = new WebClient())
            {
                string content = await webClient.DownloadStringTaskAsync(url);
                // Separate string into an array of words, removing some common punctuation.
                wordList = content.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '_', '/' },
                    StringSplitOptions.RemoveEmptyEntries);
            }

            return wordList;
        }

        const string exampleText = "Here, you would like the word counter to produce a sorted list of word counts." +
        "List and array instances provide a Sort method that can be used to sort their " +
        "contents.Unfortunately, the Dictionary class does not provide a sort " +
        "behavior.However, you can use a LINQ query on a dictionary to produce a " +
        "sorted iteration of the dictionary contents.This can be used by a foreach loop " +
        "to generate sorted output.The code to do this is shown next. It requires careful " +
        "study. Items in a Dictionary have Key and Value properties that are used " +
        "for sorting and output.When trying the code on an early version of this text I " +
        "found that the word “the” was used around twice as many times as the next most " +
        "popular word, which was a.";
    }
}
