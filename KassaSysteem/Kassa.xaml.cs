using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Kassa.xaml
    /// </summary>
    public partial class Kassa : Window
    {
        private ArticleService articleService;
        public Kassa()
        {
            InitializeComponent();
            vulGrid();
            btnMin.IsEnabled = false;
            btnPlus.IsEnabled = false;
        }

        public void vulGrid()
        {
            articleService = new ArticleService();
            int atlArtikelen = articleService.getAantal();


            IEnumerable<Article> articles = articleService.All();
            int row = 0;
            int column = -1;
            foreach (var item in articles)
            {
                if (column == 3)
                {
                    row++;
                    column = -1;
                }
                column++;
                Button b = new Button();
                b.Background = Brushes.Gray;
                b.Margin = new Thickness(10);
                b.Width = 100;
                b.Height = 100;
                b.Content = item.Name +"\n €"+ item.Price;
                b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
                b.Tag = item;
                Grid.SetRow(b, row);
                Grid.SetColumn(b, column);
                mainGrid.Children.Add(b);


            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Article article = (Article)b.Tag;

            //for (int i = 0; i < dataGrid.Items.Count; i++)
            //{
            //    if (article == dataGrid.Items.Comparer)
            //    {
            //        int row = dataGrid.SelectedIndex;
            //        int column = 1;
            //        dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[row], dataGrid.Columns[column]);
            //        string tekst = dataGrid.CurrentCell.ToString();
            //        int aantal = Convert.ToInt16(tekst);
            //        aantal++;
            //        string nieuweText = aantal.ToString();
            //        dataGrid.SetValue(ContentProperty, nieuweText);

            //    }
            //    else
            //    {
            //        dataGrid.Items.Add(article);
            //    }
            //}
            dataGrid.Items.Add(article);
            btnPlus.IsEnabled = true;
            btnMin.IsEnabled = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex >= 0)
            {
                for (int i = 0; i <= dataGrid.SelectedItems.Count; i++)
                {
                    dataGrid.Items.Remove(dataGrid.SelectedItems[i]);
                }
            }
        }
    }
}
