using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class Disc
    {
        public string Name { get; set; }
        public string[] Songs { get; set; }
        public string Description { get; set; }

        public Disc(string name, string description, params string[] songs)
        {
            Name = name;
            Songs = songs;
            Description = description;
        }
    }
}
