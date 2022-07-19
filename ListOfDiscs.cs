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
    public class ListOfDiscs
    {
        LinkedList<Disc> discs;
        string[] paths_Discs;
        string file;

        public ListOfDiscs(string fileName)
        {
            discs = new LinkedList<Disc>();
            paths_Discs = Directory.GetFiles(fileName);
            file = fileName;
            foreach (string path in paths_Discs)
            {
                discs.AddLast(JsonConvert.DeserializeObject<Disc>(File.ReadAllText(path)));
            }
        }

        public LinkedList<Disc> GetDiscs()
        {
            discs = new LinkedList<Disc>();
            paths_Discs = Directory.GetFiles(file);
            foreach (string disc in paths_Discs)
            {
                discs.AddLast(JsonConvert.DeserializeObject<Disc>(File.ReadAllText(disc)));
            }
            return discs;
        }

        public Disc GetCertainDisc(int index)
        {
            if (index > -1)
                return discs.ElementAt(index);
            else
                return null;
        }
        
        public void DeleteCertainDisc(int index)
        {
            discs.Remove(GetCertainDisc(index));
        }

        public void Add_Json(string name, Disc disc)
        {
            File.WriteAllText("C:\\Player\\Discs\\" + name + ".json", JsonConvert.SerializeObject(disc));
        }
    }
}