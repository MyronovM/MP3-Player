using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Threading;

namespace CourseWork
{
    public class ListOfGroups
    {
        LinkedList<Group> groups;
        string[] paths_Groups;
        string file;

        public ListOfGroups(string fileName)
        {
            groups = new LinkedList<Group>();
            paths_Groups = Directory.GetFiles(fileName);
            file = fileName;
            foreach (string path in paths_Groups)
            {
                groups.AddLast(JsonConvert.DeserializeObject<Group>(File.ReadAllText(path)));
            }
        }

        public LinkedList<Group> GetGroups()
        {
            groups = new LinkedList<Group>();
            paths_Groups = Directory.GetFiles(file);
            foreach (string path in paths_Groups)
            {
                groups.AddLast(JsonConvert.DeserializeObject<Group>(File.ReadAllText(path)));
            }
            return groups;
        }

        public Group GetCertainGroup(int index) 
        {
            if (index > -1) 
                return groups.ElementAt(index);
            else
                return null;   
        }

        public void DeleteCertainGroup(int index)
        {
           groups.Remove(GetCertainGroup(index));
        }

        public void Add_Json_And_List(string name, Group group)
        {
            groups.AddLast(group);
            File.WriteAllText("C:\\Player\\Groups\\" + name + ".json", JsonConvert.SerializeObject(group));
        }

        public void Add_Json(string name, Group group)
        {
            File.WriteAllText("C:\\Player\\Groups\\" + name + ".json", JsonConvert.SerializeObject(group));
        }
    }
} 