using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Configuration;
using System.Xml.Linq;

//Language INtegrated Query, or LINQ, was created to make it very easy for C# programmers to work with data sources.

namespace _70_483.LinqQuery
{
    public class LinqQueryExamples
    {
        public LinqQueryExamples()
        {

        }

        const int pageSize = 6;

        private TrackDetails[] GetRecord(int pageNo)
        {
            List<MusicTrackDB> musicTracksDB = null;
            List<ArtistDB> artistsDB = null;
            CreateSampleData(out musicTracksDB, out artistsDB);

            var trackDetails = (from musicTrackDB in musicTracksDB
                                join artistDB in artistsDB on musicTrackDB.ArtistID equals artistDB.ID
                                orderby artistDB.Name descending, musicTrackDB.Title
                                select new TrackDetails
                                {
                                    ArtistName = artistDB.Name,
                                    Title = musicTrackDB.Title
                                }).Skip(pageSize * pageNo).Take(pageSize).ToArray();

            Trace.WriteLineIf(trackDetails == null, "trackDetails == null");
            Trace.WriteLineIf(trackDetails != null, string.Format("trackDetails array size: {0}", trackDetails.Length));

            //IQueryable<TrackDetails> trackDetails 
            //    = (from musicTrackDB in musicTracksDB
            //       join artistDB in artistsDB on musicTrackDB.ArtistID equals artistDB.ID
            //       orderby artistDB.Name descending, musicTrackDB.Title
            //        select new TrackDetails
            //        {
            //            ArtistName = artistDB.Name,
            //            Title = musicTrackDB.Title
            //        }).Skip(pageSize * pageNo).Take(pageSize).AsQueryable();

            //Example below is not orderdby artistDB.Name, example above are correct   
            //var trackDetails = from musicTrackDB in musicTracksDB.Skip(pageSize*pageNo).Take(pageSize)
            //                   join artistDB in artistsDB on musicTrackDB.ArtistID equals artistDB.ID
            //                   orderby artistDB.Name, musicTrackDB.Title
            //                   select new TrackDetails
            //                   {
            //                        ArtistName = artistDB.Name,
            //                        Title = musicTrackDB.Title
            //                   };

            return trackDetails;
        }

        private const string XMLText = 
            "<MusicTracks> " +
                "<MusicTrack> " +
                    "<Artist>Rob Miles</Artist> " +
                    "<Title>My Way</Title> " +
                    "<Length>150</Length>" +
                "</MusicTrack>" +
                "<MusicTrack>" +
                    "<Artist>Immy Brown</Artist> " +
                    "<Title>Her Way</Title> " +
                    "<Length>200</Length>" +
                "</MusicTrack>" +
            "</MusicTracks>";

        public void Run()
        {
            //The LINQ to XML features include very easy way to create XML documents.
            //An XElement can contain a collection of other elements to build the tree structure of an XML document.
            //In the example below, note that the arrangement of the constructor calls to each
            //XElement item mirror the structure of the document.
            XElement MusicTracks = new XElement("MusicTracks",
                new List<XElement> 
                {
                    new XElement("MusicTrack", new XElement("Artist", "Rob Miles"), new XElement("Title", "my Way")),                    
                    new XElement("MusicTrack", new XElement("Artist", "Immy Brown"), new XElement("Title", "Her Way"))
                });

            Console.WriteLine(MusicTracks.ToString());

            //XElement can contain a collection of other elements to build the tree structure of an XML document.
            //You can programmatically add and remove elements to change the structure of the XML document.
            //Suppose that you decide to add a new data element to MusicTrack.You
            //want to store the “style” of the music, whether it is “Pop”, “Rock,” or
            //“Classical.” The code below finds all of the items in our sample data
            //that are missing a Style element and then adds the element to the item.

            //Add ne entry to MusicTracks
            MusicTracks.Add(new XElement("MusicTrack", 
                new XElement("Artist", "Frank Sinatra"), 
                new XElement("Title", "I did it my way"),
                new XElement("Style", "Classical")));

            Console.WriteLine(MusicTracks.ToString());

            IEnumerable<XElement> tracksToModify = from track in MusicTracks.Descendants("MusicTrack")
                                                     where track.Element("Style") == null
                                                     //where string.IsNullOrEmpty(track.Element("Style")?.FirstNode.ToString())
                                                     select track;

            foreach(XElement track in tracksToModify)
            {
                //Console.WriteLine("Artist:{0} Title:{1}",
                //    track.Element("Artist").FirstNode,
                //    track.Element("Title").FirstNode);
                track.Add(new XElement("Style", "Pop"));
            }

            Console.WriteLine(MusicTracks.ToString());

            //The XElement class provides methods that can be used to modify the contents
            //of a given XML element. The ReplaceWith method can be used to change content of 

            //Select elements that have the Title "my Way"  
            IEnumerable<XElement> tracksToChange = from track in MusicTracks.Descendants("MusicTrack")
                                                   where track.Element("Title").FirstNode.ToString() == "my Way"
                                                   select track;

            //XElement [] tracksToChange = (from track in MusicTracks.Descendants("MusicTrack")
            //                              where track.Element("Title").FirstNode.ToString() == "my Way"
            //                              select track).ToArray();

            //We can change the XML content because "IEnumerable<XElement> tracksToChange" references the
            //original content.
            foreach (XElement item in tracksToChange)
            {
                item.Element("Title").FirstNode.ReplaceWith("My Way");
            }

            Console.WriteLine(MusicTracks.ToString());

            //Using XDocument allows you to easily 
            //LINQ to XML features allow you to use LINQ constructions to parse XML documents.
            //The classes thatprovide these behaviors are in the System.XML.Linq namespace.
            //In other section, “Consume XML data” you learned how to consume
            //XML data in a program using the XMLDocument class. This class has been
            //superseded in later versions of.NET(version 3.5 onwards) by the XDocument
            //class, which allows the use of LINQ queries to parse XML files.
            //A program can create an XDocument instance that represents the earlier
            //document by using the Parse method provided by the XDocument class as shown here.
            XDocument musicTracksDocument = XDocument.Parse(XMLText);

            //LINQ have been expressed using query comprehension. It is possible, however, to express 
            //the same query in the form of a method - based query. The Descendants method 
            //returns an object that provides the Where behavior.
            IEnumerable<XElement> selectedTrackElements2 = musicTracksDocument.Descendants("MusicTrack")
                .Where(xElement => xElement.Element("Artist").FirstNode.ToString() == "Rob Miles");

            //We could also use sytax below
            //IEnumerable<XElement> selectedTrackElements2 = musicTracksDocument.Descendants("MusicTrack")
            //    .Where(element => (string)element.Element("Artist") == "Rob Miles");
            foreach (XElement item in selectedTrackElements2)
            {
                Console.WriteLine("Artist:{0} Title:{1} Length:{2}",
                item.Element("Artist").FirstNode,
                item.Element("Title").FirstNode,
                item.Element("Length").FirstNode);
            }

            //A program can perform filtering in the query by adding a where operator, just
            //as with ordinary LINQ statements. Note that the where operation has to extract 
            //the data value from the element so that it can perform the comparison.
            IEnumerable<XElement> selectedTrackElements1 = from track in musicTracksDocument.Descendants("MusicTrack")
                                                           where track.Element("Artist").FirstNode.ToString() == "Rob Miles"
                                                           select track;

            foreach (XElement item in selectedTrackElements1)
            {
                Console.WriteLine("Artist:{0} Title:{1} Length:{2}",
                item.Element("Artist").FirstNode,
                item.Element("Title").FirstNode,
                item.Element("Length").FirstNode);
            }
            
            //The query below selects all the “MusicTrack” elements from the source document.
            //The result of the query is an enumeration of XElement items that have been extracted from the document.
            //The XElement class is a development of the XMLElement class that includes
            //XML behaviors.The program uses a foreach construction to work through the
            //collection of XElement results, extracting the required text values.
            IEnumerable<XElement> selectedTrackElements = from track in musicTracksDocument.Descendants("MusicTrack") 
                                                          select track;
            
            foreach (XElement item in selectedTrackElements)
            {
                Console.WriteLine("Artist:{0} Title:{1} Length:{2}",
                item.Element("Artist").FirstNode,
                item.Element("Title").FirstNode,
                item.Element("Length").FirstNode);
            }
            
            List<MusicTrackDB> musicTracksDB1 = null;
            List<ArtistDB> artistsDB1 = null;
            CreateSampleData(out musicTracksDB1, out artistsDB1);

            //Force execution of a query
            //The result of a LINQ query is an item that can be iterated. We have used the
            //foreach construction to display the results from queries.The actual evaluation
            //of a LINQ query normally only takes place when a program starts to extract
            //results from the query. This is called deferred execution.If you want to force the
            //execution of a query you can use the ToArray().
            //A query result also provides ToList and ToDictionary methods that will
            //force the execution of the query and generate an immediate result of that type.If
            //a query returns a singleton value(for example the result of an aggregation
            //operation such as sum or count) it will be immediately evaluated.
            var artistsSummaryQuery = from artist in artistsDB1
                                      join track in musicTracksDB1 on artist.ID equals track.ArtistID
                                      orderby artist.Name descending
                                      select new
                                      {
                                          Artist = artist.Name,
                                          Title = track.Title,
                                          Length = track.Length
                                      };

            //Execution of the query takes now place
            var artistsSummaryResult = artistsSummaryQuery.ToArray();
            foreach(var artistSummary in artistsSummaryResult)
            {
                Console.WriteLine("Artist: {0} Title: {1} Length: {2}",
                    artistSummary.Artist,
                    artistSummary.Title,
                    artistSummary.Length);
            }

            var artistSummary4 = musicTracksDB1.Join(artistsDB1,
                track => track.ArtistID,
                artist => artist.ID,
                (track, artist) =>
                new //anonymous type   
                            {
                    Track = track,
                    Artist = artist
                })
                .GroupBy(x => x.Artist.Name, x => x.Track)
                .Select(group =>
                new //anonymous type
                {
                    ArtistName = group.Key,
                    TotalSongsLength = group.Sum(track => track.Length),
                    Count = group.Count()
                })
                .OrderByDescending(record => record.ArtistName);

            foreach (var group in artistSummary4)
            {
                Console.WriteLine("Artist Name: {0} TotalSongsLength: {1} Count: {2}", 
                    group.ArtistName, 
                    group.TotalSongsLength,
                    group.Count);
            }
                        
            var artistSummary3 = musicTracksDB1.Join(artistsDB1,
                track => track.ArtistID,
                artist => artist.ID,
                (track, artist) =>
                new //anonymous type   
                {
                    Track = track,
                    Artist = artist
                })
                .GroupBy(x => x.Artist.Name, x => x.Track)
                .Select(x => x);

            //so an IGrouping<K, T> essentially has a Key property of type K and it also implements 
            //IEnumerable<T> so it’s pretty obvious what it is we’re now doing with this – we get 
            //back a list of entries(one for each group) and each of those entries allows us to get 
            //access to the Key and it also then allows us to enumerate the grouped values for that 
            //Key value.So, we can do;
            foreach (var group in artistSummary3)
            {
                Console.WriteLine("Artist Name: {0}", group.Key);

                foreach (var item in group)
                {
                    Console.WriteLine("\tTitle: {0} Length: {1}", item.Title, item.Length);
                }
            }
            
            //Note that you can also create anonymous type instances in method - based SQL queries.
            //The example below shows the use of an intermediate anonymous class that is used to implement
            //the join between the two queries and generate objects that contain artist and
            //track information.
            //Using join instead of LINQ Query
            var artistSummary2 = musicTracksDB1.Join(artistsDB1,
                track => track.ArtistID,
                artist => artist.ID,
                (track, artist) =>
                new //anonymous type   
                {
                    Track = track,
                    Artist = artist
                })
                .OrderBy(record => record.Artist.Name);
                //.OrderByDescending(record => record.Artist.Name);

            //We can now print track and the corresponding artist.    
            foreach (var trackSummary in artistSummary2)
            {
                Console.WriteLine($"Title: {trackSummary.Track.Title} " +
                    $"Length: {trackSummary.Track.Length} " +
                    $"Artist: {trackSummary.Artist.Name}");
            }

            //The phrase “query comprehension syntax” refers to the way that you can build
            //LINQ queries for using the C# operators provided specifically for expressing
            //data queries. The intention is to make the C# statements that strongly resemble
            //the SQL queries that perform the same function. This makes it easier for
            //developers familiar with SQL syntax to use LINQ.
            //LINQPad application allows programmers to create LINQ queries and view 
            //the SQL and method - based implementations.
            //Query below shows Artist name and the total time of their songs   
            //This uses the orderby operator to order the output by artist name.
            var artistSummary1 = from track in musicTracksDB1
                                join artist in artistsDB1 on track.ArtistID equals artist.ID
                                //where artist.Name == "Rob Miles"
                                orderby artist.Name ascending
                                group track by artist.Name
                                into artistGroup
                                select new //Using anonymous type 
                                {
                                    ArtistName = artistGroup.Key,
                                    TotalSongsLength = artistGroup.Sum(track => track.Length)
                                };

            foreach(var summary in artistSummary1)
            {
                Console.WriteLine($"ArtistName: {summary.ArtistName} TotalSongsLength: {summary.TotalSongsLength}");
            }

            //Programs can use the LINQ query methods(and execute LINQ queries) on
            //data collections, such as lists and arrays, and also on database connections. The
            //methods that implement LINQ query behaviors are not added to the classes that
            //use them. Instead they are implemented as extension methods.
            IEnumerable <MusicTrackDB> selectedTracks2 = from track in musicTracksDB1 
                                                       where track.ID == 1 select track;
            //The query statement uses query comprehension syntax, which includes the
            //operators from, in, where, and select. The compiler uses these to generate
            //a call to the Where method on the MusicTracks collection. In other words,
            //the code that is actually created to perform the query is the statement below:
            //In this situation the Where method is receiving a piece of behavior
            //that the method can use to determine which tracks to select.
            IEnumerable<MusicTrackDB> selectedTracks1 = musicTracksDB1.Where(track => track.ID == 1);

            //LINQ aggregate commands
            //In the context of LINQ commands, the word aggregate means “bring together a
            //number of related values to create a single result.” You can use aggregate
            //operators on the results produced by group operations.
            //The parameter to the Sum operator is a lambda expression that the operator
            //will use on each item in the group to obtain the value to be added to the total sum for that item.
            //You can use Average, Max, and Min to generate other items of aggregate
            //information.You can also create your own Aggregate behavior that will be
            //called on each successive item in the group and will generate a single aggregate result.
            var tracks1 = from track in musicTracksDB1
                          join artist in artistsDB1 on track.ArtistID equals artist.ID
                          orderby artist.Name descending
                          group track by artist.Name
                          into artistSummary
                          //select artistSummary; //Selects the IGrouping<string,MusicTrackDB>  
                          select new
                          {
                              ArtistName = artistSummary.Key,
                              NrOfSongs = artistSummary.Count(),
                              AverageSongLength = artistSummary.Average(x => x.Length),
                              MaxSongLength = artistSummary.Max(x => x.Length),
                              MinSongLength = artistSummary.Min(x => x.Length),
                              TotalSongLength = artistSummary.Sum(x => x.Length)
                          };
            foreach(var artist in tracks1)
            {
                Console.WriteLine("ArtistName: {0} NrOfSongs: {1} AvgSongsLength: {2} " +
                    "MaxSongsLength: {3} MinSongsLength: {4} TotalSongsLength: {5}", artist.ArtistName,
                    artist.NrOfSongs,
                    artist.AverageSongLength,
                    artist.MaxSongLength,
                    artist.MinSongLength,
                    artist.TotalSongLength);
            }
               
            //Example of Skip Take can be used for paging.
            //A LINQ query will normally return all of the items that if finds.However, this
            //might be more items that your program wants. For example, you might want to
            //show the user the output one page at a time.You can use take to tell the query
            //to take a particular number of items and the skip to tell a query to skip a
            //particular number of items in the result before taking the requested number.
            TrackDetails[] pagingRecords = null;
            int pageNo = 0;

            pagingRecords = GetRecord(pageNo++);
            while (pagingRecords != null && pagingRecords.Length > 0)
            {
                foreach(TrackDetails trackDetails in pagingRecords)
                {
                    Console.WriteLine("ArtistName: {0} Title: {1}", trackDetails.ArtistName, trackDetails.Title);
                }
                pagingRecords = GetRecord(pageNo++);
            }

            //LINQ provides a join operator that can be used to join the output
            //of one LINQ query to the input of another.
            List<MusicTrackDB> musicTracksDB = null;
            List<ArtistDB> artistsDB = null;
            CreateSampleData(out musicTracksDB, out artistsDB);
            //Combining group and join 
            var _artistSummaries =  from track in musicTracksDB
                                    join artist in artistsDB on track.ArtistID equals artist.ID
                                    group artist by artist.Name
                                    into artistTrackSummary
                                    orderby artistTrackSummary.Key
                                    select new
                                    {
                                        ArtistName = artistTrackSummary.Key,
                                        Count = artistTrackSummary.Count()
                                    };

            foreach (var artistSummary in _artistSummaries)
            {
                Console.WriteLine("Name: {0}, Count: {1}", artistSummary.ArtistName, artistSummary.Count);
            }

            //Another useful LINQ feature is the ability to group the results of a query to
            //create a summary output. For example, you may want to create a query to tell
            //how many tracks there are by each artist in the music collection.
            //The artistTrackSummary contains an entry for each different artist.
            //Each of the items in the summary has a Key property, which is the value that item is
            //“grouped” around.You want to create a group around artists, so the key is the
            //ArtistID value of each track. The Key property of the artistTrackSummary gives the value of this key.
            var artistSummaries = from track in musicTracksDB
                                group track by track.ArtistID
                                into artistTrackSummary
                                select new
                                {
                                    ArtistID = artistTrackSummary.Key,
                                    Count = artistTrackSummary.Count()
                                };
            foreach(var artistSummary in artistSummaries)
            {
                Console.WriteLine("ArtistID: {0}, Count: {1}", artistSummary.ArtistID, artistSummary.Count);
            }
            
            //Select records from "Rob Miles" using LINQ join
            var records = from artist in artistsDB
                          where artist.Name == "Rob Miles"
                          join track in musicTracksDB on artist.ID equals track.ArtistID
                          select new
                          {//Anonymous type 
                              ArtistName = artist.Name,
                              Title = track.Title
                          };

            foreach (var record in records)
            {
                Console.WriteLine("Name:{0} Title:{1}", record.ArtistName, record.Title);
            }

            foreach (MusicTrackDB musicTrackDB in musicTracksDB)
            {
                Console.WriteLine("ID:{0} Title:{1} Length:{2} ArtistID:{3}",
                    musicTrackDB.ID,
                    musicTrackDB.Title,
                    musicTrackDB.Length,
                    musicTrackDB.ArtistID);
            }

            foreach(ArtistDB artistDB in artistsDB)
            {
                Console.WriteLine("ID:{0} Name:{1}", artistDB.ID, artistDB.Name);
            }
            
            //Create sample data
            List<MusicTrack> musicTracks = CreateSampleData();
            //You can remove the need to create a class to hold the result of a search query by
            //making the query return results of an anonymous type.You can see how this
            //works in the code below. Note that the name of the type is now missing from the
            //end of the select new statement.
            var _projection = from track in musicTracks
                              where track.Artist.Name == "Rob Miles"
                              //The query creates new instances of an anonymous type that
                              //contain just the data items needed from the query. Instances of the new type are
                              //initialized using the object initializer syntax.
                              //The item that is returned by this query is an enumerable collection of
                              //instances of a type that has no name. It is an anonymous type.This means you
                              //have to use a var reference to refer to the query result.
                              select new
                              {
                                ArtistName = track.Artist.Name,
                                Title = track.Title
                              };

            foreach(var entry in _projection)
            {
                Console.WriteLine("Artist:{0} Title:{1}", entry.ArtistName, entry.Title);
            }

            //LINQ projection
            //You can use the select operation in LINQ to produce a filtered version of a data source.
            //You can create other search criteria, for example by selecting the
            //tracks with a certain title, or tracks longer than a certain length.
            //The result of a select is a collection of references to objects in the source data collection.
            //There are a couple of reasons why a program might not want to
            //work like this.First, you might not want to provide references to the actual data
            //objects in the data source.Second, you might want the result of a query to
            //contain a subset of the original data.
            //You can use projection to ask a query to “project” the data in the class onto
            //new instances of a class created just to hold the data returned by the query.
            //Projection results like this are particularly useful when you are using data
            //binding to display the results to the user.Values in the query result can be bound
            //to items to be displayed.
            var projection = from track in musicTracks
                             where track.Artist.Name == "Rob Miles"
                             select new TrackDetails
                             {
                                ArtistName = track.Artist.Name,
                                Title = track.Title
                             };

            foreach(TrackDetails trackDetails in projection)
            {
                Console.WriteLine("Artist:{0} Title:{1}", trackDetails.ArtistName,
                    trackDetails.Title);
            }

            //The C# language is “statically typed.” The type of objects in a program is
            //determined at compile time and the compiler rejects any actions that are not valid.
            //Most of the time, the compiler can infer the type to be used for any given variable.
            //You can simplify code by using the var keyword to tell the compiler to infer
            //the type of the variable being created from the context in which the variable is used.
            //The var keyword is especially useful when using LINQ.
            //There are also some situations where you won’t know the type of a variable when
            //writing the code.Later in this section you will discover objects that are created
            //dynamically as the program runs and have no type at all. These are called
            //anonymous types.The only way code can refer to these is by use of variables of type var.
            IEnumerable < MusicTrack > _selectedTracks = from track in musicTracks
                                                             where track.Artist.Name == "Rob Miles"
                                                             select track;

            //To write this statement you must find out the type of data in the
            //musicTracks collection, and then use this type with IEnumerable.The
            //var keyword makes this code much easier to write
            var __selectedTracks = from track in musicTracks
                                   where track.Artist.Name == "Rob Miles"
                                   select track;

            //The code below prints out the titles of all
            //the tracks that were recorded by the artist with the name “Rob Miles.” The first
            //statement uses a LINQ query to create an enumerable collection of
            //MusicTrack references called selectedTracks that is then enumerated by
            //the foreach construction to print out the results.
            IEnumerable<MusicTrack> selectedTracks = from track in musicTracks
                                                     where (track.Artist.Name == "Rob Miles")
                                                     select track;

            foreach(MusicTrack selectedTrack in selectedTracks)
            {
                Console.WriteLine("Artist:{0} Title:{1} Length:{2}", selectedTrack.Artist.Name, 
                    selectedTrack.Title, selectedTrack.Length);
            }

            //Print all records
            foreach (MusicTrack track in musicTracks)
            {
                Console.WriteLine("Artist:{0} Title:{1} Length:{2}", track.Artist.Name, track.Title, track.Length);
            }
        }

        private readonly string[] artistNames = new string[] 
        { 
            "Rob Miles", "Fred Bloggs", "The Bloggs Singers", "Immy Brown" 
        };
        private readonly string[] titleNames = new string[] 
        { 
            "My Way", "Your Way", "His Way", "Her Way", "Milky Way" 
        };

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

        private List<MusicTrack> CreateSampleData()
        {
            List<MusicTrack> musicTracks = new List<MusicTrack>();
            Random rand = new Random(1);

            foreach(string name in artistNames)
            {
                Artist artist = new Artist() { Name = name };
                foreach(string title in titleNames)
                {
                    //If you look at the code below you will see that we are using object
                    //initializer syntax to create new instances of the music objects and initialize their
                    //values at the same time. This is a very useful C# feature that allows you to
                    //initialize objects when they are created without the need to create a constructor
                    //method in the class being initialized. Note the default constructor is called in example below.
                    //Unless the class is static, classes without constructors are given a public parameterless 
                    //constructor by the C# compiler in order to enable class instantiation

                    //You don’t have to initialize all of the elements of the instance; any properties
                    //not initialized are set to their default values(zero for a numeric value and null
                    //for a reference types such as strings).The properties to be initialized in this 
                    //way must all be public members of the class.                                        
                    MusicTrack musicTrack = new MusicTrack()
                    {
                        Artist = artist,
                        Title = title,
                        Length = rand.Next(20, 600)
                    };
                    musicTracks.Add(musicTrack);
                }
            }

            return musicTracks;
        }
    }
}
