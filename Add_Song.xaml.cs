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
    /// Логика взаимодействия для Add_Song.xaml
    /// </summary>
    public partial class Add_Song : Window
    {
        bool type = true;
        int index =  -1;
        bool toReturn = false;
        string song, text1, text2, text3, text4, text5;
        string[] paths;
        Song songSend;
        LinkedList<Song> songList;
        
        public Add_Song(string[] pathsIn, LinkedList<Song> songs)
        {
            InitializeComponent();
            songList = songs;
            buttonOpen.Visibility = Visibility.Visible;
            path.Visibility = Visibility.Visible;
            paths = pathsIn;
            buttonAdd.Content = "Add";
        }

        public Add_Song(bool type, Song songIn, LinkedList<Song> songs, int index)
        {
            InitializeComponent();
            this.index = index;
            songList = songs;
            songSend = songIn;
            this.type = false;
            buttonOpen.Visibility = Visibility.Hidden;
            path.Visibility = Visibility.Hidden;
            textBox1.Text = songIn.Name;
            textBox2.Text = songIn.Artist;
            textBox3.Text = songIn.Duration;
            textBox4.Text = songIn.Year.ToString();
            textBox5.Text = songIn.Genre;
            buttonAdd.Content = "Edit";
        }

        public void Initialize()
        {

        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {           
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = ".mp3"
            };
            openFileDialog.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
            bool? dialogOk = openFileDialog.ShowDialog();

            if (dialogOk == true)
            {
                song = openFileDialog.FileName;
                bool check = true;
                foreach (string path in paths)
                {
                    if (Path.GetFileName(path).Equals(Path.GetFileName(song)))
                        check = false;
                }
                if (check)
                {
                    Opened.Content = Path.GetFileName(openFileDialog.FileName);
                    TagLib.File file = TagLib.File.Create(song);
                    textBox1.Text = file.Tag.Title;
                    textBox2.Text = file.Tag.FirstArtist;
                    string temp = file.Properties.Duration.Minutes + ":";
                    if (file.Properties.Duration.Seconds % 60 > 9) temp = temp + file.Properties.Duration.Seconds + "";
                    else temp = temp + "0" + file.Properties.Duration.Seconds;
                    textBox3.Text = temp;
                }
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            text1 = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            text2 = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            text3 = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            text4 = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            text5 = textBox5.Text;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                Player player = new Player();
                int l = 0;
                int year = int.Parse(textBox4.Text);
                if (year > 2022) throw new PathTooLongException();
                if (text1 == "" || text2 == "" || text3 == "" || text4 == "" || text5 == "")
                    throw new InvalidDataException();
                foreach(Song song in songList)
                {
                    if (song.Name == text1 && l != index)
                    {
                        throw new AccessViolationException();
                    }
                    if (l == index && song.Artist == textBox2.Text)
                    {
                        toReturn = true;
                    }
                    l++;
                }
                if (type) player.recieveDataSong(song, text1, text2, text3, year, text5);
                else player.recieveDataEditSong(songSend.Path, text1, text2, text3, year, text5, toReturn);

                this.Close();
            }
            catch (PathTooLongException ex)
            {
                MessageBox.Show("Year is too long");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Format of year is not accurate");
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show("Some boxs are not filled");
            }
            catch (AccessViolationException ex)
            {
                MessageBox.Show("Song with the same name is created");
            }
            
        }
    }
}
