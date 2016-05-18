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
using Kassa.Model;
using Kassa.Service;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for AdminArtikelen.xaml
    /// </summary>
    public partial class AdminArtikelen : Window
    {
        private ArticleService articleService;
        private List<Article> delete;
        private List<Article> add;
        private List<Article> update;

        public AdminArtikelen()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            articleService = new ArticleService();
            List<Article> articles = articleService.All().ToList();

            foreach (var item in articles)
            {
                Artikelen.Items.Add(item);
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
                    Article article = (Article)articles.GetItemAt(selectedIndex);
                    Name.Text = article.Name;
                    Price.Text = article.Price.ToString();
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
                Article article = (Article) Artikelen.Items.GetItemAt(selectedIndex);
                article.Price = Convert.ToSingle(Price.Text);
                article.Name = Name.ToString();
                if (!add.Contains(article))
                {
                    update.Remove(article);
                    update.Add(article);
                }
            }
        }

        private void Price_Changed(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(Artikelen.SelectedIndex);
        }

        private void Delete_Article(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Artikelen.SelectedIndex;
            Artikelen.SelectedIndex = -1;
            Artikelen.Items.RemoveAt(selectedIndex);
        }

        private void Save_Changes(object sender, RoutedEventArgs e)
        {
            articleService = new ArticleService();
            foreach (var item in delete)
            {
                articleService.Delete(item);
            }

            foreach (var item in update)
            {
                articleService.Update(item);
            }

            foreach (var item in add)
            {
                articleService.Add(item);
            }
        }
    }
}
