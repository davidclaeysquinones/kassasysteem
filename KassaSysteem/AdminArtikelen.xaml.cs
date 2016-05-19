using System;
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
using KassaSysteem.ViewModel;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for AdminArtikelen.xaml
    /// </summary>
    public partial class AdminArtikelen : Window
    {
        private int maxColumns;
        private ArticleService articleService;
        private List<ArtikelViewModel> delete;
        private List<ArtikelViewModel> add;
        private List<ArtikelViewModel> update;

        public AdminArtikelen()
        {
            InitializeComponent();
            maxColumns = 4;
            loadData();
            delete = new List<ArtikelViewModel>();
            add = new List<ArtikelViewModel>();
            update = new List<ArtikelViewModel>();

        }

        private void loadData()
        {
            articleService = new ArticleService();
            List<Article> articles = articleService.All().ToList();
            articles.Sort();

            foreach (var item in articles)
            {
                ArtikelViewModel artikelViewModel = new ArtikelViewModel();
                artikelViewModel.Name = item.Name;
                artikelViewModel.ArtikelId = item.Id;
                artikelViewModel.Price = item.Price;
                if (item.MenuIndexX != null && item.MenuIndexY != null)
                {
                    artikelViewModel.Position = CalcPosition((int) item.MenuIndexX, (int) item.MenuIndexY);
                }
                else
                {
                    artikelViewModel.Position = Artikelen.Items.Count;
                }
                
                Artikelen.Items.Insert(artikelViewModel.Position,artikelViewModel);
            }
        }

        private void Artikelen_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void selection_changed(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid d = (DataGrid) sender;
            ItemCollection articles = d.Items;
            if (articles != null)
            {
                int selectedIndex = Artikelen.SelectedIndex;
                if (selectedIndex != -1)
                {
                    ArtikelViewModel artikelViewModel = (ArtikelViewModel) articles.GetItemAt(selectedIndex);
                    
                    if (!Price.IsFocused && !Name.IsFocused)
                    {
                        Keyboard.Focus(Name);
                    }
                    Name.Text = artikelViewModel.Name;
                    Price.Text = artikelViewModel.Price.ToString();
                }

            }


        }

        private void back(object sender, RoutedEventArgs e)
        {
            AdminScherm adminScherm = new AdminScherm();
            adminScherm.Show();
            this.Close();
        }

        private void Name_Changed(object sender, TextChangedEventArgs e)
        {
            String input = Name.Text;

            if (input!=null)
            {

                int selectedIndex = Artikelen.SelectedIndex;
                Console.WriteLine(selectedIndex + " " + Artikelen.Items.Count);
                if (selectedIndex != -1)
                {
                    ArtikelViewModel artikelViewModel= (ArtikelViewModel) Artikelen.Items.GetItemAt(selectedIndex);
                    if (artikelViewModel != null)
                    {
                        if (!add.Contains(artikelViewModel))
                        {
                            update.Remove(artikelViewModel);
                            artikelViewModel.Name = input;
                            update.Add(artikelViewModel);
                        }
                        else
                        {
                            add.Remove(artikelViewModel);
                            artikelViewModel.Name = input;
                            add.Add(artikelViewModel);
                        }
                        Artikelen.Items.Refresh();
                        Artikelen.SelectedIndex = selectedIndex;
                    }
                }
                else
                {
                    Name.Text = "";
                }
               
               
                
            }
        }

        private void Price_Changed(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(Artikelen.SelectedIndex);
            String input = Price.Text;

            if (input != null)
            {
                if (!(Regex.IsMatch(input, @"\D+?")) && !input.Equals(""))
                {
                    int selectedIndex = Artikelen.SelectedIndex;
                    Console.WriteLine(selectedIndex + " " + Artikelen.Items.Count);
                    if (selectedIndex != -1)
                    {
                        ArtikelViewModel artikelViewModel =(ArtikelViewModel)Artikelen.Items.GetItemAt(selectedIndex);
                        if (artikelViewModel != null)
                        {
                          
                            if (!add.Contains(artikelViewModel))
                            {
                                update.Remove(artikelViewModel);
                                artikelViewModel.Price = Convert.ToSingle(input);
                                update.Add(artikelViewModel);
                            }
                            else
                            {
                                add.Remove(artikelViewModel);
                                artikelViewModel.Price = Convert.ToSingle(input);
                                add.Add(artikelViewModel);
                            }
                            Artikelen.Items.Refresh();

                            Artikelen.SelectedIndex = selectedIndex;
                        }
                    }
                   
                }
                else
                {
                    if (!input.Equals(""))
                    {
                        Price.Text = "";
                    }
   
                }
            
            }
        }

        private void Delete_Article(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Artikelen.SelectedIndex;
            if (selectedIndex != -1)
            {
                ArtikelViewModel artikelViewModel = (ArtikelViewModel)Artikelen.Items.GetItemAt(selectedIndex);
                if (!add.Contains(artikelViewModel))
                {
                    delete.Add(artikelViewModel);
                }
                else
                {
                    add.Remove(artikelViewModel);
                }

                if (update.Contains(artikelViewModel))
                {
                    update.Remove(artikelViewModel);
                }
                Artikelen.Items.RemoveAt(selectedIndex);
                if (selectedIndex + 1 != Artikelen.Items.Count)
                {
                    for(int i =0;i< Artikelen.Items.Count;i++)
                    {
                        artikelViewModel = (ArtikelViewModel) Artikelen.Items.GetItemAt(i);
                        if (artikelViewModel.Position > selectedIndex)
                        {
                            Artikelen.Items.Remove(artikelViewModel);
                            artikelViewModel.Position -= 1;
                            Artikelen.Items.Insert(artikelViewModel.Position, artikelViewModel);
                        }

                    }

                }

                if (selectedIndex  != Artikelen.Items.Count)
                {
                    Artikelen.SelectedIndex = selectedIndex;
                }
                else
                {
                    Artikelen.SelectedIndex = -1;
                }

            }
          

        }

        private void Save_Changes(object sender, RoutedEventArgs e)
        {
            articleService = new ArticleService();
           

            foreach (var item in update)
            {
                Article article = new Article();
                article.Name = item.Name;
                article.Id = item.ArtikelId;
                article.Price = item.Price;
                Point coor = CalcCoordinates(item.Position);
                article.MenuIndexX = (int?)coor.X;
                article.MenuIndexY = (int?)coor.Y;
                articleService.Add(article);
            }

            foreach (var item in add)
            {
                Article article = new Article();
                article.Name = item.Name;
                article.Price = item.Price;
                Point coor = CalcCoordinates(item.Position);
                article.MenuIndexX = (int?) coor.X;
                article.MenuIndexY = (int?) coor. Y;
                articleService.Add(article);
            }

            foreach (var item in delete)
            {
                Article article = new Article();
                article.Name = item.Name;
                article.Id = item.ArtikelId;
                article.Price = item.Price;
                Point point = CalcCoordinates(item.Position);
                article.MenuIndexX = (int?)point.X;
                article.MenuIndexY = (int?)point.Y;
                articleService.Delete(article);
            }

            AdminScherm adminScherm = new AdminScherm();
            adminScherm.Show();
            this.Close();
        }

        private void New_Article(object sender, RoutedEventArgs e)
        {
            ArtikelViewModel artikelViewModel = new ArtikelViewModel();
            artikelViewModel.Name = "New Article";
            artikelViewModel.Price = 1;
            artikelViewModel.Position = Artikelen.Items.Count;
            add.Add(artikelViewModel);
            Artikelen.Items.Add(artikelViewModel);
            Artikelen.SelectedIndex = Artikelen.Items.IndexOf(artikelViewModel);
            

        }

        private int CalcPosition(int x, int y)
        {
            int position = (y*maxColumns) + x;
            Console.WriteLine(position+" position");
            return position;
        }

        private Point CalcCoordinates(int position)
        {
            Point point = new Point();
            int row = (position / maxColumns);
            point.Y = row;
            Console.WriteLine(row+" test y");
            int column = position - (row*maxColumns);
            Console.WriteLine(column+" test x");
            point.X = column;
            return point;
        }


        private void Button_Up(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Artikelen.SelectedIndex;
            if (selectedIndex != -1)
            {
                if (selectedIndex > 0)
                {
                    ArtikelViewModel artikelViewModel = (ArtikelViewModel) Artikelen.Items.GetItemAt(selectedIndex);
                 
               
                    ArtikelViewModel vorig = (ArtikelViewModel) Artikelen.Items.GetItemAt(selectedIndex - 1);
                    
                    Artikelen.Items.Remove(artikelViewModel);
                    Artikelen.Items.Remove(vorig);
                 



                    if (!add.Contains(artikelViewModel))
                    {
                        update.Remove(artikelViewModel);
                        artikelViewModel.Position -= 1;
                        update.Add(artikelViewModel);
                        Artikelen.Items.Insert(selectedIndex - 1, artikelViewModel);
                    }
                    else
                    {
                        add.Remove(artikelViewModel);
                        artikelViewModel.Position -= 1;
                        add.Add(artikelViewModel);
                        Artikelen.Items.Insert(selectedIndex - 1, artikelViewModel);
                    }

                    if (!add.Contains(vorig))
                    {
                        update.Remove(vorig);
                        vorig.Position += 1;
                        update.Add(vorig);
                        Artikelen.Items.Insert(selectedIndex, vorig);
                    }
                    else
                    {
                        add.Remove(vorig);
                        vorig.Position += 1;
                        add.Add(vorig);
                        Artikelen.Items.Insert(selectedIndex, vorig);
                    }

                    Artikelen.SelectedIndex = selectedIndex - 1;
                }
            }
        }

        private void Button_Down(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Artikelen.SelectedIndex;
            if (selectedIndex != -1)
            {
                if (selectedIndex+1 != Artikelen.Items.Count)
                {
                    ArtikelViewModel artikelViewModel = (ArtikelViewModel)Artikelen.Items.GetItemAt(selectedIndex);
                

                    ArtikelViewModel volgend = (ArtikelViewModel)Artikelen.Items.GetItemAt(selectedIndex + 1);
                  
                    Artikelen.Items.Remove(artikelViewModel);
            
                    Artikelen.Items.Remove(volgend);
                   


                    if (!add.Contains(artikelViewModel))
                    {
                        Console.WriteLine("update");
                        update.Remove(artikelViewModel);
                        artikelViewModel.Position += 1;
                        update.Add(artikelViewModel);
                        Artikelen.Items.Insert(selectedIndex + 1, artikelViewModel);
                    }
                    else
                    {
                        Console.WriteLine("add");
                        add.Remove(artikelViewModel);
                        artikelViewModel.Position += 1;
                        add.Add(artikelViewModel);
                        Artikelen.Items.Insert(selectedIndex + 1, artikelViewModel);
                    }

                    if (!add.Contains(volgend))
                    {
                        update.Remove(volgend);
                        volgend.Position -= 1;
                        update.Add(volgend);
                        Artikelen.Items.Insert(selectedIndex, volgend);
                    }
                    else
                    {
                        add.Remove(volgend);
                        volgend.Position -= 1;
                        add.Add(volgend);
                        Artikelen.Items.Insert(selectedIndex, volgend);
                    }

                    Artikelen.SelectedIndex = selectedIndex + 1;
                }
            }
        }


        private void Artikelen_OnKeyDown(object sender, KeyEventArgs e)
        {
            
            int selectedIndex = Artikelen.SelectedIndex;

            if (selectedIndex != -1)
            {

                Key key = e.Key;
                if (key == Key.Down)
                {
                    Console.WriteLine("on key down");
                    if (selectedIndex != Artikelen.Items.Count)
                    {
                        Artikelen.SelectedIndex += 1;
                    }
                }

                if (key == Key.Up)
                {
                    Console.WriteLine("on key up");
                    if (selectedIndex >0)
                    {
                        Artikelen.SelectedIndex -= 1;
                    }
                }
               
            }
        }
    }
}
