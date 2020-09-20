using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.MusicStore.Models
{
    public class Album
    {
        public Album()
        {
            Songs = new List<Song>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; }

        public override string ToString()
        {
            //string songs = default;
            //foreach(Song s in Songs)
            //{
            //    songs += $"\t{s}\n";
            //}

            return string.Format("{0} {1}", Id, Name);
        }
    }
}
