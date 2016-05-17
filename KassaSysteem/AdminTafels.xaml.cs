using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Kassa.Model;
using Kassa.Service;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for AdminTafels.xaml
    /// </summary>
    public partial class AdminTafels : Window
    {
        private TafelService tafelService;
        private Button huidig;
        private double minWidth;
        private double minHeight;
        private List<Tafel> delete; 

        public AdminTafels()
        {
            minHeight = 50;
            minWidth = 50;
            tafelService=new TafelService();
            InitializeComponent();
            VullTafels();
            Tables.MinHeight = minHeight;
            Tables.MinWidth = minWidth;
            delete = new List<Tafel>();


        }

        private void VullTafels()
        {
            tafelService = new TafelService();
            

            IEnumerable<Tafel> tafels = tafelService.All();

            foreach (var item in tafels)
            {
                Button b = new Button();
                if (b.Width > minWidth)
                {
                    minWidth = b.Width;
                }
                b.Width = item.Width;
                b.Height = item.Height;
                if (b.Height > minHeight)
                {
                    minHeight = b.Height;
                }
                b.Background = new SolidColorBrush(Colors.Red);
                b.AllowDrop = true;
                b.Content = item.Name;
                b.Tag = item;
                
                b.Click+=new RoutedEventHandler(this.setTable);
                Canvas.SetLeft(b, item.PositionX);
                Canvas.SetTop(b, item.PositionY);
                Tables.Children.Add(b);
            }
        }

        private void setTable(Object sender, RoutedEventArgs e)
        {
            Button b = (Button) sender;
            Tafel item = (Tafel)b.Tag;
            huidig = b;
            huidig.Background= new SolidColorBrush(Colors.Blue);
            xposition.Text = item.PositionX.ToString();
            yposition.Text = item.PositionY.ToString();
            artikelNaam.Text = item.Name;
            foreach (Button itemB in Tables.Children)
            {
                if (itemB != huidig)
                {
                    itemB.Background = new SolidColorBrush(Colors.Red);
                }
            }
            Console.WriteLine("Click");
        }

        private void X_position_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (!(Regex.IsMatch(t.Text, @"\D+?")) && !t.Text.Equals(""))
            {
                if (huidig != null)
                {
                    Tafel tafel = (Tafel)huidig.Tag;
                    int x = Convert.ToInt32(t.Text);
                    if (x <= Tables.MaxWidth)
                    {
                        Console.WriteLine(MaxWidth);
                        tafel.PositionX = x;
                        huidig.Tag = tafel;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);
                    }
                    else
                    {
                        xposition.Text = "";
                    }
                   
                }
                else
                {

                        xposition.Text = "";

                   
                }
            }
            else
            {
                if (huidig != null)
                {

                    if (!t.Text.Equals(""))
                    {
                        Tafel tafel = (Tafel)huidig.Tag;
                        xposition.Text = tafel.PositionX.ToString();
                    }
                }
                else
                {

                        xposition.Text = "";

                    
                }
            }
           
        }

        private void Y_position_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (!(Regex.IsMatch(t.Text, @"\D+?")) && !t.Text.Equals(""))
            {
                if (huidig != null)
                {
                    Tafel tafel = (Tafel)huidig.Tag;
                    int y = Convert.ToInt32(t.Text);
                    if (y <= Tables.ActualHeight)
                    {
                        tafel.PositionY = y;
                        huidig.Tag = tafel;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);
                    }
                    else
                    {
                       
                            yposition.Text = "";

                    }
                    

                }
                else
                {
                    yposition.Text = "";
                }
            }
            else
            {
                if (huidig != null)
                {

                    if (!t.Text.Equals(""))
                    {
                        Tafel tafel = (Tafel)huidig.Tag;
                        yposition.Text = tafel.PositionY.ToString();
                    }

                }
                else
                {
                    yposition.Text = "";
                }

                  
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            List <Tafel> tafels= new List<Tafel>();
            UIElementCollection buttons = Tables.Children;
            foreach (var item in buttons)
            {
                Button b = (Button) item;
                Tafel tafel = (Tafel) b.Tag;
                tafels.Add(tafel);
            }
            AdminScherm admin = new AdminScherm();

            foreach (var item in tafels)
            {
                tafelService.Update(item);
            }

            foreach (var item in delete)
            {
                tafelService.Delete(item);
            }
            admin.Show();
            this.Close();
        }
      
        private void deleteTable(object sender, RoutedEventArgs e)
        {
            if (huidig != null)
            {
                Tafel tafel = (Tafel) huidig.Tag;
                delete.Add(tafel);
                Tables.Children.Remove(huidig);
            }
        }

        private void addTable(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("add table");
           huidig = new Button();
            Tafel tafel = new Tafel();
            tafel.Name = "New table";
            tafel.PositionX = 0;
            tafel.PositionY = 0;
            huidig.Tag = tafel;
            Tables.Children.Add(huidig);
            Canvas.SetLeft(huidig, tafel.PositionX);
            Canvas.SetTop(huidig, tafel.PositionY);
        }

        private void ArtikelNaam_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;

                if (huidig != null)
                {
                        Tafel tafel = (Tafel)huidig.Tag;
                        tafel.Name = t.Text;                    
                        huidig.Tag = tafel;
                        huidig.Content = tafel.Name;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);
                        artikelNaam.Text = t.Text;



                }
                else
                {
                    artikelNaam.Text = "";
                }
            
       
        }
    }
}
