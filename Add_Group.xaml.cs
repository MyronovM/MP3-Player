using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для Add_Group.xaml
    /// </summary>
    public partial class Add_Group : Window
    {
        Group group;
        LinkedList<Group> listGroups;
        int index;
       
        public Add_Group(Group group, LinkedList<Group> groups, int index)
        {
            this.index = index;
            InitializeComponent();
            listGroups = groups;
            name.Text = group.Name;
            country.Text = group.Creation;
            activity.Text = group.Date;
            this.group = group;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int l = 0, k = 0;
            foreach (Group added in listGroups)
            {
                if (added.Name == name.Text && l != index)
                {
                    k = 1;
                }
                l++;
            }
            if (name.Text == "" || country.Text == "" || activity.Text == "" || k == 1)
            {
                if (k == 1) MessageBox.Show("The group with the same name is created"); else
                MessageBox.Show("Some fields aren't filled.");
            }
            else
            {
                Player player = new Player();
                
                player.recieveDataGroup(name.Text, country.Text, activity.Text, group);

                this.Close();
            }
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void country_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void activity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
