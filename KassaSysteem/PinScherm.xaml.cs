using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PinScherm.xaml
    /// </summary>
    public partial class PinScherm : Window
    {
        private Boolean accepted = false;
        public PinScherm()
        {
            InitializeComponent();
            Keyboard.Focus(Input);

        }

        private void GoToAdmin(object sender, RoutedEventArgs e)
        {
            String setting =Properties.Settings.Default["Pincode"].ToString();
            if (Input.Password.Equals(setting))
            {
                accepted = true;
               AdminScherm adminScherm = new AdminScherm();
                adminScherm.Show();
                this.Close();
            }
            else
            {
                Output.Content = "Onjuist wachtwoord";
            }
        }

        private void PinScherm_OnClosing(object sender, CancelEventArgs e)
        {
            if (!accepted)
            {
                Startscherm startscherm = new Startscherm();
                startscherm.Show();
            }

        }

        private void PinScherm_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;

            if (key == Key.Enter)
            {
                RoutedEventArgs args = new RoutedEventArgs();
                GoToAdmin(sender,args);
            }
        }
    }
}
