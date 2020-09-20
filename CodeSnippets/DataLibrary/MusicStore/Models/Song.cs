using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.MusicStore.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public Album Album { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Id, Name, Duration);
        }
    }
}
