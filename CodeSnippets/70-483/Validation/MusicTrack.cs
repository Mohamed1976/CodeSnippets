using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace _70_483.Validation
{
    public class MusicTrack : IComparable<MusicTrack>
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        //If you want to save and load private properties in a class you need to mark
        //these items with the[JsonProperty] attribute.
        //[JsonProperty]
        //private int Length { get; set; }
        //This attribute is used to provide the correct validation behaviors,
        [Range(20, 600)]
        public int Length { get; set; }
        // ToString that overrides the behavior in the base class
        public override string ToString()
        {
            return Artist + " " + Title + " " + Length.ToString() + "(seconds)";
        }

        public int CompareTo([AllowNull] MusicTrack other)
        {
            throw new NotImplementedException();
        }

        public MusicTrack(string artist, string title, int length)
        {
            Artist = artist;
            Title = title;
            Length = length;
        }

        // Parameterless constructor required by the XML serializer
        // Does not need to set any property values, these are public.
        public MusicTrack()
        {

        }

        //Copy constructor
        public MusicTrack(MusicTrack musicTrack)
        {
            Artist = musicTrack.Artist;
            Title = musicTrack.Title;
            Length = musicTrack.Length;
        }
    }
}
