using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
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
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Add_Disc : Window
    {
        string[] added, display;
        string text, returnPath;
        bool type = true;
        Model model = new Model(@"C:\Player\SongsJson", @"C:\Player\Groups", @"C:\Player\Discs");
        LinkedList<Disc> listDiscs;
        int index = -1;

        public Add_Disc(LinkedList<Disc> discs, int index)
        {
            InitializeComponent();
            listDiscs = discs;
            button_add.Visibility = Visibility.Visible;
            buttonAdd.Visibility = Visibility.Hidden;
            buttonDelete.Visibility = Visibility.Hidden;
        }

        public Add_Disc(string returnPath, Disc disc, LinkedList<Disc> discs, int index)
        {
            InitializeComponent();
            this.index = index;
            listDiscs = discs;
            type = false;
            this.returnPath = returnPath;
            button_add.Visibility = Visibility.Hidden;
            buttonAdd.Visibility = Visibility.Visible;
            buttonDelete.Visibility = Visibility.Visible;
            textbox1.Text = disc.Name;
            textbox2.Text = disc.Description;
            display = disc.Songs;
            foreach (string file in display)
            {
                list.Items.Add(model.FindSongByPath(file).Name);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int l = 0, k = 0;
            foreach (Disc added in listDiscs)
            {
                if ((added.Name == textbox1.Text && index != -1 && index != l) || (type == true && added.Name == textbox1.Text))
                {
                    k = 1;
                }
                l++;
            }
            if (textbox1.Text == "" || textbox2.Text == "" || k == 1)
            {
                if (k == 1) MessageBox.Show("The same disc is created"); else
                MessageBox.Show("Some fields aren't filled.");
            }
            else
            {
                if (type)
                {
                    Player player = new Player();

                    player.recieveDataDisc(textbox1.Text, textbox2.Text, added);

                    this.Close();
                }
                else
                {
                    Player player = new Player();

                    player.recieveDataEditDisc(textbox1.Text, textbox2.Text, display, returnPath);

                    this.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "Json files (*.json)|*.json";
            dialog.InitialDirectory = "C:\\Player\\SongsJson\\";

            if (dialog.ShowDialog() == true)
            {
                list.Items.Clear();
                added = dialog.FileNames;
                foreach (string file in added)
                {                    
                    list.Items.Add(model.FindSongByPathJson(file).Name);
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
           
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "Json files (*.json)|*.json";
            dialog.InitialDirectory = "C:\\Player\\SongsJson\\";

            if (dialog.ShowDialog() == true)
            {
                list.Items.Clear();
                int s = 0;
                string[] temp = dialog.FileNames;
                string[] toAdd = new string[dialog.FileNames.Length];
                foreach(string file in temp)
                {
                    int l = 0;
                    foreach (string item in display) {
                        if (model.FindSongByPathJson(file).Path.Equals(item))
                        {
                            l = 1;
                        }
                    }
                    if (l == 1)
                    {
                        temp = temp.Where(x => x != file).ToArray();
                    }
                    else
                    {
                        toAdd[s] = model.FindSongByPathJson(file).Path;
                        s++;
                    }
                }
                toAdd = toAdd.Where(x => x != null).ToArray();
                string[] tempArray = display.Concat(toAdd).ToArray();
                display = tempArray;
                display = display.Distinct().ToArray();
                foreach (string file in display)
                {
                    list.Items.Add(model.FindSongByPath(file).Name);
                }
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            text = display[list.SelectedIndex];
            if (text != null)
            {
                list.Items.Clear();
                
                display = display.Where(s => s != text).ToArray();
                display = display.Where(s => s != null).ToArray();
                foreach (string file in display)
                {
                    list.Items.Add(model.FindSongByPath(file).Name);
                }
                text = null;
            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
