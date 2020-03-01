using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.Serialization
{
    public class MusicTrackDB
    {
        public MusicTrackDB()
        {

        }
        public int ID { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
        public int ArtistID { get; set; }
    }
}
