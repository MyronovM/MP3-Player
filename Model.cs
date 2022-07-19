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
    public class Model
    {
        ListOfSongs listOfSongs;
        ListOfGroups listOfGroups;
        ListOfDiscs listOfDiscs;

        public Model(string linkToSongs, string linkToGroups, string linkToDiscs)
        {
            listOfSongs = new ListOfSongs(linkToSongs);
            listOfGroups = new ListOfGroups(linkToGroups);
            listOfDiscs = new ListOfDiscs(linkToDiscs);
        }

        //Return
        public LinkedList<Song> GetListOfSongs()
        {
            return listOfSongs.GetSongs();
        }

        public LinkedList<Group> GetListOfGroups()
        {
            return listOfGroups.GetGroups();
        }

        public LinkedList<Disc> GetListOfDiscs()
        {
            return listOfDiscs.GetDiscs();
        }

        public string[] GetActualPaths()
        {
            string[] paths = new string[listOfSongs.GetSongs().Count];
            int i = 0;
            foreach (var song in GetListOfSongs())
            {
                paths[i] = song.Path;
                i++;
            }
            return paths;
        }

        public Song GetSong(int index)
        {
            return listOfSongs.GetCertainSong(index);
        }

        public Group GetGroup(int index)
        {
            return listOfGroups.GetCertainGroup(index);
        }

        public Disc GetDisc(int index)
        {
            return listOfDiscs.GetCertainDisc(index);
        }

        public string[] GetAListOfSongsInDisc(int index)
        {
            Disc disc = listOfDiscs.GetCertainDisc(index);
            return disc.Songs;
        }

        public Song FindSongByPath(string path)
        {
            foreach (Song song in listOfSongs.GetSongs())
            {
                if (path == song.Path) return song;
            }
            return null;
        }

        public Song FindSongByPathJson(string path)
        {
            foreach (Song song in listOfSongs.GetSongs())
            {
                if (Path.GetFileName(path) == song.Name + ".json") return song;
            }
            return null;
        }

        public Disc FindDiscByPath(string path)
        {
            foreach (Disc disc in GetListOfDiscs())
            {
                if (path == "C:\\Player\\Discs\\" + disc.Name + ".json") return disc;
            }
            return null;
        }

        public LinkedList<Disc> FindDiscBySong(string song)
        {
            LinkedList<Disc> discs = new LinkedList<Disc>();
            foreach (Disc disc in listOfDiscs.GetDiscs())
            {
                foreach (var songInDisc in disc.Songs)
                {
                    Song found = FindSongByPath(songInDisc);
                    if (found != null)
                    {
                        if (found.Name == song)
                        {
                            discs.AddLast(disc);
                            //break;
                        }
                    }
                }
            }
            return discs;
        }

        //Add
        public void AddSong(Song song)
        {
            Group found = null;
            foreach (Group group in GetListOfGroups())
            {
                if (group.Name == song.Artist)
                {
                    found = group;
                    break;
                }
            }
            if (found != null)
            {
                string[] temp = found.Songs.Concat(new string[] { song.Path }).ToArray();
                found.Songs = temp;
                listOfGroups.Add_Json_And_List(found.Name, found);
                listOfSongs.Add_Json_And_List(song.Name, song);
            }
            else
            {
                Group group = new Group(song.Artist, "Not defined", "Not defined", song.Path);
                listOfGroups.Add_Json_And_List(group.Name, group);
                listOfSongs.Add_Json_And_List(song.Name, song);
            }
        }

        public void AddDisc(Disc disc)
        {
            string[] songs = new string[disc.Songs.Length];
            int i = 0;
            foreach (string song in disc.Songs)
            {
                Song found = FindSongByPathJson(song);
                songs[i] = found.Path;
                i++;
            }
            listOfDiscs.Add_Json(disc.Name, new Disc(disc.Name, disc.Description, songs));
        }

        //Delete
        public void DeleteSong(int index)
        {
            Song song = listOfSongs.GetCertainSong(index);
            foreach (Disc disc in listOfDiscs.GetDiscs())
            {
                foreach (var songInDisc in disc.Songs)
                {
                    if (Path.GetFileName(songInDisc).Equals(Path.GetFileName(song.Path)))
                    {
                        disc.Songs = disc.Songs.Where(s => s != songInDisc).ToArray();
                        File.WriteAllText("C:\\Player\\Discs\\" + disc.Name + ".json", JsonConvert.SerializeObject(disc));
                    }
                }
            }
            foreach (Group group in listOfGroups.GetGroups())
            {
                foreach (var songInGroup in group.Songs)
                {
                    if (Path.GetFileName(songInGroup).Contains(Path.GetFileName(song.Path)))
                    {
                        group.Songs = group.Songs.Where(s => s != song.Path).ToArray();
                        listOfGroups.Add_Json(group.Name, group);
                    }
                }
            }
            listOfSongs.DeleteCertainSong(index);
            File.Delete(song.Path);
            File.Delete("C:\\Player\\SongsJson\\" + song.Name + ".json");
        }

        public void DeleteGroup(int index)
        {
            Group group = listOfGroups.GetCertainGroup(index);
            foreach (var song in group.Songs)
            {
                File.Delete(song);
                Song found = FindSongByPath(song);
                File.Delete("C:\\Player\\SongsJson\\" + found.Name + ".json");
                listOfSongs.DeleteSong(FindSongByPath(song));

                foreach (Disc disc in listOfDiscs.GetDiscs())
                {
                    foreach (var songInDisc in disc.Songs)
                    {
                        if (Path.GetFileName(songInDisc).Equals(Path.GetFileName(song)))
                        {
                            disc.Songs = disc.Songs.Where(s => s != songInDisc).ToArray();
                            File.WriteAllText("C:\\Player\\Discs\\" + disc.Name + ".json", JsonConvert.SerializeObject(disc));
                        }
                    }
                }
            }
            listOfGroups.DeleteCertainGroup(index);
            File.Delete("C:\\Player\\Groups\\" + group.Name + ".json");
        }

        public void DeleteDisc(int index)
        {
            Disc disc = listOfDiscs.GetCertainDisc(index);
            listOfDiscs.DeleteCertainDisc(index);
            File.Delete("C:\\Player\\Discs\\" + disc.Name + ".json");
        }

        //Edit
        public void EditSong(Song song, bool type)
        {
            if (type == true)
            {
                string temp = song.Path;
                File.Delete("C:\\Player\\SongsJson\\" + FindSongByPath(temp).Name + ".json");
                File.WriteAllText("C:\\Player\\SongsJson\\" + song.Name + ".json", JsonConvert.SerializeObject(song));
            }
            else
            {
                Group found = null;
                Group compare = null;
                string temp = null;
                foreach (Group group in GetListOfGroups())
                {
                    if (group.Name.Equals(song.Artist))
                    {
                        found = group;
                    }
                }
                Group tempGroup = found;
                if (found != null)
                {
                    foreach (Group groupFor in GetListOfGroups())
                    {
                        foreach (string path in groupFor.Songs)
                        {
                            if (song.Path.Equals(path))
                            {
                                compare = groupFor;
                                temp = path;
                            }
                        }
                    }
                    if (compare != null)
                    {
                        compare.Songs = compare.Songs.Where(s => s != temp).ToArray();
                        compare.Songs = compare.Songs.Where(s => s != null).ToArray();
                        listOfGroups.Add_Json_And_List(compare.Name, compare);
                    }
                    File.Delete("C:\\Player\\SongsJson\\" + FindSongByPath(song.Path).Name + ".json");
                    listOfSongs.Add_Json(song.Name, song);
                    string[] tempArray = tempGroup.Songs.Concat(new string[] { song.Path }).ToArray();
                    tempGroup.Songs = tempArray;
                    listOfGroups.Add_Json_And_List(tempGroup.Name, tempGroup);
                }
                else
                {
                    foreach (Group groupFor in GetListOfGroups())
                    {
                        foreach (string path in groupFor.Songs)
                        {
                            if (song.Path == path)
                            {
                                found = groupFor;
                                temp = path;
                            }
                        }
                    }
                    if (found != null)
                    {
                        found.Songs = found.Songs.Where(s => s != temp).ToArray();
                        found.Songs = found.Songs.Where(s => s != null).ToArray();
                        listOfGroups.Add_Json_And_List(found.Name, found);
                    }
                    File.Delete("C:\\Player\\SongsJson\\" + FindSongByPath(song.Path).Name + ".json");
                    Group group = new Group(song.Artist, "Not defined", "Not defined", song.Path);
                    listOfGroups.Add_Json_And_List(group.Name, group);
                    listOfSongs.Add_Json_And_List(song.Name, song);
                }
            }
        }

        public void EditGroup(string name, string activity, string country, Group group)
        {
            string[] songs = new string[GetActualPaths().Length];
            int s = 0;
            foreach (Song song in GetListOfSongs())
            {
                if (song.Artist == group.Name)
                {
                    song.Artist = name;
                    songs[s] = song.Path;
                    s++;
                }
            }
            songs = songs.Where(x => x != null).ToArray();
            foreach(string song in songs)
            {
                Song found = FindSongByPath(song);
                File.Delete("C:\\Player\\SongsJson\\" + found.Name + ".json");
                found.Artist = name;
                File.WriteAllText("C:\\Player\\SongsJson\\" + found.Name + ".json", JsonConvert.SerializeObject(found));
            }
            File.Delete("C:\\Player\\Groups\\" + group.Name + ".json");
            File.WriteAllText("C:\\Player\\Groups\\" + name + ".json", JsonConvert.SerializeObject(new Group(name, activity, country , songs)));
        }

        public void EditDisc(Disc disc, string path)
        {
            File.Delete(path);
            File.WriteAllText("C:\\Player\\Discs\\" + disc.Name + ".json", JsonConvert.SerializeObject(disc));
        }
    }
}