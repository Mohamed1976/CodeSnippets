using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Add reference to System.Web.Extensions
using System.Web.Script.Serialization;
//Add a reference to the System.Runtime.Serialization assembly
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

//There are essentially two kinds of serialization that a program can use: binary
//serialization and text serialization.

//Serialization is best used for transporting data between applications.You can
//think of it as transferring the “value” of an object from one place to another.
//Serialization can be used for persisting data, and a serialized stream can be
//directed into a file, but this is not normally how applications store their state.

//.NET Core does not support JavaScriptSerializer Class 
//Note in order to use JavaScriptSerializer Class
//Add reference to System.Web.Extensions
//Right click References and do Add Reference, then from Assemblies->Framework select System.Web.Extensions.

//We can implement JSON Serialization/Deserialization in the following three ways:
//=> Using JavaScriptSerializer class
//=> Using DataContractJsonSerializer class
//=> Using JSON.NET library 

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class SerializationExercises
    {
        public async Task Run()
        {
            //-----------------------------------------------------------------------------------
            // In addition to the ISerializable interface, we can also customize
            // serializable attributes such as: OnSerializing, OnSerialized, OnDeserializing, OnDeserialized 
            //-----------------------------------------------------------------------------------
            const string file2 = "DataFile.dat";
            Archive archive = new Archive();
            Console.WriteLine("\n Before serialization the object contains: ");
            archive.Print();

            BinaryFormatter binaryFormatter2 = new BinaryFormatter();
            // Open a file and serialize the object into binary format.
            using (Stream stream = File.Open(file2, FileMode.Create, FileAccess.Write))
            {
                binaryFormatter2.Serialize(stream, archive);
                //After serialization => member2 = "This value was reset after serialization.";
                Console.WriteLine("\n After serialization the object contains: ");
                archive.Print();
            }

            // Open a file and deserialize the object from binary format.
            using (Stream stream1 = File.Open(file2, FileMode.Open, FileAccess.Read))
            {
                Archive archive1 = (Archive)binaryFormatter2.Deserialize(stream1);
                Console.WriteLine("\n After deserialization the object contains: ");
                archive1.Print();
            }

            //-----------------------------------------------------------------------------------
            // Use custom serialization by implementing the 
            //
            //Sometimes it might be necessary for code in a class to get control during the
            //serialization process.You might want to add checking information or encryption
            //to data elements, or you might want to perform some custom compression of the
            //data. There are two ways that to do this. The first way is to create our own
            //implementation of the serialization process by making a data class implement
            //the ISerializable interface.
            //A class that implements the ISerializable interface must contain a
            //GetObjectData method.This method will be called when an object is
            //serialized.It must take data out of the object and place it in the output stream.
            //The class must also contain a constructor that will initialize an instance of the
            //class from the serialized data source.
            //-----------------------------------------------------------------------------------
            List<Book> books = new List<Book>()
            {
                new Book("27460406", "The Hunger Games"),
                new Book("84363445", "The Stand")
            };

            //Book implements the ISerializable interface
            //We are going the serialize and deserialize book list using BinaryFormatter 
            //Note the data is written in Binary format to file
            const string fileName = "Serializable.bin";

            try
            {
                BinaryFormatter binaryFormatter1 = new BinaryFormatter();
                //Serialize
                using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    binaryFormatter1.Serialize(fileStream, books);
                }

                //Deserialize
                using (FileStream fileStream1 = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    List<Book> books1 = (List<Book>)binaryFormatter1.Deserialize(fileStream1);
                    foreach (Book book in books1)
                        Console.WriteLine(book);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}",ex.Message);
                throw;
            }
            
            //-----------------------------------------------------------------------------------
            //IFormatter = BinaryFormatter
            //
            //When the XML data is deserialized each MusicTrack instance will contain
            //a reference to its own Artist instance, which might not be what you expect.In
            //other words, all of the data serialized using a text serializer is serialized by value.
            //If you want to preserve references you must use binary serialization.
            //If this is track is serialized using binary serialization, the Artist reference is
            //preserved, with a single Artist instance being referred to by all the tracks that
            //were recorded by that artist.
            //
            //Binary serialization imposes its own format on the data that is being serialized, mapping
            //the data onto a stream of 8 - bit values.The data in a stream created by a binary
            //serializer can only be read by a corresponding binary de-serializer.Binary
            //serialization can provide a complete “snapshot” of the source data. Both public
            //and private data in an object will be serialized, and the type of each data item is preserved.
            //Classes to be serialized by the binary serializer must be marked with the
            //[Serializable] attribute as shown below for the Artist class.
            //Properties/fileds you dont want to serialize, you need to decorate them with [NonSerialized] attribute
            //Binary serialization is the only serialization technique that serializes private
            //data members by default(i.e.without the developer asking).
            //-----------------------------------------------------------------------------------
            MusicTrack[] musicTracks3 = MusicTrack.GetTestData();
            // musicTracks[0] and musicTracks[1] share the same Artist instance/object, as shown below 
            Console.WriteLine("ReferenceEquals(musicTracks3[0].Artist, musicTracks3[1].Artist) = {0}",
                              Object.ReferenceEquals(musicTracks3[0].Artist, musicTracks3[1].Artist)); // True
            Console.WriteLine("ReferenceEquals(musicTracks3[2].Artist, musicTracks3[3].Artist) = {0}",
                  Object.ReferenceEquals(musicTracks3[2].Artist, musicTracks3[3].Artist)); // True
            Console.WriteLine("ReferenceEquals(musicTracks3[4].Artist, musicTracks3[5].Artist) = {0}",
                              Object.ReferenceEquals(musicTracks3[4].Artist, musicTracks3[5].Artist)); // True

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream3 = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream3, musicTracks3);
                string base64 = Convert.ToBase64String(memoryStream3.ToArray());
                Console.WriteLine(base64);
                memoryStream3.Position = 0;
                //Note we could also use new memorystream initialized with bytes from Base64 string
                //byte[] fromBase64 = Convert.FromBase64String(base64);
                //MemoryStream memoryStream4 = new MemoryStream(fromBase64);
                //Uses a cast to convert the object returned by the Deserialize method into a MusicTrack[].
                MusicTrack[] musicTracks4 = (MusicTrack[])binaryFormatter.Deserialize(memoryStream3);
                
                foreach (MusicTrack musicTrack in musicTracks4)
                    Console.WriteLine(musicTrack);

                //Note shared objects are maintained, (in XML this does not happen)  
                Console.WriteLine("ReferenceEquals(musicTracks4[0].Artist, musicTracks4[1].Artist) = {0}",
                  Object.ReferenceEquals(musicTracks4[0].Artist, musicTracks4[1].Artist)); // True
                Console.WriteLine("ReferenceEquals(musicTracks4[2].Artist, musicTracks4[3].Artist) = {0}",
                      Object.ReferenceEquals(musicTracks4[2].Artist, musicTracks4[3].Artist)); // True
                Console.WriteLine("ReferenceEquals(musicTracks4[4].Artist, musicTracks4[5].Artist) = {0}",
                                  Object.ReferenceEquals(musicTracks4[4].Artist, musicTracks4[5].Artist)); // True
            }
            
            //-----------------------------------------------------------------------------------
            // Using XmlSerializer formatter = new XmlSerializer(typeof(MusicDataStore));
            // XML serialization is called a text serializer, because the serialization process
            // creates text documents.
            //-----------------------------------------------------------------------------------
            //Note Date because of datetime format is not serialized in XML document 
            WeatherForecast weatherForecast = new WeatherForecast
            { 
                Date = DateTime.Now, TemperatureC = 30, Summary = "Hot" 
            };

            //serialize
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeatherForecast));
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
                using (StreamReader streamReader2 = new StreamReader(memoryStream2))
                {
                    //Serialize
                    xmlSerializer.Serialize(memoryStream2, weatherForecast);
                    memoryStream2.Position = 0;
                    string xml = streamReader2.ReadToEnd();
                    Console.WriteLine(xml);
                    //Deserialize
                    memoryStream2.Position = 0;
                    WeatherForecast weatherForecast2 = (WeatherForecast)xmlSerializer.Deserialize(memoryStream2);
                    Console.WriteLine(weatherForecast2.Summary +", "+ weatherForecast2.TemperatureC +
                        ", Date: "+ weatherForecast2.Date);
                }
            }
                
            //-----------------------------------------------------------------------------------
            // DataContractSerializer 
            // The JSON serializer uses the JavaScript Object Notation to store serialized data
            // in a text file. The data contract serializer is provided as part of the Windows Communication
            // Framework(WCF).It is located in the System.Runtime.Serialization
            // library.Note that this library is not included in a project by default.It can be
            // used to serialize objects to XML files.
            // The methods to serialize and deserialize are called WriteObject and ReadObject respectively.
            // => Only items marked with the [DataMember] attribute will be serialized.
            // => It is possible to serialize private class properties
            //
            //-----------------------------------------------------------------------------------           
            MusicTrack[] musicTracks = MusicTrack.GetTestData();
            // musicTracks[0] and musicTracks[1] share the same Artist instance/object, as shown below 
            Console.WriteLine("ReferenceEquals(musicTracks[0].Artist, musicTracks[1].Artist) = {0}",
                              Object.ReferenceEquals(musicTracks[0].Artist, musicTracks[1].Artist)); // True
            Console.WriteLine("ReferenceEquals(musicTracks[2].Artist, musicTracks[3].Artist) = {0}",
                  Object.ReferenceEquals(musicTracks[2].Artist, musicTracks[3].Artist)); // True
            Console.WriteLine("ReferenceEquals(musicTracks[4].Artist, musicTracks[5].Artist) = {0}",
                              Object.ReferenceEquals(musicTracks[4].Artist, musicTracks[5].Artist)); // True

            //Display all Tracks and artists
            foreach (MusicTrack musicTrack in musicTracks)
                Console.WriteLine(musicTrack);

            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(MusicTrack[]));
            MemoryStream memoryStream1 = new MemoryStream();
            StreamReader streamReader1 = new StreamReader(memoryStream1);
            //Serialization
            dataContractSerializer.WriteObject(memoryStream1, musicTracks);
            memoryStream1.Position = 0;
            string musicTrackSer = streamReader1.ReadToEnd();
            Console.WriteLine(musicTrackSer);
            //Deserialization
            memoryStream1.Position = 0;
            MusicTrack[] musicTracks2  = (MusicTrack[])dataContractSerializer.ReadObject(memoryStream1);
            //After deserialization all Artists are separate instances
            Console.WriteLine("ReferenceEquals(musicTracks2[0].Artist, musicTracks2[1].Artist) = {0}",
                  Object.ReferenceEquals(musicTracks2[0].Artist, musicTracks2[1].Artist)); // False
            Console.WriteLine("ReferenceEquals(musicTracks2[2].Artist, musicTracks2[3].Artist) = {0}",
                  Object.ReferenceEquals(musicTracks2[2].Artist, musicTracks2[3].Artist)); // False
            Console.WriteLine("ReferenceEquals(musicTracks2[4].Artist, musicTracks2[5].Artist) = {0}",
                              Object.ReferenceEquals(musicTracks2[4].Artist, musicTracks2[5].Artist)); // False

            //Display all Tracks and artists
            foreach (MusicTrack musicTrack in musicTracks2)
                Console.WriteLine(musicTrack);

            //-----------------------------------------------------------------------------------
            // NewtonSoft package also has another JsonSerializer as illustrated below 
            // 
            //-----------------------------------------------------------------------------------            
            //WeatherForecast weatherForecast = new WeatherForecast
            //{ Date = DateTime.Now, TemperatureC = 30, Summary = "Hot" };
            //JsonSerializer jsonSerializer = new JsonSerializer();
            //MemoryStream memoryStream2 = new MemoryStream();
            //TextWriter textWriter = new StreamWriter(memoryStream2);
            //jsonSerializer.Serialize(textWriter, weatherForecast);
            //textWriter.Close();
            //memoryStream2.Position = 0;
            //StreamReader streamReader2 = new StreamReader(memoryStream2);
            //string json4 = streamReader2.ReadToEnd();
            //Console.WriteLine(json4);
            //memoryStream1.Position = 0;
            //streamWriter.Flush();
            //StreamReader streamReader1 = new StreamReader(memoryStream1);
            //string json4 = streamReader1.ReadToEnd();
            //Console.WriteLine(json4);
            //string json4 = JsonConvert.SerializeObject(weatherForecast);
            //Console.WriteLine(json4);
            //Create 5 objects serialize and deserialize
            Random rng = new Random();
            string[] Summaries = new string[] { "Hot", "Wet", "Cold", "Rainy"  };
            WeatherForecast[] weatherForecasts = Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).ToArray();

            //Serialize
            string json5 = JsonConvert.SerializeObject(weatherForecasts);
            Console.WriteLine(json5);

            //Deserialize
            WeatherForecast[] weatherForecasts2 = JsonConvert.DeserializeObject<WeatherForecast[]>(json5);
            foreach (WeatherForecast forecast in weatherForecasts2)
                Console.WriteLine(forecast.Date +", "+ forecast.Summary + ", " + forecast.TemperatureC);

            //-----------------------------------------------------------------------------------
            // Using Json.NET, you need to install NewtonSoft package
            // 
            //-----------------------------------------------------------------------------------
            BlogSite[] blogSites2 = new BlogSite[]
            {
                new BlogSite() { Name = "C-sharpcorner", Description = "Share Knowledge"  },
                new BlogSite() { Name = "C++ programmers", Description = "Realtime programming"  },
                new BlogSite() { Name = "Python AI", Description = "Create AI solutions"  }
            };

            string json3 = JsonConvert.SerializeObject(blogSites2);
            Console.WriteLine(json3);
            BlogSite[] blogSites3 = JsonConvert.DeserializeObject<BlogSite[]>(json3);
            foreach (BlogSite blogSite in blogSites3)
            {
                Console.WriteLine(blogSite.Name + ", " + blogSite.Description);
            }
            
            //-----------------------------------------------------------------------------------
            // Using DataContractJsonSerializer
            // Methods are decorated with DataMember attribute, class is decorated wit DataContract attribute  
            //-----------------------------------------------------------------------------------
            BlogSite[] blogSites = new BlogSite[]
            {
                new BlogSite() { Name = "C-sharpcorner", Description = "Share Knowledge"  },
                new BlogSite() { Name = "C++ programmers", Description = "Realtime programming"  },
                new BlogSite() { Name = "Python AI", Description = "Create AI solutions"  }
            };

            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(BlogSite[]));            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    //Serialize array of objects
                    dataContractJsonSerializer.WriteObject(memoryStream, blogSites);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    string json1 = streamReader.ReadToEnd();
                    Console.WriteLine(json1);
                    //Deserialize array of objects
                    memoryStream.Position = 0; // The same as memoryStream.Seek(0, SeekOrigin.Begin) 
                    BlogSite[] blogSites1 = (BlogSite[])dataContractJsonSerializer.ReadObject(memoryStream);
                    foreach (BlogSite blogSite in blogSites1)
                    {
                        Console.WriteLine(blogSite.Name + ", " + blogSite.Description);
                    }
                }
            }

            //-----------------------------------------------------------------------------------
            // Using JavaScriptSerializer
            //-----------------------------------------------------------------------------------
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            List<Person> people = new List<Person>()
            {
                new Person() { PersonID = 1, Name = "Bryon Hetrick", Registered = true },
                new Person() { PersonID = 2, Name = "Nicole Wilcox", Registered = true },
                new Person() { PersonID = 3, Name = "Adrian Martinson", Registered = false },
                new Person() { PersonID = 4, Name = "Nora Osborn", Registered = false }
            };

            //Serialize to JSON format 
            string json = javaScriptSerializer.Serialize(people);
            //Note you can also write output to string builder
            StringBuilder stringBuilder = new StringBuilder();
            javaScriptSerializer.Serialize(people, stringBuilder);
            Console.WriteLine(stringBuilder.ToString());
            Console.WriteLine(json);
            //Deserialization has two 
            List<Person> peopleDeserialized = javaScriptSerializer.Deserialize<List<Person>>(json);
            List<Person> peopleDeserialized2 = (List<Person>)javaScriptSerializer.Deserialize(json, typeof(List<Person>));
            //Note you can also use dynamic to access each member
            //TODO for later, use dynamic objects in serialization 
            //List<dynamic> peopleDeserialized3 = javaScriptSerializer.Deserialize<List<dynamic>>(json);
            //Console.WriteLine("#{0}, {1}, {2}", peopleDeserialized3[0].Name,
            //    peopleDeserialized3[0].PersonID, peopleDeserialized3[0].Registered);
            //Console.WriteLine("#{0}, {1}, {2}", peopleDeserialized3[2]["Name"],
            //    peopleDeserialized3[2].PersonID, peopleDeserialized3[2].Registered);

            foreach (Person person in peopleDeserialized2)
                Console.WriteLine("{0:g}", person);

            foreach (Person person in peopleDeserialized)
                Console.WriteLine("{0}", person.ToString());
        }
    }

    public class Person : IFormattable
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        public bool Registered { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("{0}, {1}, {2}, {3}", Name, PersonID, Registered, format);
        }

        public override string ToString()
        {
            return string.Format("public override string ToString(): {0}, {1}, {2}", Name, PersonID, Registered);
        }
    }

    [DataContract]
    class BlogSite
    {
        [DataMember(Name = "FirstName", IsRequired =true)]
        public string Name { get; set; }

        [DataMember(Name = "MyDescription", IsRequired = true)]
        public string Description { get; set; }
    }

    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Artist
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MusicTrack
    {
        public MusicTrack(string description)
        {
            Description = description;
        }

        [DataMember]
        public int ID { get; set; }
        
        [DataMember]
        public int ArtistID { get; set; }
        
        [DataMember]
        public string Title { get; set; }
        
        [DataMember]
        public int Length { get; set; }

        [DataMember]
        public Artist Artist { get; set; }

        [DataMember]
        private string Description { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3} == {4}", Description, Artist.Name, ID, ArtistID, Artist.ID);
        }

        public static MusicTrack[] GetTestData()
        {
            Artist[] artists = new Artist[]
            {
                new Artist() { ID = 1, Name = "Drake and Future" },
                new Artist() { ID = 2, Name = "Lil Baby, Gunna" },
                new Artist() { ID = 3, Name = "Lady Gaga" }
            };

            MusicTrack[] musicTracks = new MusicTrack[]
            {
                new MusicTrack("#Heatin Up description") { Artist = artists[0], ArtistID = 1, ID = 1, Length = 90, Title = "Heatin Up" },
                new MusicTrack("#Stupid Love description") { Artist = artists[0], ArtistID = 1, ID = 2, Length = 110, Title = "Stupid Love" },
                
                new MusicTrack("#Live Off My Closet description") { Artist = artists[1], ArtistID = 2, ID = 3, Length = 67, Title = "Live Off My Closet" },
                new MusicTrack("#Say So description") { Artist = artists[1], ArtistID = 2, ID = 4, Length = 45, Title = "Say So" },

                new MusicTrack("#La Difícil description") { Artist = artists[2], ArtistID = 3, ID = 5, Length = 44, Title = "La Difícil" },
                new MusicTrack("#Don't Start Now description") { Artist = artists[2], ArtistID = 3, ID = 6, Length = 15, Title = "Don't Start Now" },
            };

            return musicTracks;
        }
    }

    [Serializable]
    public class Book : ISerializable
    {
        //The constructor accepts info and context parameters and uses the GetString method on the 
        //info parameter to obtain the name information from the serialization stream and use it to set the value of
        //the Name property of the new instance. The GetObjectData method must access private data in an object in order
        //to store it.This can be used to read the contents of private data in serialized
        //objects.For this reason, the GetObjectData method definition should be
        //preceded by the security permission attribute you can see below.       
        public Book(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException(nameof(info));

            Console.WriteLine("Book(SerializationInfo info, StreamingContext context)");
            ISBN = (string)info.GetValue("BookTitle", typeof(string));
            Title = (string)info.GetValue("BookISBN", typeof(string));
        }

        public Book(string isbn, string title)
        {
            ISBN = isbn;
            Title = title;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            Console.WriteLine("GetObjectData(SerializationInfo info, StreamingContext context)");
            //We could encrypt date here
            info.AddValue("BookTitle", Title);
            info.AddValue("BookISBN", ISBN);
        }

        private string _isbn;

        public string ISBN
        {
            get { return _isbn; }
            private set { _isbn = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        public override string ToString()
        {
            return Title +", "+ ISBN;
        }
    }

    [Serializable()]
    public class Archive
    {
        public Archive()
        {
            member1 = 11;
            member2 = "Hello World!";
            member3 = "This is a nonserialized value";
            member4 = null;
        }

        // This member is serialized and deserialized with no change.
        public int member1;

        // The value of this field is set and reset during and 
        // after serialization.
        private string member2;

        // This field is not serialized. The OnDeserializedAttribute 
        // is used to set the member value after serialization.
        [NonSerialized()]
        public string member3;

        // This field is set to null, but populated after deserialization.
        private string member4;

        [OptionalField]
        public string Style;

        public void Print()
        {
            Console.WriteLine("member1 = '{0}'", member1);
            Console.WriteLine("member2 = '{0}'", member2);
            Console.WriteLine("member3 = '{0}'", member3);
            Console.WriteLine("member4 = '{0}'", member4);
            Console.WriteLine("Style = '{0}'", Style);
        }

        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerializing()");
            member2 = "This value went into the data file during serialization.";
        }

        [OnSerialized()]
        internal void OnSerializedMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerialized()");
            member2 = "This value was reset after serialization.";
        }

        //The OnDeserializing method can be used to set values of fields that might
        //not be present in data that is being read from a serialized document.
        //Optional properties need to be decorated with the [OptionalField]
        //The method is called before the data for the object is deserialized and can set default values  
        //for data fields.If the input stream contains a value for a field, this will overwrite
        //the default set by OnDeserializing.
        [OnDeserializing()]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            Console.WriteLine("OnDeserializing()");
            member3 = "This value was set during deserialization";
            Style = "unknown";
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Console.WriteLine("OnDeserialized()");
            member4 = "This value was set after deserialization.";
        }
    }
}
