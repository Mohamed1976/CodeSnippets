using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.String_Manipulation
{
    class MusicTrack : IFormattable
    {
        public MusicTrack(string artist, string title)
        {
            Artist = artist;
            Title = title;
        }

        public string Artist { get; private set; }
        public string Title { get; private set; }

        //The MusicTrack class also contains a ToString
        //method that accepts two parameters.The first of these is a string that specifies a
        //format for the conversion of the MusicTrack information into a string. The
        //second parameter is format provided that the MusicTrack can use to
        //determine the culture for the conversion.
        public string ToString(string format, IFormatProvider formatProvider)
        {
            // Select the default behavior if no format specified
            if (string.IsNullOrWhiteSpace(format))
                format = "G";

            switch (format)
            {
                case "A": return Artist;
                case "T": return Title;
                case "G": // default format
                case "F": return Artist + " " + Title;
                default:
                    throw new FormatException("Format specifier was invalid.");
            }

            throw new NotImplementedException();
        }

        // ToString that overrides the behavior in the base class
        public override string ToString()
        {
            return Artist + " " + Title;
        }
    }
}
