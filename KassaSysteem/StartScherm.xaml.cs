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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kassa.Model;
using Kassa.Service;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for Startscherm.xaml
    /// </summary>
    public partial class Startscherm : Window
    {
        private TafelService tafelService;
        public Startscherm()
        {
            InitializeComponent();
            vulCanvas();
        }

        public void vulCanvas()
        {
            tafelService = new TafelService();
            int atlTafels = tafelService.getAantal();

            //int marginLeftEerste = 25;
            //int marginTopEerste = 25;
            //int marginLeftTweede = 125;
            //int marinTopTweede = 250;
            //int teller = 0;
            //Boolean eersteRij = true;
            //Boolean tweedeRij = false;

            IEnumerable<Tafel> tafels = tafelService.All();

            foreach (var item in tafels)
            {
                Button b = new Button();
                b.Width = item.Width;
                b.Height = item.Height;
                b.Background = new SolidColorBrush(Colors.Red);
                b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
                b.Content = item.Name;
                b.Tag = item;
                Canvas.SetLeft(b, item.PositionX);
                Canvas.SetTop(b, item.PositionY);
                canvas.Children.Add(b);
            }
            


            //foreach (var item in tafels)
            //{
            //    if(atlTafels <= 8)
            //    {
            //        teller += 1;
            //        Button b = new Button();
            //        b.Width = 125;
            //        b.Height = 125;
            //        b.Background = new SolidColorBrush(Colors.Red);
            //        b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
            //        b.Content = item.Name;
            //        b.Tag = item;
            //        Canvas.SetLeft(b, marginLeftEerste);
            //        Canvas.SetTop(b, marginTopEerste);
            //        canvas.Children.Add(b);
            //        if(teller > 3 && eersteRij == true)
            //        {
            //            tweedeRij = true;
            //            eersteRij = false;
            //            marginLeftEerste += 200;
            //            marginTopEerste = 300;
            //            marginLeftEerste = -100;
            //        }
            //        if (tweedeRij == true )
            //        {
            //            marginLeftEerste += 200;
            //        }
            //        else
            //        {
            //            marginLeftEerste += 200;
            //        }

            //    }
            //}

            //for(int i=0; i<4; i++)
            //{
            //    Button b = new Button();
            //    b.Width = 125;
            //    b.Height = 125;
            //    b.Background = new SolidColorBrush(Colors.Red);
            //    b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
            //    Canvas.SetLeft(b, marginLeftEerste);
            //    Canvas.SetTop(b, marginTopEerste);
            //    canvas.Children.Add(b);
            //    marginLeftEerste += 200;
            //}

            //for (int i = 0; i < 4; i++)
            //{
            //    Button b = new Button();
            //    b.Width = 125;
            //    b.Height = 125;
            //    b.Background = new SolidColorBrush(Colors.Red);
            //    b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
            //    Canvas.SetLeft(b, marginLeftTweede);
            //    Canvas.SetTop(b, marinTopTweede);
            //    canvas.Children.Add(b);
            //    marginLeftTweede += 200;
            //}
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Tafel tafel = (Tafel)b.Tag;
            Kassa kassa = new Kassa(tafel); //Create object of Page2
            kassa.Show(); //Show page2
            this.Close(); //this will close Page1
        }

        private void GoToAdmin(object sender, RoutedEventArgs e)
        {
            AdminScherm admin = new AdminScherm();
            admin.Show();
            this.Close();
        }
    }
}
