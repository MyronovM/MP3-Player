using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public abstract class List
    {
        string[] Paths;
        string File;
        string[] Paths_Certain;

        public abstract List Get();
        public abstract List GetCertain(string path);
        public abstract void DeleteCertain();
        public abstract void Add_Json(string path);
    }
}
