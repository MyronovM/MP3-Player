using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class Song
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Duration { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }

        public Song(string path, string name, string artist, string duration, string genre, int year)
        {
            Path = path;
            Name = name;
            Artist = artist;
            Duration = duration;
            Genre = genre;
            Year = year;
        }
    }
}
