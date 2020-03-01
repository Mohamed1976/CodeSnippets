using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace _70_483.Validation
{
    class ValidationExamples
    {
        public ValidationExamples()
        {
                
        }

        public void Run()
        {
            //In order to use JSON, you need to install Newtonsoft.JSON 
            //If you want to serialize a class using JSON you don’t have to add the [Serializable] attribute to the class.
            //JSON, or JavaScript Object Notation, is a very popular means by which applications can exchange data.
            //A JSON document contains a number of name/value pairs that represent the data
            //in an application. A JSON document can also contain arrays of JSON objects.
            //JSON documents map very well onto objects in an object-oriented language,
            //although JSON itself is not tied to any one programming language.JSON is
            //therefore very useful if you want to share data between programs written in different languages.
            MusicTrack track = new MusicTrack(artist: "Rob Miles", title: "My Way", length: 150);
            string json = JsonConvert.SerializeObject(track);
            Console.WriteLine("JSON: " + json);
            //Create object from serialized data
            MusicTrack trackRead = JsonConvert.DeserializeObject<MusicTrack>(json);
            Console.WriteLine("Read back: " + trackRead);

            //A JSON document is enclosed in braces and contains a series of name and value pairs.
            //JSON representation of track object
            //{"Artist":"Rob Miles","Title":"My Way","Length":150}
            //Note that the type of data(the fact that it is an instance of the MusicTrack
            //class) is not stored as part of the document.The JSON document only contains
            //the names and values of the data members of the MusicTrack instance. Note
            //also that there is no type information added to the Length property. This is
            //stored as a string of text and it is up to the recipient of the JSON document to
            //understand this and act appropriately.
            List<MusicTrack> album = new List<MusicTrack>();
            string[] trackNames = new[] { "My Way", "Your Way", "Their Way", "The Wrong Way" };
            foreach (string trackName in trackNames)
            {
                MusicTrack newTrack = new MusicTrack(artist: "Rob Miles", title: trackName,
                length: 150);
                album.Add(newTrack);
            }

            string albumSerialized = JsonConvert.SerializeObject(album);
            Console.WriteLine("Collection serialized: " + albumSerialized);
            List<MusicTrack> DeserializedList = JsonConvert.DeserializeObject<List<MusicTrack>>(albumSerialized);
            foreach(MusicTrack musicTrack in DeserializedList)
            {
                Console.WriteLine(musicTrack);
            }
            //The JSON encoded data for the album list looks as follows:
            //[{"Artist":"Rob Miles","Title":"My Way","Length":150},
            //{"Artist":"Rob Miles","Title":"Your Way","Length":150},
            //{"Artist":"Rob Miles","Title":"Their Way","Length":150},
            //{"Artist":"Rob Miles","Title":"The Wrong Way","Length":150}]

            //When loading a class back using JSON you need to provide the type into
            //which the result is to be stored.No type information is stored in the file that
            //is stored.The Length property of the MusicTrack is automatically
            //converted into an integer upon reloading because the JSON deserializer
            //determines the type of each property in the destination object and then
            //performs type conversion automatically.This is a nice example of the use of the reflection techniques

            //There is absolutely nothing to prevent changes to the content of the text in a
            //JSON document.If you wish to detect modification of a document
            //transferred by JSON you can add a checksum or hash property to the type
            //that is validated by the recipient of the data.

            //XML(eXtensible Markup Language) is another way of expressing the content of
            //an object in a portable and human readable form. This is a slightly more
            //heavyweight standard, in that an XML document contains more metadata(data
            //about data) than a JSON document. An example of an XML file is given below:
            /*<? xml version = "1.0" encoding = "utf-16" ?>
               < MusicTrack xmlns:xsi = http://www.w3.org/2001/XMLSchema-instance
            xmlns: xsd = "http://www.w3.org/2001/XMLSchema" >
             < Artist > Rob Miles </ Artist >
                < Title > My Way </ Title >
                   < Length > 150 </ Length >
                   </ MusicTrack >*/

            //XML serialization can only save and load the public data elements in a type.
            //If you want to save the private elements in a class you should use the Data Contract serializer
            //For XML serialization to work the class being serialized must contain a
            //parameterless constructor(a constructor that accepts no parameters).
            //XML documents can have a schema attached to them. A schema formally
            //sets out the items that a document must contain to be valid. For example, for
            //the MusicTrack type the schema can require that MusicTrack
            //elements in the document must contain Artist, Title, and Length
            //elements. Schemas can be used to automatically validate the structure of
            //incoming documents.            
            //Elements in an XML document can also be given attributes to provide more
            //information about them. For example, the Length attribute of a
            //MusicTrack can be given attributes to indicate that it is an integer and the
            //units of the value are in seconds.
            //An XML document is no less vulnerable to tampering than a JSON
            //document.However, the attribute mechanism can be used to add validation
            //information to data fields.

            MusicTrack track2 = new MusicTrack(artist: "Rob Miles", title: "My Way", length: 150);
            XmlSerializer musicTrackSerializer = new XmlSerializer(typeof(MusicTrack));
            TextWriter serWriter = new StringWriter();
            musicTrackSerializer.Serialize(textWriter: serWriter, o: track2);
            serWriter.Close();
            string trackXML = serWriter.ToString();
            Console.WriteLine("Track XML\n" + trackXML);
            //The XML deserialization process returns a reference to an object.
            TextReader serReader = new StringReader(trackXML);
            MusicTrack DeserializedTrack = musicTrackSerializer.Deserialize(serReader) as MusicTrack;
            Console.WriteLine(DeserializedTrack);

            //Validate JSON data
            //You can perform simple text - based checks on a JSON file to get some level of
            //confidence about the validity of its contents.For example, a program can check
            //that the text starts and ends with a matching pair of brace characters(curly
            //brackets), contains the same number of open square brackets as close square
            //brackets, and an even number of double quote characters.
            string invalidJson = "{\"Artist\":\"Rob Miles\",\"Title\":\"My Way\",\"Length\":150\"}";
            Console.WriteLine(invalidJson);

            try
            {
                MusicTrack musicTrack1 = JsonConvert.DeserializeObject<MusicTrack>(invalidJson);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e.Message);
            }

            //Data collection type
            //From a program security point of view, it is advisable to use the typed versions
            //of the C# collection classes. The System.Collections namespace provides
            //untyped collection classes such as ArrayList.These are regarded as untyped
            //because the data in them is managed in terms of a reference to an object.Since
            //object is the base type of all C# types; this means that such collections can
            //hold any type of object, in any arrangement.
            //In other words, a single ArrayList collection can hold references to
            //Person, MusicTrack, and BankAccount objects.This can lead to
            //problems at runtime, if an application extracts a MusicTrack reference from
            //the ArrayList and tries to use it as a BankAccount the application will fail
            //with a type exception.
            ArrayList arrayList = new ArrayList();
            MusicTrack track3 = new MusicTrack(artist: "Alfa", title: "My Way", length: 150);
            MusicTrack track4 = new MusicTrack(artist: "Beta", title: "My Way", length: 150);
            MusicTrack track5 = new MusicTrack(artist: "Gamma", title: "My Way", length: 150);
            arrayList.Add(track3);
            arrayList.Add(track4);
            arrayList.Add(track5);
            arrayList.Add("Hello");
            arrayList.Add("World");
            // Displays the properties and values of the ArrayList.
            Console.WriteLine("arrayList");
            Console.WriteLine("    Count:    {0}", arrayList.Count);
            Console.WriteLine("    Capacity: {0}", arrayList.Capacity);
            Console.Write("    Values:\n");
            foreach(object obj in arrayList)
            {
                if(obj is MusicTrack)
                {
                    Console.WriteLine("MusicTrack: "+ obj);
                }
                
                if(obj is string)
                {
                    Console.WriteLine("String: " + obj);
                }
            }

            //The List class in the Systems.Collections.Generic namespace uses generic typing, which
            //allows the programmer to specify that a given list can only contain
            //BankAccount references.Any attempts to store MusicTrack references in
            //such a list will fail at compile time.
            //List<MusicTrack> musicTracks = new List<MusicTrack>();
            Queue queue = new Queue(); //first -in, first -out (FIFO)
            Stack stack = new Stack(); //last-in, first out (LIFO)
            queue.Enqueue("Alfa");
            queue.Enqueue("Beta");
            Console.WriteLine("Queue: " + queue.Dequeue() + "== \"Alfa\""); //Alfa First in First out
            stack.Push("Alfa");
            stack.Push("Beta");
            Console.WriteLine("Stack: " + stack.Pop() + "==\"Beta\""); //Last in first out 

            //If your application is going to use an index to access elements and you know
            //in advance how many elements there are, you can use a simple array.This is
            //very useful for lookup tables.
            //If the number of items being stored is not known
            //in advance you can use a List<T> type, which allows your program indexed
            //access to given elements (starting at element zero).
            MusicTrack[] musicTrack2 = new MusicTrack[10]; //Create 10 references, no objects attached yet    
            List<MusicTrack> musicTracks = new List<MusicTrack>();
            //musicTracks.Sort();, sort list using the IComparable interface implementation in MusicTrack  

            //If your application will perform frequent insertions and deletions of data
            //items, and also needs to work through a large list of items, you can use the
            //LinkedList<T> class. This will be more efficient than using an index to
            //access elements in sequence.
            LinkedList<MusicTrack> ll = new LinkedList<MusicTrack>();
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            //If you are storing only strings you can use:
            StringCollection stringCollection = new StringCollection();
            //StringList 

            //Any object that implements the IEnumerable or the IQueryable<T>
            //interface can be acted on by a LINQ query.
            //LINQ queries are compiled into expression trees and execute very quickly.It is easy to
            //create LINQ queries and process their results, and the queries can produce sorted
            //results.IQueryable<T> is more efficient than IEnumerable, because it
            //performs data filtering on the source database, rather than loading all data and
            //then filtering it to produce the result.
            //In keeping with my previously expressed opinions on premature optimization;
            //I advise you to start by using LINQ for any searching and sorting operations,
            //rather than trying to create custom data high performance storage elements.

            //Using the Copy constructor to avoid that a method changes the value of object
            PrintTrack(new MusicTrack(track3));

            //shallow copy vs deep copy 
            //When creating copies of objects, you have to be aware of the implications of
            //copying objects that contain references to other objects.
            //Create copy of reference or make deep copy of reference objects.
            //Deep copy can make copying an object slower because you need to create objects included in the copied object

            //String editing using regular expressions
            //The Regex object(which is in the System.Text.RegularExpressions namespace)
            //provides a method called Replace that can be used to replace regular
            //expressions in a source string. The Replace method is supplied with
            //parameters that specify an input string, a regular expression to match, and a
            //replacement string. When the program runs, all of the spaces in the input string
            //are replaced with commas.
            string input = "Rob Mary David Jenny Chris Imogen Rodney";
            string regularExpressionToMatch = " ";
            string patternToReplace = ",";
            string replaced = Regex.Replace(input, regularExpressionToMatch, patternToReplace);
            Console.WriteLine(replaced);
            string _input = "Rob    Mary    David    Jenny    Chris Imogen    Rodney";
            string _regularExpressionToMatch = " +";
            string _patternToReplace = ",";
            string _replaced = Regex.Replace(_input, _regularExpressionToMatch, _patternToReplace);
            Console.WriteLine(_replaced);

            string __input = "Rob Miles:My Way:120";
            //string regexToMatch = ".+:.+:.+";
            //The character . (period) in a regular expression means “match any character.”
            //The character +means “one or more of the previous item.” So, the character
            //sequence “.+” means “match one or more characters.”
            //The IsMatch method returns True if the regular expression matches a
            //substring in the string being tested.
            //Note that we’ve used the @ prefix to the regular expression string.The @
            //prefix means that the string should be processed by the compiler as a verbatim
            //string.In other words, the compiler will make no attempt to process any escape
            //sequences in the string.This is a good idea because regular expressions
            //frequently contain characters that may be regarded by the compiler as escape sequences.
            //The character $ (dollar)means “anchor this character at the end of the string.
            //The ^ (circumflex)character
            //means “the start of a line.” The | (vertical bar) character means “or.” It is used
            //between alternative match values.It is how you create a validation string to
            //match upper and lower-case letters, along with a space. This selection is
            //enclosed in brackets so that it can be repeated using the +character.
            string regexToMatch = @"^([A-Z]|[a-z]| )+:([A-Z]|[a-z]| )+:[0-9]+$";
            if (Regex.IsMatch(__input, regexToMatch))
                Console.WriteLine("Valid music track description");

            string number = "2020";
            int number1 = int.Parse(number); //rejects invalid values by throwing an exception,
            int number2;
            bool ok = int.TryParse(number, out number2);
            if (ok)
            {
                Console.WriteLine("TryParse(): " + number2);
            }
            else
            {
                Console.WriteLine("This is not a valid number");
            }

            //The C# library also contains a Convert class that can be used to convert between various types.
            //Note that you can even convert strings into Boolean
            //types.The Convert method will throw an exception if the conversion fails, but
            //not if the input value is null.
            string stringValue = "99";
            int intValue = Convert.ToInt32(stringValue);
            Console.WriteLine("intValue: {0}", intValue);
            stringValue = "True";
            bool boolValue = Convert.ToBoolean(stringValue);
            Console.WriteLine("boolValue: {0}", boolValue);
            //Convert.ToInt32 will not, throw an exception if the supplied argument is null.It instead
            //returns the default value for that type. If the supplied argument is null the
            //ToInt32 method returns 0.
        }

        static void PrintTrack(MusicTrack track)
        {
            // Print the track
            Console.WriteLine("PrintTrack: " + track.Artist +", "+ track.Title);
        }
    }
}
