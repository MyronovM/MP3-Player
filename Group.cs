using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class Group
    {
        public string Name { get; set; }
        public string[] Songs { get; set; }
        public string Date { get; set; }
        public string Creation { get; set; }

        public Group(string name, string date, string creation, params string[] songs)
        {
            Name = name;
            Songs = songs;
            Date = date;
            Creation = creation;   
        }
    }
}
