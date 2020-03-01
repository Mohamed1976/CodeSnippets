using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;

//Serialization is a complex process.If a data structure contains a graph of
//objects that have a large number of associations between them, the serialization
//process will have to persist each of these associations in the stored file.
//Serialization is best used for transporting data between applications.You can
//think of it as transferring the “value” of an object from one place to another.
//Serialization can be used for persisting data, and a serialized stream can be
//directed into a file, but this is not normally how applications store their state.
//Using serialization can lead to problems if the structure or behavior of the
//classes implementing the data storage changes during the lifetime of the
//application. In this situation developers may find that previously serialized data
//is not compatible with the new design.

//There are essentially two kinds of serialization that a program can use: binary
//serialization and text serialization. A file contains binary data (a sequence of 8-bit values). 
//A UNICODE text file contains 8-bit values that represent text. Binary
//serialization imposes its own format on the data that is being serialized, mapping
//the data onto a stream of 8-bit values.The data in a stream created by a binary
//serializer can only be read by a corresponding binary de-serializer.Binary
//serialization can provide a complete “snapshot” of the source data.
//Both public and private data in an object will be serialized, and the type of each data item is
//preserved. Classes to be serialized by the binary serializer must be marked with the
//[Serializable] attribute. The binary serialization classes are held in the
//System.Runtime.Serialization.Formatters.Binary namespace.

namespace _70_483.Serialization
{
    class SerializationExamples
    {
        public SerializationExamples()
        {
        }

        public void Run()
        {
            //DataContractSerializer
            //The data contract serializer is provided as part of the Windows Communication
            //Framework(WCF).It is located in the System.Runtime.Serialization
            //library.Note that this library is not included in a project by default.It can be
            //used to serialize objects to XML files.It differs from the XML serializer in the
            //following ways:
            //1) Only items marked with the[DataMember] attribute will be serialized.
            //2) It is possible to serialize private class elements (although of course they will
            //   be public in the XML text produced by the serializer).
            //3) The XML serializer provides options that allow programmers to specify the
            //   order in which items are serialized into the data file. These options are not
            //   present in the DataContract serializer.
            /*
            MusicDataStore musicData = MusicDataStore.TestData();
            DataContractSerializer formatter = new DataContractSerializer(typeof(MusicDataStore));
            using (FileStream outputStream =
            new FileStream("MusicTracks.xml", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.WriteObject(outputStream, musicData);
            }
            MusicDataStore inputData;
            using (FileStream inputStream =
            new FileStream("MusicTracks.xml", FileMode.Open, FileAccess.Read))
            {
                inputData = (MusicDataStore)formatter.ReadObject(inputStream);
            }
            */

            //The serialization process handles references to objects differently from binary
            //serialization. The MusicTrack type contains a reference to the Artist describing 
            //the artist that recorded the track.
            //If this is track is serialized using binary serialization, the ArtistDB reference is
            //preserved, with a single ArtistDB instance being referred to by all the tracks that
            //were recorded by that artist. However, if this type of track is serialized using
            //XML serialization, a copy of the Artist value is stored in each track. 
            //When the XML data is deserialized each MusicTrack instance will contain
            //a reference to its own Artist instance, which might not be what you expect.In
            //other words, all of the data serialized using a text serializer is serialized by value.
            //If you want to preserve references you must use binary serialization.
            List<MusicTrackDB> musicTracksDB = null;
            List<ArtistDB> artistsDB = null;
            CreateSampleData(out musicTracksDB, out artistsDB);
            foreach(ArtistDB artist in artistsDB)
            {
                Console.WriteLine("ID: {0}, Name: {1}", artist.ID, artist.Name);
            }
            
            foreach(MusicTrackDB track in musicTracksDB)
            {
                Console.WriteLine("ID: {0}, Title: {1}, Length: {2}, ArtistID: {3}", 
                    track.ID, track.Title, track.Length, track.ArtistID);
            }

            //Lets serialize MusicTrackDB list.
            XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(List<MusicTrackDB>));
            using(FileStream fileStream2 = File.Open("MusicTrackDB.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                xmlSerializer1.Serialize(fileStream2, musicTracksDB);
                fileStream2.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[fileStream2.Length];
                fileStream2.Read(buffer, 0, (int)fileStream2.Length);
                string xmlText = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(xmlText);
            }
            
            //Serialization using the XMLSerializer
            //A program can serialize data into an XML stream using XmlSerializer,
            //when an XmlSerializer instance is created to perform the serialization, 
            //the constructor must be given the type of the data being stored.             
            //XML serialization is called a text serializer, because the serialization process
            //creates text documents.
            List <SoundTrack> soundTracks = new List<SoundTrack>()
            {
                new SoundTrack() { Artist="Artist01", Length=101, Title="Alfa" },
                new SoundTrack() { Artist="Artist02", Length=202, Title="Beta" },
                new SoundTrack() { Artist="Artist03", Length=303, Title="Gamma" }
            };

            foreach(SoundTrack soundTrack in soundTracks)
            {
                soundTrack.Print();
            }

            //Note XmlSerializer generates an error when you try to serialize an internal class    
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SoundTrack>));
            //using (FileStream fileStream2 = new FileStream("MusicDataStore.xml", FileMode.OpenOrCreate, FileAccess.Write))
            using (Stream stream = File.Open("SoundTracks.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                xmlSerializer.Serialize(stream, soundTracks);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] buff = new byte[stream.Length];
                stream.Read(buff, 0, (int)stream.Length);
                string xml = Encoding.UTF8.GetString(buff);
                Console.WriteLine(xml);
            }

            List<SoundTrack> soundTracksDeserialized = null;
            //Deserialize xml file
            using (FileStream inStream = File.Open("SoundTracks.xml", FileMode.Open, FileAccess.Read))
            {
                soundTracksDeserialized = (List<SoundTrack>)xmlSerializer.Deserialize(inStream);
            }

            foreach(SoundTrack soundTrack in soundTracksDeserialized)
            {
                soundTrack.Print();
            }

            //The second way of customizing the serialization process is to add methods
            //that will be called during serialization. The OnSerializing method is called before the
            //serialization is performed and the OnSerialized method is called when the
            //serialization is completed.The same format of attributes is used for the
            //deserialize methods.These methods allow code in a class to customize the
            //serialization process, but they don’t have access to the serialization stream, only
            //the streaming context information.
            //The OnDeserializing method can be used to set values of fields that might
            //not be present in data that is being read from a serialized document.
            //You can use this to manage versions.
            //The [OptionalField] attribute allows you to specify that new fields in a 
            //serializable type(a type to which the SerializableAttribute is applied to) 
            //are ignored by the BinaryFormatter or the SoapFormatter. 
            //This enables version - tolerant serialization of types created for older 
            //versions of an application that serializes data.For example, when the formatters 
            //encounter a stream produced by a version that does not include the new fields, 
            //no exception is thrown, and the existing data on the older type is processed as normal.
            List<Address> addresses = new List<Address>()
            {
                new Address() { City="Amsterdam", Street="Hoofdstraat" },
                new Address() { City="Rotterdam", Street="Hofstraat" },
                new Address() { City="Utrecht", Street="Bergweg" }
            };

            //Print all adresses
            foreach(Address address in addresses)
            {
                address.Print();
            }

            //Serialize List<Address> addresses to file  
            BinaryFormatter binaryFormatter1 = new BinaryFormatter();
            using (FileStream fileStream = new FileStream("Address.dat", FileMode.OpenOrCreate, FileAccess.Write))
            {
                binaryFormatter1.Serialize(fileStream, addresses);
            }

            //Deserialize List<Address> addresses from file
            List<Address> deserializedAddresses = null;
            using (FileStream fileStream1 = new FileStream("Address.dat", FileMode.Open, FileAccess.Read))
            {
                deserializedAddresses = (List<Address>)binaryFormatter1.Deserialize(fileStream1);
            }

            foreach (Address address in deserializedAddresses)
            {
                address.Print();
            }
            
            //ISerializable Interface
            //Implement this interface to allow an object to take part in its own serialization and deserialization.
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
            Person person = new Person("Mad Mike", 16, 2020);
            person.print();

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream outStream = new FileStream("person.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                binaryFormatter.Serialize(outStream, person);
            }

            Person deserializedPerson = null;
            using (FileStream inStream = new FileStream("person.bin", FileMode.Open, FileAccess.Read))
            {
                deserializedPerson = binaryFormatter.Deserialize(inStream) as Person;
            }

            deserializedPerson.print();

            MusicDataStore musicDataStore = MusicDataStore.Create();
            musicDataStore.DisplayArtists();
            musicDataStore.DisplayTracks();

            //Binary serialization is the only serialization technique that serializes private
            //data members by default (i.e.without the developer asking). A file created by a
            //binary serializer can contain private data members from the object being
            //serialized.Note, however, that once an object has serialized there is nothing to
            //stop a devious programmer from working with serialized data, perhaps viewing
            //and tampering with the values inside it.This means that a program should treat
            //deserialized inputs with suspicion.Furthermore, any security sensitive
            //information in a class should be explicitly marked NonSerialized.One way
            //to improve security of a binary serialized file is to encrypt the stream before it is
            //stored, and decrypt it before deserialization.

            //The code next shows how binary serialization is performed.It creates a test
            //MusicDataStore object and then saves it to a binary file.An instance of the
            //BinaryFormatter class provides a Serialize behavior that accepts an
            //object and a stream as parameters.The Serialize behavior serializes the object to a stream.
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream outputStream =
                new FileStream("MusicTracks.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(outputStream, musicDataStore);
            }

            //An instance of the BinaryFormatter class also provides a behavior called
            //Deserialize that accepts a stream and returns an object that it has deserialized from that stream.
            //Deserialize object that was serialized  in example above 
            MusicDataStore inputData = null;
            using (FileStream inputStream = new FileStream("MusicTracks.bin", FileMode.Open, FileAccess.Read))
            {
                //inputData = (MusicDataStore)formatter.Deserialize(inputStream);
                inputData = formatter.Deserialize(inputStream) as MusicDataStore;
            }
            Console.WriteLine("\n\n");
            inputData.DisplayArtists();
            inputData.DisplayTracks();
        }

        private static readonly string[] artistNames = new string[]
        {
            "Rob Miles", "Fred Bloggs", "The Bloggs Singers", "Immy Brown"
        };

        private static readonly string[] titleNames = new string[]
        {
            "My Way", "Your Way", "His Way", "Her Way", "Milky Way"
        };

        //List<MusicTrack> musicTracks is passed to method as reference. 
        private static void GetData(List<Artist> Artists, List<MusicTrack> musicTracks)
        {
            Random rand = new Random(1); 

            foreach(string name in artistNames)
            {
                Artist artist = new Artist()
                {
                    Name = name
                };
                Artists.Add(artist);

                foreach (string title in titleNames)
                {
                    MusicTrack musicTrack = new MusicTrack()
                    {
                        Artist = artist,
                        Title = title,
                        Length = rand.Next(20, 600)
                    };
                    musicTracks.Add(musicTrack);
                }
            }
        }

        private void CreateSampleData(out List<MusicTrackDB> musicTracks, out List<ArtistDB> artists)
        {
            List<MusicTrackDB> _musicTracks = new List<MusicTrackDB>();
            List<ArtistDB> _artists = new List<ArtistDB>();
            Random rand = new Random(1);
            int artistID = 1;
            int musicTrackID = 1;

            foreach (string name in artistNames)
            {
                ArtistDB artist = new ArtistDB() { Name = name, ID = artistID };
                _artists.Add(artist);
                foreach (string title in titleNames)
                {
                    MusicTrackDB musicTrack = new MusicTrackDB()
                    {
                        ID = musicTrackID,
                        Title = title,
                        Length = rand.Next(20, 600),
                        ArtistID = artistID
                    };
                    _musicTracks.Add(musicTrack);
                    musicTrackID++;
                }
                artistID++;
            }

            musicTracks = _musicTracks;
            artists = _artists;
        }

        // Version 1 of the Address class.  
        [Serializable]
        public class Address
        {
            public string Street;
            public string City;

            [OptionalField]
            public string CountryField;

            #region [ Serialization EventHandlers ]

            [OnSerializing()]
            internal void OnSerializingMethod(StreamingContext context)
            {
                Console.WriteLine("Called before Address is serialized");
            }
            [OnSerialized()]
            internal void OnSerializedMethod(StreamingContext context)
            {
                Console.WriteLine("Called after Address is serialized");
            }
            [OnDeserializing()]
            internal void OnDeserializingMethod(StreamingContext context)
            {
                Console.WriteLine("Called before Address is deserialized");
                CountryField = "unknown";
            }
            [OnDeserialized()]
            internal void OnDeserializedMethod(StreamingContext context)
            {
                Console.WriteLine("Called after Address is deserialized");
            }

            #endregion

            public void Print()
            {
                Console.WriteLine("City: {0}, Street: {1}, CountryField: {2}", City, Street, CountryField);
            }
        }

        [Serializable]
        public class Artist
        {
            public Artist()
            {

            }
            public string Name { get; set; }

            //If there are data elements in a class that should not be stored, they can be
            //marked with the NonSerialized attribute as shown below.
            [NonSerialized]
            public int NonSerializableField;
        }
        
        [Serializable]
        public class MusicTrack
        {
            public MusicTrack()
            {

            }
            public Artist Artist { get; set; }
            public string Title { get; set; }
            public int Length { get; set; }
        }

        [Serializable]
        public class MusicDataStore
        {
            public List<Artist> Artists = null;
            public List<MusicTrack> MusicTracks = null;
            //List<Artist> Artists = new List<Artist>();
            //List<MusicTrack> MusicTracks = new List<MusicTrack>();
            public MusicDataStore()
            {

            }

            public void DisplayTracks()
            {
                if (MusicTracks != null)
                {
                    foreach(MusicTrack track in MusicTracks)
                    {
                        Console.WriteLine("Artist:{0} Title:{1} Length:{2}", track.Artist.Name, track.Title, track.Length);
                    }
                }
            }

            public void DisplayArtists()
            {
                if(Artists != null)
                {
                    foreach(Artist artist in Artists)
                    {
                        Console.WriteLine("Artist:{0}", artist.Name);
                    }
                }
            }

            public static MusicDataStore Create()
            {
                List<Artist> artists = new List<Artist>();
                List<MusicTrack> musicTracks = new List<MusicTrack>();
                GetData(artists, musicTracks);

                MusicDataStore result = new MusicDataStore()
                {
                    Artists = artists,
                    MusicTracks = musicTracks
                };

                return result;
            }
        }

        //Examples on how to implement the ISerializable
        //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable?view=netframework-4.8
        //https://docs.microsoft.com/en-us/visualstudio/code-quality/ca2240?view=vs-2019
        //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable.getobjectdata?view=netframework-4.8
        [Serializable]
        public class Person : ISerializable
        {
            public Person(string name, int id, long ssn)
            {
                Name = name;
                ID = id;
                SSN = ssn;
            }

            //Constructor needed for deserialization
            //The constructor for the Artist type accepts info and context
            //parameters and uses the GetString method on the info parameter to obtain
            //the name information from the serialization stream and use it to set the value of
            //the Name property of the new instance.
            protected Person(SerializationInfo info, StreamingContext context)
            {
                Console.WriteLine("Person(SerializationInfo info, StreamingContext context)");
                if (info == null)
                    throw new System.ArgumentNullException("info");
                Name = (string)info.GetValue("AltName", typeof(string));
                ID = (int)info.GetValue("AltID", typeof(int));
                SSN = (long)info.GetValue("SSN", typeof(long));
            }

            private string _name;
            private int _id;

            public string Name
            {
                get { return _name; }
                private set { _name = value; }
            }

            public int ID
            {
                get { return _id; }
                private set { _id = value; }
            }

            private long SSN { get; set; }

            public void print()
            {
                Console.WriteLine("Name: {0}, ID: {1}, SSN: {2}", Name, ID, SSN);
            }

            #region [ ISerializable members ]
            //Code that calls GetObjectData requires the SecurityPermission for providing serialization services.
            //The GetObjectData method must access private data in an object in order
            //to store it. This can be used to read the contents of private data in serialized
            //objects. For this reason, the GetObjectData method definition should be
            //preceded by the security permission attribute.
            [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                Console.WriteLine("GetObjectData(SerializationInfo info, StreamingContext context)");
                if (info == null)
                    throw new System.ArgumentNullException("info");
                //You can give the property names any name. 
                info.AddValue("AltName", Name);
                info.AddValue("AltID", ID);
                info.AddValue("SSN", SSN);
            }

            #endregion
        }

    }
}
