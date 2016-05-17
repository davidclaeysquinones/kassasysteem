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
        private OrderLineService orderlineService;
        private Tafel tafel;
        private Order orderTijdelijk;
        private IEnumerable<OrderLine> oudeOrderlines;
        private List<OrderLine> toevoegenOrderlines;
        private List<OrderLine> verwijderenOrderlines;
        private List<OrderLine> updatenOrderlines;
        public Kassa(Tafel tafel)
        {
            this.tafel = tafel;
            toevoegenOrderlines = new List<OrderLine>();
            verwijderenOrderlines = new List<OrderLine>();
            updatenOrderlines = new List<OrderLine>();
            articleService = new ArticleService();
            orderService = new OrderService();
            orderlineService = new OrderLineService();
            InitializeComponent();
            vulGrid();
            btnMin.IsEnabled = false;
            btnPlus.IsEnabled = false;
            oudeOrderlines = orderlineService.GetOpenLinesTable(tafel.Id);
            vulDataGridMetCorrecteOrderLines(oudeOrderlines);
            
        }

        public void vulGrid()
        {
            
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
            if (dataGrid.Items.Count == 0)
            {
                orderTijdelijk.Status = 0;
                orderTijdelijk.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                orderTijdelijk.TafelId = tafel.Id;
                orderTijdelijk.TafelName = tafel.Name;
            }
            if (!dataGrid.Items.Contains(b))
            {
                orderline.OrderId = orderTijdelijk.Id;
                orderline.ArticleName = article.Name;
                orderline.ArticleId = article.Id;
                orderline.Amount = 1;
                orderline.Price = article.Price;
            }


            if (dataGrid.Items.Count != 0)
            {
                Boolean aangepast = false;
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    OrderLine ol = (OrderLine)dataGrid.Items.GetItemAt(i);
                    if (ol.ArticleName.Equals(orderline.ArticleName))
                    {
                        OrderLine oude = (OrderLine)dataGrid.Items.GetItemAt(i);
                        OrderLine nieuwe = new OrderLine();
                        nieuwe.OrderId = oude.OrderId;
                        Console.WriteLine("artikelid");
                        Console.WriteLine(oude.ArticleId);
                        Console.WriteLine("orderid:");
                        Console.WriteLine(ol.OrderId);
                        nieuwe.ArticleId = oude.ArticleId;
                        nieuwe.ArticleName = oude.ArticleName;
                        nieuwe.Amount = oude.Amount + 1;
                        nieuwe.Price = oude.Price;
                        nieuwe.CreatedDate = oude.CreatedDate;
                        dataGrid.Items.RemoveAt(i);
                        dataGrid.Items.Add(nieuwe);
                        i = dataGrid.Items.Count;
                        aangepast = true;
                        if (!toevoegenOrderlines.Contains(nieuwe))
                        {
                            if(updatenOrderlines.Contains(nieuwe))
                            {
                                updatenOrderlines.Remove(nieuwe);
                                updatenOrderlines.Add(nieuwe);
                            }
                            else
                            {
                                updatenOrderlines.Add(nieuwe);
                            }
                            
                        }
                        else
                        {
                            toevoegenOrderlines.Remove(nieuwe);
                            toevoegenOrderlines.Add(nieuwe);
                        }
                    }
                }
                if (aangepast == false)
                {
                    dataGrid.Items.Add(orderline);
                    toevoegenOrderlines.Add(orderline);
                }
            }
            else if (dataGrid.Items.Count == 0)
            {
                dataGrid.Items.Add(orderline);
                toevoegenOrderlines.Add(orderline);
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
                    OrderLine tijdelijk = (OrderLine) dataGrid.Items.GetItemAt(i);
                    verwijderenOrderlines.Add(tijdelijk);
                }
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedIndex != -1)
            {
                OrderLine oude = (OrderLine)dataGrid.SelectedItem;
                OrderLine nieuwe = new OrderLine();
                nieuwe.OrderId = oude.OrderId;
                nieuwe.ArticleId = oude.ArticleId;
                nieuwe.ArticleName = oude.ArticleName;
                nieuwe.Amount = oude.Amount + 1;
                nieuwe.Price = oude.Price;
                nieuwe.CreatedDate = oude.CreatedDate;
                dataGrid.Items.Remove(oude);
                dataGrid.Items.Add(nieuwe);
                int index = dataGrid.SelectedIndex;
                if (!toevoegenOrderlines.Contains(nieuwe))
                {
                    updatenOrderlines.Add(nieuwe);
                }
                else
                {
                    toevoegenOrderlines.Remove(nieuwe);
                    updatenOrderlines.Add(nieuwe);
                }
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                OrderLine oude = (OrderLine)dataGrid.SelectedItem;
                OrderLine nieuwe = new OrderLine();
                nieuwe.OrderId = oude.OrderId;
                nieuwe.ArticleId = oude.ArticleId;
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
                nieuwe.CreatedDate = oude.CreatedDate;
                dataGrid.Items.Remove(oude);
                dataGrid.Items.Add(nieuwe);
                if(!toevoegenOrderlines.Contains(nieuwe))
                {
                    updatenOrderlines.Add(nieuwe);
                }
                else
                {
                    toevoegenOrderlines.Remove(nieuwe);
                    updatenOrderlines.Add(nieuwe);
                }
              
            }
        }

        private void btnBetalen_Click(object sender, RoutedEventArgs e)
        {
            //int id=0;
            //orderService = new OrderService();
            //orderlineService = new OrderLineService();

            //Order order = new Order();
            //order.TafelId = tafel.Id;
            //order.TafelName = tafel.Name;
            //order.CreatedDate = orderTijdelijk.CreatedDate;
            //order.Status = 1;
            //if(order != null)
            //{
            //    id = orderService.Add(order);
            //    Console.WriteLine("het orderid");
            //    Console.WriteLine(id);
            //}
            //for(int i=0; i<dataGrid.Items.Count;i++)
            //{
            //    OrderLine ol = (OrderLine)dataGrid.Items.GetItemAt(i);
            //    OrderLine nieuw = new OrderLine();
            //    nieuw.OrderId = id;
            //    nieuw.ArticleId = ol.ArticleId;
            //    Console.WriteLine("het articleid");
            //    Console.WriteLine(ol.ArticleId);
            //    nieuw.ArticleName = ol.ArticleName;
            //    nieuw.Amount = ol.Amount;
            //    nieuw.Price = ol.Price;
            //    nieuw.CreatedDate = ol.CreatedDate;

            //    orderlineService.Add(nieuw);
            //}
            

        }


        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            int id = 0;

            

            if (orderService.OrderExists(tafel.Id) == -1)
            {
                Console.WriteLine("HHHHHHHHHHHHHHHHHHH");
                orderTijdelijk.TafelId = tafel.Id;
                Console.WriteLine(orderTijdelijk.TafelId);
                orderTijdelijk.TafelName = tafel.Name;
                Console.WriteLine(orderTijdelijk.TafelName);
                orderTijdelijk.CreatedDate = orderTijdelijk.CreatedDate;
                Console.WriteLine(orderTijdelijk.CreatedDate);
                orderTijdelijk.Status = orderTijdelijk.Status;
                Console.WriteLine(orderTijdelijk.Status);

                id = orderService.Add(orderTijdelijk);
            }
            else
            {
                id = orderService.OrderExists(tafel.Id);
            }
            

            

            if (dataGrid.Items.Count != 0)
            {
                for(int i = 0; i<toevoegenOrderlines.Count; i++)
                {
                    OrderLine huidig = toevoegenOrderlines[i];
                    huidig.OrderId = id;
                    huidig.ArticleId = huidig.ArticleId;
                    huidig.ArticleName = huidig.ArticleName;
                    huidig.Amount = huidig.Amount;
                    huidig.Price = huidig.Price;
                    huidig.CreatedDate = huidig.CreatedDate;
                    orderlineService.Add(huidig);
                }

                for (int i = 0; i < updatenOrderlines.Count; i++)
                {
                    OrderLine huidig = updatenOrderlines[i];
                    huidig.OrderId = id;
                    huidig.ArticleId = huidig.ArticleId;
                    huidig.ArticleName = huidig.ArticleName;
                    huidig.Amount = huidig.Amount;
                    huidig.Price = huidig.Price;
                    huidig.CreatedDate = huidig.CreatedDate;
                    orderlineService.Update(huidig);
                }

                //for (int i = 0; i < verwijderenOrderlines.Count; i++)
                //{
                //    OrderLine huidig = verwijderenOrderlines[i];
                //    huidig.OrderId = id;
                //    huidig.ArticleId = huidig.ArticleId;
                //    huidig.ArticleName = huidig.ArticleName;
                //    huidig.Amount = huidig.Amount;
                //    huidig.Price = huidig.Price;
                //    huidig.CreatedDate = huidig.CreatedDate;
                //    if (!toevoegenOrderlines.Contains(huidig))
                //    {
                //        orderlineService.Remove(huidig);
                //    }
                //    else
                //    {
                //        toevoegenOrderlines.Remove(huidig);
                //        orderlineService.Remove(huidig);
                //    }
                //}
            }

            //if (dataGrid.Items.Count != 0)
            //{
            //    for (int i = 0; i < dataGrid.Items.Count; i++)
            //    {
            //        Console.Write("orderlines verwijderen");
            //        OrderLine ol = (OrderLine)dataGrid.Items.GetItemAt(i);
            //        Console.Write(ol.ArticleId);
            //        Console.Write(ol.ArticleName);
            //        Console.Write(ol.Amount);
            //        Console.Write(ol.Price);
            //        Console.Write(ol.OrderId);
            //        //orderlineService.Remove(ol);
            //        //dataGrid.Items.Remove(dataGrid.CurrentItem);
            //    }
            //}

            //if (oudeOrderlines == null)
            //{
            //    Order order = new Order();
            //    order.TafelId = tafel.Id;
            //    order.TafelName = tafel.Name;
            //    order.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            //    order.Status = 0;
            //    if (order != null)
            //    {
            //        id = orderService.Add(order);
            //        Console.WriteLine("het orderid");
            //        Console.WriteLine(id);
            //    }
            //    for (int i = 0; i < dataGrid.Items.Count; i++)
            //    {
            //        OrderLine ol = (OrderLine)dataGrid.Items.GetItemAt(i);
            //        OrderLine nieuw = new OrderLine();
            //        nieuw.OrderId = id;
            //        nieuw.ArticleId = ol.ArticleId;
            //        Console.WriteLine("het articleid");
            //        Console.WriteLine(ol.ArticleId);
            //        nieuw.ArticleName = ol.ArticleName;
            //        nieuw.Amount = ol.Amount;
            //        nieuw.Price = ol.Price;
            //        nieuw.CreatedDate = ol.CreatedDate;

            //        orderlineService.Add(nieuw);
            //    }
            //}

            Startscherm startscherm = new Startscherm(); //Create object of Page2
            startscherm.Show(); //Show page2
            this.Close(); //this will close Page1
        }

        private void vulDataGridMetCorrecteOrderLines(IEnumerable<OrderLine> orderlines)
        {
            int aantal = orderlines.Count();

            
            for (int i = 0; i < aantal; i++)
            {
                OrderLine oude = orderlines.ElementAt(i);
                OrderLine ol = new OrderLine();
                ol.OrderId = oude.OrderId;
                ol.ArticleId = oude.ArticleId;
                ol.ArticleName = oude.ArticleName;
                ol.Amount = oude.Amount;
                ol.Price = oude.Price;
                ol.CreatedDate = oude.CreatedDate;

                dataGrid.Items.Add(ol);
            }
            btnPlus.IsEnabled = true;
            btnMin.IsEnabled = true;
        }




    }
}






//TO DO:
//tijdens back knop, indien lijst null is, niets doen, indien lijst verandert => updaten
//keuze: niets doen, updaten, indien nieuwe elementen => deze toevoegen
//terug knop => maken van een nieuw order, mag niet!

