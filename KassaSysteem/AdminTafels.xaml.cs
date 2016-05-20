using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        private Boolean pressed;
        private List<Tafel> delete;
        private List<Tafel> update;
        private List<Tafel> add;
        private static int maxWidth = 1000;
        private static int maxHeight = 420;



        public AdminTafels()
        {
            tafelService=new TafelService();
            InitializeComponent();
            VullTafels();
            //Tables.MinHeight = minHeight;
            //Tables.MinWidth = minWidth;
            //Tables.Width = minWidth;
            //Tables.Height = minWidth;
           
            pressed = false;
            delete = new List<Tafel>();
            update = new List<Tafel>();
            add = new List<Tafel>();


        }

        private void VullTafels()
        {
            tafelService = new TafelService();
            

            IEnumerable<Tafel> tafels = tafelService.All();

            foreach (var item in tafels)
            {
                Button b = new Button();
                //if (b.Width > minWidth)
                //{
                //    minWidth = b.Width;
                ////}
                b.Width = item.Width;
                b.Height = item.Height;
                //if (b.Height > minHeight)
                //{
                //    minHeight = b.Height;
                //}
                b.Background = new SolidColorBrush(Colors.Red);
                b.AllowDrop = true;
                b.PreviewMouseLeftButtonDown+= new MouseButtonEventHandler(this.mouseEnter);
                b.PreviewMouseLeftButtonUp+= new MouseButtonEventHandler(this.mouseUp);
                b.PreviewMouseMove+=new MouseEventHandler(this.mouseMove);
                b.Click += new RoutedEventHandler(this.setTable);
                b.Content = item.Name;
                b.Tag = item;
                

                Canvas.SetLeft(b, item.PositionX);
                Canvas.SetTop(b, item.PositionY);
                Tables.Children.Add(b);
            }
        }

        private void mouseEnter(Object sender,MouseEventArgs args)
        {
            Console.WriteLine("mouse enter");
            pressed = true;

            Button b = (Button)sender;
            Tafel item = (Tafel)b.Tag;
            huidig = b;
            huidig.Background = new SolidColorBrush(Colors.Blue);
            xposition.Text = item.PositionX.ToString();
            yposition.Text = item.PositionY.ToString();
            Height.Text = item.Height.ToString();
            Width.Text = item.Width.ToString();
            artikelNaam.Text = item.Name;
            foreach (Button itemB in Tables.Children)
            {
                if (itemB != huidig)
                {
                    itemB.Background = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void mouseUp(Object sender, MouseEventArgs args)
        {

                Console.WriteLine("mouse up");
                pressed = false;


        }

        private void mouseMove(object sender, MouseEventArgs args)
        {
            if (pressed)
            {
                if (huidig != null)
                {
                    Console.WriteLine("item is pressed");
                    Button b = huidig;
                    Tafel tafel = (Tafel)b.Tag;

                    Point input = args.GetPosition(Tables);
                    double xmove = input.X;
                    double ymove = input.Y;

                    
                    Tables.Children.Remove(b);
                    if (xmove >= 0)
                    {
                        if (xmove <= maxWidth)
                        {
                            tafel.PositionX = (int)xmove;
                            Canvas.SetLeft(b, tafel.PositionX);
                        }
                        else
                        {
                            tafel.PositionX = maxWidth;
                            Canvas.SetLeft(b, tafel.PositionX);
                        }
                    }

                    if (ymove >= 0)
                    {
                        if (ymove <= maxHeight)
                        {
                            tafel.PositionY = (int)ymove;
                            Canvas.SetTop(b, tafel.PositionY);
                        }
                        else
                        {
                            tafel.PositionY = maxHeight;
                            Canvas.SetTop(b, tafel.PositionY);
                        }
                    }

                    
                    Tables.Children.Add(b);

                    xposition.Text = tafel.PositionX.ToString();
                    yposition.Text = tafel.PositionY.ToString();
                    huidig = b;
                }

            }
        }

        private void setTable(Object sender, RoutedEventArgs e)
        {
          
                Button b = (Button)sender;
                Tafel item = (Tafel)b.Tag;
                huidig = b;
                huidig.Background = new SolidColorBrush(Colors.Blue);
                xposition.Text = item.PositionX.ToString();
                yposition.Text = item.PositionY.ToString();
                Height.Text = item.Height.ToString();
                Width.Text = item.Width.ToString();
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
                    if (x <= maxWidth)
                    {
                        Console.WriteLine(MaxWidth);
                        tafel.PositionX = x;
                        huidig.Tag = tafel;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);
                        if (!add.Contains(tafel))
                        {
                            if (update.Contains(tafel))
                            {
                                update.Remove(tafel);
                                update.Add(tafel);
                            }
                            else
                            {
                                update.Add(tafel);
                            }
                        }
                        else
                        {
                            add.Remove(tafel);
                            add.Add(tafel);
                        }
                       
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
                Console.WriteLine("aaa");
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
                    if (y <= maxHeight)
                    {
                        tafel.PositionY = y;
                        huidig.Tag = tafel;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);

                        if (!add.Contains(tafel))
                        {
                            if (update.Contains(tafel))
                            {
                                update.Remove(tafel);
                                update.Add(tafel);
                            }
                            else
                            {
                                update.Add(tafel);
                            }
                        }
                        else
                        {
                            add.Remove(tafel);
                            add.Add(tafel);
                        }
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

            foreach (var item in update)
            {
                tafelService.Update(item);
            }


            foreach (var item in delete)
            {
                tafelService.Delete(item);
            }

            foreach (var item in add)
            {
                tafelService.Add(item);
            }
            AdminScherm admin = new AdminScherm();
            admin.Show();
            this.Close();
        }
      
        private void deleteTable(object sender, RoutedEventArgs e)
        {
            if (huidig != null)
            {
                Tafel tafel = (Tafel) huidig.Tag;
                if (!add.Contains(tafel))
                {
                    delete.Add(tafel);
                }
                else
                {
                    add.Remove(tafel);
                }

                Tables.Children.Remove(huidig);
                huidig = null;
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
            tafel.Width = 125;
            tafel.Height = 125;
            huidig.Content = tafel.Name;
            huidig.Background = new SolidColorBrush(Colors.Blue);
            huidig.Tag = tafel;
            huidig.Width = tafel.Width;
            huidig.Height = tafel.Height;
            add.Add(tafel);
            Tables.Children.Add(huidig);
            huidig.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.mouseEnter);
            huidig.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.mouseUp);
            huidig.PreviewMouseMove += new MouseEventHandler(this.mouseMove);
            huidig.Click += new RoutedEventHandler(this.setTable);
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

        private void Width_OnTextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox t = (TextBox)sender;
            if (!(Regex.IsMatch(t.Text, @"\D+?")) && !t.Text.Equals(""))
            {
                if (huidig != null)
                {
                    Tafel tafel = (Tafel)huidig.Tag;
                    int width = Convert.ToInt32(Width.Text);
                    double x =Canvas.GetLeft(huidig);
                    Console.WriteLine("current position"+x);
                    if ((width+x) <= maxWidth)
                    {
                        Console.WriteLine(MaxWidth);
                        tafel.Width = width;
                        huidig.Tag = tafel;
                        huidig.Width = width;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);
                        if (!add.Contains(tafel))
                        {
                            if (update.Contains(tafel))
                            {
                                update.Remove(tafel);
                                update.Add(tafel);
                            }
                            else
                            {
                                update.Add(tafel);
                            }
                        }
                        else
                        {
                            add.Remove(tafel);
                            add.Add(tafel);
                        }

                    }
                    else
                    {
                        Width.Text = "";
                    }

                }
                else
                {

                    Width.Text = "";


                }
            }
            else
            {
                if (huidig != null)
                {

                    if (!t.Text.Equals(""))
                    {
                        Tafel tafel = (Tafel)huidig.Tag;
                        Width.Text = tafel.Width.ToString();
                    }
                }
                else
                {

                    Width.Text = "";


                }
            }
        }

        private void Height_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (!(Regex.IsMatch(t.Text, @"\D+?")) && !t.Text.Equals(""))
            {
                if (huidig != null)
                {
                    Tafel tafel = (Tafel)huidig.Tag;
                    int height = Convert.ToInt32(Height.Text);
                    double y = Canvas.GetTop(huidig);
                    if ((height+y) <= maxHeight)
                    {
                        Console.WriteLine(maxHeight);
                        tafel.Height = height;
                        huidig.Tag = tafel;
                        huidig.Height = height;
                        Tables.Children.Remove(huidig);
                        Tables.Children.Add(huidig);
                        Canvas.SetLeft(huidig, tafel.PositionX);
                        Canvas.SetTop(huidig, tafel.PositionY);
                        if (!add.Contains(tafel))
                        {
                            if (update.Contains(tafel))
                            {
                                update.Remove(tafel);
                                update.Add(tafel);
                            }
                            else
                            {
                                update.Add(tafel);
                            }
                        }
                        else
                        {
                            add.Remove(tafel);
                            add.Add(tafel);
                        }

                    }
                    else
                    {
                        Height.Text = "";
                    }

                }
                else
                {

                    Height.Text = "";


                }
            }
            else
            {
                if (huidig != null)
                {

                    if (!t.Text.Equals(""))
                    {
                        Tafel tafel = (Tafel)huidig.Tag;
                        Height.Text = tafel.Height.ToString();
                    }
                }
                else
                {

                    Height.Text = "";


                }
            }
        }

        private void back_onclick(object sender, RoutedEventArgs e)
        {
            AdminScherm adminScherm = new AdminScherm();
            adminScherm.Show(); //Show page2
            this.Close(); //this will close Page1
        }

        private void AdminTafels_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key down");
            Key key = e.Key;
            Console.WriteLine(key.ToString());
            if (key == Key.Up)
            {
                Console.WriteLine("key up");
                if (huidig != null)
                {
                    Console.WriteLine("not null");
                    Tafel tafel = (Tafel) huidig.Tag;
                    int y = tafel.PositionY;
                    y -= 10;
                    if (y < 0)
                    {
                        y = y-y;
                    }
                    yposition.Text = y.ToString();
                }
                
            }

            if (key == Key.Down)
            {
                Console.WriteLine("key down");
                if (huidig != null)
                {
                    Console.WriteLine("not null");
                    Tafel tafel = (Tafel)huidig.Tag;
                    int y = tafel.PositionY;
                    y += 10;
                    if (y > maxHeight)
                    {
                        y = y - (y -maxHeight);
                    }
                    yposition.Text = y.ToString();
                }
            }

            if (key == Key.Left)
            {

                Console.WriteLine("key left");
                if (huidig != null)
                {
                    Console.WriteLine("not null");
                    Tafel tafel = (Tafel)huidig.Tag;
                    int x = tafel.PositionX;
                    x -= 10;
                    if (x < 0)
                    {
                        x = x - x;
                    }
                    xposition.Text = x.ToString();
                }
            }

            if (key == Key.Right)
            {
                Console.WriteLine("key right");
                if (huidig != null)
                {
                    Console.WriteLine("not null");
                    Tafel tafel = (Tafel)huidig.Tag;
                    int x = tafel.PositionX;
                    x += 10;
                    if (x > maxWidth)
                    {
                        x = x - (x - maxWidth);
                    }
                    xposition.Text = x.ToString();
                }
            }
        }
    }
}
