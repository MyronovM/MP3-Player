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
    public class ListOfSongs
    {
        LinkedList<Song> songs;
        string[] paths_Songs;
        string file;

        public ListOfSongs(string fileName)
        {
            songs = new LinkedList<Song>();
            paths_Songs = Directory.GetFiles(fileName, ".", SearchOption.TopDirectoryOnly);
            file = fileName;
            foreach (string path in paths_Songs)
            {
                songs.AddLast(JsonConvert.DeserializeObject<Song>(File.ReadAllText(path)));
            }
        }

        public LinkedList<Song> GetSongs()
        {
            paths_Songs = Directory.GetFiles(file, ".", SearchOption.TopDirectoryOnly);
            songs = new LinkedList<Song>();
            foreach (string path in paths_Songs)
            {
                songs.AddLast(JsonConvert.DeserializeObject<Song>(File.ReadAllText(path)));
            }
            return songs;
        }

        public Song GetCertainSong(int index)
        {
            if (index > -1)
                return songs.ElementAt(index);
            else
                return null;
        }

        public string[] GetSongsOfDiscs()
        {
            return null;
        }

        public void DeleteCertainSong(int index)
        {
            songs.Remove(GetCertainSong(index));
        }

        public void DeleteSong(Song song)
        {
            songs.Remove(song);
        }

        public void AddSong(Song song)
        {
            songs.AddLast(song);
        }

        public void Add_Json_And_List(string name, Song song)
        {
            songs.AddLast(song);
            File.WriteAllText("C:\\Player\\SongsJson\\" + name + ".json", JsonConvert.SerializeObject(song));
        }

        public void Add_Json(string name, Song song)
        {
            File.WriteAllText("C:\\Player\\SongsJson\\" + name + ".json", JsonConvert.SerializeObject(song));
        }
    }
}