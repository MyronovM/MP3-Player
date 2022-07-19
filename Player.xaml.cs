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



namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Player : Window
    {
        int k = 0, s = 0;
        public string[] paths_Show;
        string display;
        Model model = new Model(@"C:\Player\SongsJson", @"C:\Player\Groups", @"C:\Player\Discs");
        MediaPlayer mediaPlayer = new MediaPlayer();
        Add_Disc windowDisc;
        Add_Group windowGroup;
        Add_Song windowSong;

        public Player()
        {
            InitializeComponent();

            mediaPlayer.Volume = 50;
            button_play.Visibility = Visibility.Hidden;
            button_previous.Visibility = Visibility.Hidden;
            button_stop.Visibility = Visibility.Hidden;
            button_next.Visibility = Visibility.Hidden;
            button_delete.Visibility = Visibility.Hidden;
            button_add.Visibility = Visibility.Hidden;
            button_edit.Visibility = Visibility.Hidden;
            Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
            model = new Model(settings.Paths[0], settings.Paths[1], settings.Paths[2]);

            paths_Show = new string[model.GetListOfSongs().Count];
        }

        //Previous
        private void button_previous_click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex > 0)
            {
                list.SelectedIndex = list.SelectedIndex - 1;
            }
        }

        //Stop
        private void button_stop_click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        //Next
        private void button_next_click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex < list.Items.Count - 1)
            {
                list.SelectedIndex = list.SelectedIndex + 1;
            }
        }

        //Type
        private void searchbysong_TextChanged(object sender, TextChangedEventArgs e)
        {
            k++;
            if (k == 2) search_bysong.Text = "";

        }

        //Type 2
        private void search_bydiscsTextChanged(object sender, TextChangedEventArgs e)
        {
            s++;
            if (s == 2) search_discs.Text = "";

        }

        //Search
        private void button_search_song_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            string text = search_bysong.Text;
            Group found = null;
            foreach (Group group in model.GetListOfGroups())
            {
                if (group.Name.Equals(text))
                {
                    check = true;
                    found = group;
                }
            }
            int i = 0;
            if (check)
            {
                mediaPlayer.Stop();
                button_add.Visibility = Visibility.Hidden;
                button_edit.Visibility = Visibility.Hidden;
                button_delete.Visibility = Visibility.Hidden;
                button_play.Visibility = Visibility.Visible;
                button_play.Content = "Play";
                button_play.Height = 60;
                button_previous.Visibility = Visibility.Visible;
                button_stop.Visibility = Visibility.Visible;
                button_next.Visibility = Visibility.Visible;
                list.Items.Clear();
                paths_Show = new string[model.GetListOfSongs().Count];
                foreach (var song in found.Songs)
                {
                    Song foundSong = model.FindSongByPath(song);
                    list.Items.Add(foundSong.Name);
                    paths_Show[i] = song;
                    i++;
                }
                display = "songs";
            }
        }

        //Search_2
        private void button_search_disc_Click(object sender, RoutedEventArgs e)
        {
            string song = search_discs.Text;
            bool check = false;
            int i = 0;
            if (model.FindDiscBySong(song).Count != 0)
            {
                mediaPlayer.Stop();
                list.Items.Clear();
                //Array.Clear(paths_Show, 0, paths_Show.Length);
                paths_Show = new string[model.GetListOfSongs().Count];
                foreach (Disc disc in model.FindDiscBySong(song))
                {
                    list.Items.Add(disc.Name);
                    paths_Show[i] = "C:\\Player\\Discs\\" + disc.Name + ".json";
                    i++;
                }
                display = "discs";
                button_play.Visibility = Visibility.Visible;
                button_play.Content = "Open";
                button_play.Height = 95;
                button_next.Visibility = Visibility.Hidden;
                button_previous.Visibility = Visibility.Hidden;
                button_stop.Visibility = Visibility.Hidden;
            }
        }

        //Selection Change
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            name.Content = "";
            artist.Content = "";
            genre.Content = "";
            year.Content = "";
            duration.Content = "";
            image.Source = null;

            if (display == "songs")
            {
                if (list.SelectedIndex > -1)
                {
                    Song song = model.FindSongByPath(paths_Show[list.SelectedIndex]);
                    if (song != null)
                    {
                        TagLib.File file = TagLib.File.Create(paths_Show[list.SelectedIndex]);

                        name.Visibility = Visibility.Visible;
                        name.Content = "Title of the song: \n" + song.Name;
                        artist.Visibility = Visibility.Visible;
                        artist.Content = "Artist: " + song.Artist;
                        duration.Visibility = Visibility.Visible;
                        duration.Content = "Duration: \n" + song.Duration;
                        year.Visibility = Visibility.Visible;
                        year.Content = "Year: \n" + song.Year;
                        genre.Visibility = Visibility.Visible;
                        genre.Content = "Genre: \n" + song.Genre;

                        TagLib.IPicture pic = file.Tag.Pictures[0];
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        ms.Seek(0, SeekOrigin.Begin);
                        // ImageSource for System.Windows.Controls.Image
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        // Create a System.Windows.Controls.Image control
                        Image img = new Image();
                        img.Source = bitmap;
                        image.Source = img.Source;
                    }
                }
            }
            if (display == "groups")
            {
                Group group = model.GetGroup(list.SelectedIndex);
                if (group != null)
                {
                    genre.Visibility = Visibility.Hidden;
                    name.Visibility = Visibility.Visible;
                    name.Content = "Group or artist name: \n" + group.Name;
                    artist.Visibility = Visibility.Visible;
                    artist.Content = "Number of the songs: " + group.Songs.Length;
                    year.Visibility = Visibility.Visible;
                    year.Content = "Dates: \n" + group.Date;
                    duration.Visibility = Visibility.Visible;
                    duration.Content = "Country: \n" + group.Creation;
                }
            }
            if (display == "discs")
            {
                if (list.SelectedIndex > -1)
                {
                    Disc disc = model.FindDiscByPath(paths_Show[list.SelectedIndex]);
                    if (disc != null)
                    {
                        genre.Visibility = Visibility.Hidden;
                        year.Visibility = Visibility.Hidden;
                        duration.Visibility = Visibility.Hidden;
                        name.Visibility = Visibility.Visible;
                        name.Content = "Disc name: \n" + disc.Name;
                        artist.Visibility = Visibility.Visible;
                        artist.Content = "Description: \n" + disc.Description;
                    }
                }
            }
        }

        //Play/Open
        private void button_play_Click(object sender, RoutedEventArgs e)
        {
            if (display == "songs")
            {
                if (list.SelectedIndex > -1)
                {
                    mediaPlayer.Open(new Uri(paths_Show[list.SelectedIndex]));
                    mediaPlayer.Play();
                }
            }
            if (display == "discs")
            {
                if (list.SelectedIndex > -1)
                {
                    button_add.Visibility = Visibility.Hidden;
                    button_edit.Visibility = Visibility.Hidden;
                    button_delete.Visibility = Visibility.Hidden;
                    button_play.Content = "Play";
                    button_play.Height = 60;
                    button_previous.Visibility = Visibility.Visible;
                    button_stop.Visibility = Visibility.Visible;
                    button_next.Visibility = Visibility.Visible;
                    display = "songs";

                    paths_Show = model.FindDiscByPath(paths_Show[list.SelectedIndex]).Songs;
                    list.Items.Clear();

                    foreach (var song in paths_Show)
                    {
                        Song found = model.FindSongByPath(song);
                        list.Items.Add(found.Name);
                    }
                }
            }
        }

        //List Songs
        public void checkbox_songs_Checked(object sender, RoutedEventArgs e)
        {
            display = "songs";
            mediaPlayer.Stop();

            button_add.Content = "Add";
            button_delete.Visibility = Visibility.Visible;
            button_add.Visibility = Visibility.Visible;
            button_edit.Visibility = Visibility.Visible;
            button_play.Visibility = Visibility.Visible;
            button_previous.Visibility = Visibility.Visible;
            button_stop.Visibility = Visibility.Visible;
            button_next.Visibility = Visibility.Visible;
            artist.Visibility = Visibility.Hidden;
            name.Visibility = Visibility.Hidden;
            genre.Visibility = Visibility.Hidden;
            year.Visibility = Visibility.Hidden;
            duration.Visibility = Visibility.Hidden;

            button_play.Content = "Play";
            button_play.Height = 60;
            checkbox_discs.IsChecked = false;
            checkbox_groups.IsChecked = false;
            list.Items.Clear();
            image.Source = null;

            paths_Show = model.GetActualPaths();
            foreach (var song in model.GetListOfSongs())
            {
                list.Items.Add(song.Name);
            }
        }

        //List Groups          
        public void checkbox_groups_Checked(object sender, RoutedEventArgs e)
        {
            display = "groups";
            mediaPlayer.Stop();

            button_add.Content = "Edit";
            button_delete.Visibility = Visibility.Visible;
            button_add.Visibility = Visibility.Visible;
            button_edit.Visibility = Visibility.Hidden;
            button_play.Visibility = Visibility.Hidden;
            button_previous.Visibility = Visibility.Hidden;
            button_stop.Visibility = Visibility.Hidden;
            button_next.Visibility = Visibility.Hidden;
            artist.Visibility = Visibility.Hidden;
            name.Visibility = Visibility.Hidden;
            genre.Visibility = Visibility.Hidden;
            year.Visibility = Visibility.Hidden;
            duration.Visibility = Visibility.Hidden;

            checkbox_discs.IsChecked = false;
            checkbox_songs.IsChecked = false;

            list.Items.Clear();
            image.Source = null;

            foreach (var group in model.GetListOfGroups())
            {
                list.Items.Add(group.Name);
            }
        }

        //List Discs
        private void checkbox_discs_Checked(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();

            display = "discs";
            button_add.Content = "Add";
            button_delete.Visibility = Visibility.Visible;
            button_add.Visibility = Visibility.Visible;
            button_edit.Visibility = Visibility.Visible;
            button_play.Visibility = Visibility.Visible;
            button_previous.Visibility = Visibility.Hidden;
            button_stop.Visibility = Visibility.Hidden;
            button_next.Visibility = Visibility.Hidden;
            button_play.Content = "Open";
            button_play.Height = 95;
            checkbox_songs.IsChecked = false;
            checkbox_groups.IsChecked = false;

            list.Items.Clear();
            image.Source = null;

            foreach (var disc in model.GetListOfDiscs())
            {
                list.Items.Add(disc.Name);
            }
            paths_Show = Directory.GetFiles("C:\\Player\\Discs");
        }

        //Unchecked Songs
        private void checkbox_songs_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkbox_songs.IsChecked == false && checkbox_groups.IsChecked == false && checkbox_discs.IsChecked == false)
            {
                mediaPlayer.Stop();
                button_delete.Visibility = Visibility.Hidden;
                button_add.Visibility = Visibility.Hidden;
                button_edit.Visibility = Visibility.Hidden;
                button_play.Visibility = Visibility.Hidden;
                button_previous.Visibility = Visibility.Hidden;
                button_stop.Visibility = Visibility.Hidden;
                button_next.Visibility = Visibility.Hidden;
                list.Items.Clear();
                image.Source = null;
            }
        }

        //Unchecked Groups
        private void checkbox_groups_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkbox_songs.IsChecked == false && checkbox_groups.IsChecked == false && checkbox_discs.IsChecked == false)
            {
                mediaPlayer.Stop();
                button_delete.Visibility = Visibility.Hidden;
                button_add.Visibility = Visibility.Hidden;
                button_edit.Visibility = Visibility.Hidden;
                button_play.Visibility = Visibility.Hidden;
                button_previous.Visibility = Visibility.Hidden;
                button_stop.Visibility = Visibility.Hidden;
                button_next.Visibility = Visibility.Hidden;
                list.Items.Clear();
                image.Source = null;
            }
        }

        //Unchecked Discs
        private void checkbox_discs_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkbox_songs.IsChecked == false && checkbox_groups.IsChecked == false && checkbox_discs.IsChecked == false)
            {
                mediaPlayer.Stop();
                button_delete.Visibility = Visibility.Hidden;
                button_add.Visibility = Visibility.Hidden;
                button_edit.Visibility = Visibility.Hidden;
                button_play.Visibility = Visibility.Hidden;
                button_previous.Visibility = Visibility.Hidden;
                button_stop.Visibility = Visibility.Hidden;
                button_next.Visibility = Visibility.Hidden;
                list.Items.Clear();
                image.Source = null;
            }
        }

        //Delete
        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            if (checkbox_songs.IsChecked == true && list.SelectedIndex > -1)
            {
                model.DeleteSong(list.SelectedIndex);
                paths_Show = paths_Show.Where(x => x != paths_Show[list.SelectedIndex]).ToArray();
                checkbox_songs.IsChecked = false;
            }
            if (checkbox_groups.IsChecked == true && list.SelectedIndex > -1)
            {
                model.DeleteGroup(list.SelectedIndex);
                checkbox_groups.IsChecked = false;
            }
            if (checkbox_discs.IsChecked == true && list.SelectedIndex > -1)
            {
                model.DeleteDisc(list.SelectedIndex);
                paths_Show = paths_Show.Where(x => x != paths_Show[list.SelectedIndex]).ToArray();
                checkbox_discs.IsChecked = false;
            }
        }

        //Add
        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            if (checkbox_songs.IsChecked == true)
            {
                windowSong = new Add_Song(model.GetActualPaths(), model.GetListOfSongs());
                windowSong.ShowDialog();

                checkbox_songs.IsChecked = false;
            }
            if (checkbox_groups.IsChecked == true && list.SelectedIndex > -1)
            {
                windowGroup = new Add_Group(model.GetGroup(list.SelectedIndex), model.GetListOfGroups(), list.SelectedIndex);
                windowGroup.ShowDialog();

                checkbox_groups.IsChecked = false;
            }
            if (checkbox_discs.IsChecked == true)
            {
                windowDisc = new Add_Disc(model.GetListOfDiscs(), list.SelectedIndex);
                windowDisc.ShowDialog();

                checkbox_discs.IsChecked = false;
            }
        }

        //Edit
        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            if (checkbox_songs.IsChecked == true && list.SelectedIndex > -1)
            {
                windowSong = new Add_Song(true, model.FindSongByPath(paths_Show[list.SelectedIndex]), model.GetListOfSongs(), list.SelectedIndex);
                windowSong.ShowDialog();

                checkbox_songs.IsChecked = false;
            }
            if (checkbox_discs.IsChecked == true && list.SelectedIndex > -1)
            {
                Disc disc = model.FindDiscByPath(paths_Show[list.SelectedIndex]);
                windowDisc = new Add_Disc("C:\\Player\\Discs\\" + disc.Name + ".json", disc, model.GetListOfDiscs(), list.SelectedIndex);
                windowDisc.ShowDialog();

                checkbox_discs.IsChecked = false;
            }
        }

        //RecieveSong
        public void recieveDataSong(string path, string name, string artist, string duration, int year, string genre)
        {
            model.AddSong(new Song(path, name, artist, duration, genre, year));
        }

        public void recieveDataEditSong(string path, string name, string artist, string duration, int year, string genre, bool type)
        {
            model.EditSong(new Song(path, name, artist, duration, genre, year), type);
        }
        
        //RecieveGroup
        public void recieveDataGroup(string name, string country, string active, Group group)
        {
            model.EditGroup(name, active, country, group);
        }
        
        //RecieveDisc
        internal void recieveDataDisc(string text1, string text2, string[] added)
        {
            model.AddDisc(new Disc(text1, text2, added));
        }

        internal void recieveDataEditDisc(string text1, string text2, string[] display, string reWrite)
        {
            model.EditDisc(new Disc(text1, text2, display), reWrite);
        }
    }
}
