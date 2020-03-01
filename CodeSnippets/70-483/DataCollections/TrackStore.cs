using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.DataCollections
{
    public class TrackStore : List<MusicTrack> //Implements the IList, ICollection, IEnumerable interfaces
    {
        public int RemoveArtist(string removeName)
        {
            int count = this.RemoveAll(track => track.Artist.Name == removeName);
            return count;

            //List<MusicTrack> tracksToRemove = new List<MusicTrack>();
            //foreach(MusicTrack musicTrack in this)
            //{
            //    if(musicTrack.Artist.Name == removeName)
            //    {
            //        tracksToRemove.Add(musicTrack);
            //    }
            //}

            //foreach(MusicTrack musicTrack in tracksToRemove)
            //{
            //    this.Remove(musicTrack);
            //}


            //return tracksToRemove.Count;
        }

        public static TrackStore GetTestTrackStore()
        {
            List<MusicTrack> musicTracks = new List<MusicTrack>();
            Random rand = new Random(1);
            string[] artistNames = new string[]
            {
                "Rob Miles", "Fred Bloggs", "The Bloggs Singers", "Immy Brown"
            };

            string[] titleNames = new string[]
            {
                "My Way", "Your Way", "His Way", "Her Way", "Milky Way"
            };

            foreach (string name in artistNames)
            {
                Artist artist = new Artist() { Name = name };
                foreach (string title in titleNames)
                {
                    MusicTrack musicTrack = new MusicTrack()
                    {
                        Title = title,
                        Length = rand.Next(20, 600),
                        Artist = artist
                    };
                    musicTracks.Add(musicTrack);
                }
            }

            TrackStore tracks = new TrackStore();
            tracks.AddRange(musicTracks);
            return tracks;
        }

        public override string ToString()
        {
            string result = default;

            for(int index = 0; index < this.Count; index++)
            {
                result += this[index].Artist.Name + ", " + this[index].Title + ", " + this[index].Length + "\n";
            }

            return result;
        }
    }
}
