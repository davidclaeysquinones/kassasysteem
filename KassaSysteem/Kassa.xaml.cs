using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private OrderService orderService;
        private Tafel tafel;
        private Order orderTijdelijk;
        public Kassa(Tafel tafel)
        {
            this.tafel = tafel;
            InitializeComponent();
            vulGrid();
            btnMin.IsEnabled = false;
            btnPlus.IsEnabled = false;
        }

        public void vulGrid()
        {
            articleService = new ArticleService();
            int atlArtikelen = articleService.getAantal();

            int aantalRijen = atlArtikelen / 4;
            for (int i = 0; i<=aantalRijen+1; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1.0, GridUnitType.Star);
                mainGrid.RowDefinitions.Add(rd);
            }

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
            OrderLine orderline = new OrderLine();
            Article article = (Article)b.Tag;
            orderTijdelijk = new Order();
            if (dataGrid.Items.Count == -1)
            {
                orderTijdelijk.Status = 0;
                orderTijdelijk.CreatedDate = new DateTime(DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Year,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
                orderTijdelijk.TafelId = tafel.Id;
                orderTijdelijk.TafelName = tafel.Name;
            }
            if (!dataGrid.Items.Contains(b))
            {
                orderline.ArticleName = article.Name;
                orderline.Amount = 1;
                orderline.Price = article.Price;
                orderline.OrderId = orderTijdelijk.Id;
            }
            

            if (dataGrid.Items.Count != 0)
            {
                Boolean aangepast = false;
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    OrderLine ol = (OrderLine) dataGrid.Items.GetItemAt(i);
                    if (ol.ArticleName.Equals(orderline.ArticleName))
                    {
                        OrderLine oude = (OrderLine)dataGrid.Items.GetItemAt(i);
                        OrderLine nieuwe = new OrderLine();
                        nieuwe.ArticleName = oude.ArticleName;
                        nieuwe.Amount = oude.Amount + 1;
                        nieuwe.Price = oude.Price;
                        dataGrid.Items.RemoveAt(i);
                        dataGrid.Items.Add(nieuwe);
                        i = dataGrid.Items.Count;
                        aangepast = true;
                    }
                }
                if (aangepast == false)
                {
                    dataGrid.Items.Add(orderline);
                }
            }
            else if (dataGrid.Items.Count == 0)
            {
                dataGrid.Items.Add(orderline);
            }

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

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedIndex != -1)
            {
                OrderLine oude = (OrderLine)dataGrid.SelectedItem;
                OrderLine nieuwe = new OrderLine();
                nieuwe.ArticleName = oude.ArticleName;
                nieuwe.Amount = oude.Amount + 1;
                nieuwe.Price = oude.Price;
                dataGrid.Items.Remove(oude);
                dataGrid.Items.Add(nieuwe);
                int index = dataGrid.SelectedIndex;
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                OrderLine oude = (OrderLine)dataGrid.SelectedItem;
                OrderLine nieuwe = new OrderLine();
                nieuwe.ArticleName = oude.ArticleName;
                if(oude.Amount > 2)
                {
                    nieuwe.Amount = oude.Amount - 1;
                }
                else
                {
                    nieuwe.Amount = 1;
                }
                nieuwe.Price = oude.Price;
                dataGrid.Items.Remove(oude);
                dataGrid.Items.Add(nieuwe);
            }
        }

        private void btnBetalen_Click(object sender, RoutedEventArgs e)
        {
            orderService = new OrderService();

            Order order = new Order();
            order.TafelId = tafel.Id;
            order.TafelName = tafel.Name;
            order.CreatedDate = orderTijdelijk.CreatedDate;
            order.Status = 1;
            if(order != null)
            {
                orderService.Add(order);
            }
            

        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            Startscherm start = new Startscherm();
            start.Show(); //Show page2
            this.Close(); //this will close Page1
        }
    }
}
