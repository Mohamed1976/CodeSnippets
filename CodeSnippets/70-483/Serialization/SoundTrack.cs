using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.Serialization
{
    [Serializable]
    public class SoundTrack
    {
        public SoundTrack()
        {
        }

        public string Artist { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }

        public void Print()
        {
            Console.WriteLine("Artist: {0}, Title: {1}, Length: {2}", Artist, Title, Length);
        }
    }

}
