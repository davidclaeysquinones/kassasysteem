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
        //list met te verwijderen tafels
        private List<Tafel> delete;
        //list met up te daten tafels
        private List<Tafel> update;
        //list met te toe te voegen tafels
        private List<Tafel> add;
        private static int maxWidth = 1000;
        private static int maxHeight = 420;



        public AdminTafels()
        {
            tafelService=new TafelService();
            InitializeComponent();
            //canvas vullen
            VullTafels();
           
            //muis wordt niet ingedrukt bij de opstart
            pressed = false;
            //initialiseren van lists
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
                b.Width = item.Width;
                b.Height = item.Height;
                b.Background = new SolidColorBrush(Colors.Red);
                b.AllowDrop = true;
                b.PreviewMouseLeftButtonDown+= new MouseButtonEventHandler(this.mouseEnter);
                b.PreviewMouseLeftButtonUp+= new MouseButtonEventHandler(this.mouseUp);
                b.PreviewMouseMove+=new MouseEventHandler(this.mouseMove);
                b.Click += new RoutedEventHandler(this.setTable);
                b.Content = item.Name;
                b.Tag = item;
                
                //item in canvas op juiste positie zetten
                Canvas.SetLeft(b, item.PositionX);
                Canvas.SetTop(b, item.PositionY);
                Tables.Children.Add(b);
            }
        }

        //wordt opgeroepen wanneer de muis gedurende lange tijd wordt ingedrukt op een tafel
        private void mouseEnter(Object sender,MouseEventArgs args)
        {
            Console.WriteLine("mouse enter");
            //boolean pressen op true zetten
            pressed = true;

            //ingedrukte button
            Button b = (Button)sender;
            Tafel item = (Tafel)b.Tag;
            huidig = b;
            //huidig item van kleur veranderen
            huidig.Background = new SolidColorBrush(Colors.Blue);
            xposition.Text = item.PositionX.ToString();
            yposition.Text = item.PositionY.ToString();
            Height.Text = item.Height.ToString();
            Width.Text = item.Width.ToString();
            artikelNaam.Text = item.Name;
            //andere items in hun originele kleur zetten
            foreach (Button itemB in Tables.Children)
            {
                //nagaan of het huidig item niet het geselecteerde item is
                if (itemB != huidig)
                {
                    itemB.Background = new SolidColorBrush(Colors.Red);
                }
            }
        }
        //wordt opgeroepen wanneer de muis niet meer wordt ingedrukt op een tafel
        private void mouseUp(Object sender, MouseEventArgs args)
        {

                Console.WriteLine("mouse up");
                //boolean op false zetten
                pressed = false;


        }
        //wordt opgeroepen wanneer de muis beweegt over een tafel
        private void mouseMove(object sender, MouseEventArgs args)
        {
            //controleren of de muis wordt ingedrukt over de tafel
            if (pressed)
            {
                if (huidig != null)
                {
                    Console.WriteLine("item is pressed");
                    Button b = huidig;
                    //geselecteerde tafel ophalen
                    Tafel tafel = (Tafel)b.Tag;

                    //positie van item achterhalen
                    Point input = args.GetPosition(Tables);
                    double xmove = input.X;
                    double ymove = input.Y;

                    //huidig item van canvas werwijderen
                    Tables.Children.Remove(b);
                    if (xmove >= 0)
                    {
                        if (xmove <= maxWidth)
                        {
                            //positie van huidig item updaten
                            tafel.PositionX = (int)xmove;
                            Canvas.SetLeft(b, tafel.PositionX);
                        }
                        else
                        {
                            //positie van huidig item updaten
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

                    //huidig item terug toevoegen aan canvas
                    Tables.Children.Add(b);

                    xposition.Text = tafel.PositionX.ToString();
                    yposition.Text = tafel.PositionY.ToString();
                    //huidig item updaten
                    huidig = b;
                }

            }
        }
        //wordt opgeroepen tijdens een click event
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
        //wordt opgeroepen wanneer de textbox x-position wijzigt
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
        //wordt opgeroepen wanneer de textbox y-position wijzigt
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
        //slaat veranderingen op
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
        //verwijdert een tafel uit de lijst
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
        //voegt een tafel aan de lisjt toe
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
        //wordt opgeroepen wanneer de textbox naam wijzigt
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
        //wordt opgeroepen wanneer de textbox width wijzigt
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
        //wordt opgeroepen wanneer de textbox height wijzigt
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
        //terug naar vorig scherm
        private void back_onclick(object sender, RoutedEventArgs e)
        {
            AdminScherm adminScherm = new AdminScherm();
            adminScherm.Show(); //Show page2
            this.Close(); //this will close Page1
        }
        //wordt opgeroepen wanneer een toets wordt ingedrukt
        private void AdminTafels_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key down");
            if (!pressed)
            {
                Key key = e.Key;
                Console.WriteLine(key.ToString());
                if (key == Key.Up)
                {
                    Console.WriteLine("key up");
                    if (huidig != null)
                    {
                        Console.WriteLine("not null");
                        Tafel tafel = (Tafel)huidig.Tag;
                        int y = tafel.PositionY;
                        y -= 10;
                        if (y < 0)
                        {
                            y = y - y;
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
                            y = y - (y - maxHeight);
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
            pressed = false;
        }
    }
}
