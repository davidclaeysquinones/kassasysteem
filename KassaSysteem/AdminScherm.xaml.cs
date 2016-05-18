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

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for AdminScherm.xaml
    /// </summary>
    public partial class AdminScherm : Window
    {
        public AdminScherm()
        {
            InitializeComponent();
        }

        private void Click_Tafels(object sender, RoutedEventArgs e)
        {
            AdminTafels tafels=new AdminTafels();
            tafels.Show(); //Show page2
            this.Close(); //this will close Page1
        }

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            Startscherm startscherm = new Startscherm();
            startscherm.Show();
            this.Close(); //this will close Page1
        }

        private void Click_Artikelen(object sender, RoutedEventArgs e)
        {
            AdminArtikelen adminArtikelen = new AdminArtikelen();
            adminArtikelen.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
