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
using KassaSysteem.ViewModel;

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
            OrderLine nieuweOrderline = new OrderLine();
            //OrderlineViewModel nieuweView = new OrderlineViewModel();
            Article article = (Article)b.Tag;
            if (dataGrid.Items.Count == 0)
            {
                orderTijdelijk = new Order();
                Console.WriteLine("Order wordt gemaakt");
                orderTijdelijk.Status = 0;
                orderTijdelijk.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                orderTijdelijk.TafelId = tafel.Id;
                orderTijdelijk.TafelName = tafel.Name;
                Console.WriteLine(orderTijdelijk.TafelName);
            }
            if (!dataGrid.Items.Contains(b))
            {
                nieuweOrderline.ArticleName = article.Name;
                nieuweOrderline.ArticleId = article.Id;
                nieuweOrderline.Amount = 1;
                nieuweOrderline.Price = article.Price;
                nieuweOrderline.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                nieuweOrderline.Total = nieuweOrderline.Amount * nieuweOrderline.Price;

                //nieuweView.OrderId = nieuweOrderline.OrderId;
                //nieuweView.ArticleId = nieuweOrderline.ArticleId;
                //nieuweView.ArticleName = nieuweOrderline.ArticleName;
                //nieuweView.Amount = nieuweOrderline.Amount;
                //nieuweView.Price = nieuweOrderline.Price;
                //nieuweView.CreatedDate = nieuweOrderline.CreatedDate;
                //nieuweView.Total = nieuweOrderline.Amount * nieuweOrderline.Price;
            }


            if (dataGrid.Items.Count != 0)
            {
                Boolean aangepast = false;
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    //OrderlineViewModel oudeView = (OrderlineViewModel)dataGrid.Items.GetItemAt(i);
                    OrderLine oudeOrderline = (OrderLine)dataGrid.Items.GetItemAt(i);
                    if (oudeOrderline.ArticleName.Equals(nieuweOrderline.ArticleName))
                    {
                        OrderLine huidigeLine = (OrderLine)dataGrid.Items.GetItemAt(i);
                        huidigeLine.Amount = huidigeLine.Amount + 1;
                        huidigeLine.Total = huidigeLine.Amount * huidigeLine.Price;
                        //OrderlineViewModel huidigeView = (OrderlineViewModel)dataGrid.Items.GetItemAt(i);
                        //huidigeView.Amount = huidigeView.Amount + 1;
                        //OrderLine huidigeOrderline = vanViewNaarOrderline(huidigeView);
                        //huidigeOrderline.OrderId = huidigeView.OrderId;
                        //huidigeOrderline.ArticleId = huidigeView.ArticleId;
                        //huidigeOrderline.ArticleName = huidigeView.ArticleName;
                        //huidigeView.Amount = huidigeView.Amount + 1;
                        //huidigeOrderline.Amount = huidigeView.Amount;
                        //huidigeOrderline.Price = huidigeView.Price;
                        //huidigeOrderline.CreatedDate = huidigeView.CreatedDate;
                        //huidigeView.Total = huidigeOrderline.Amount * huidigeOrderline.Price;

                        dataGrid.Items.RemoveAt(i);
                        dataGrid.Items.Add(huidigeLine);
                        i = dataGrid.Items.Count;
                        aangepast = true;
                        if (!toevoegenOrderlines.Contains(huidigeLine))
                        {
                            if(updatenOrderlines.Contains(huidigeLine))
                            {
                                updatenOrderlines.Remove(huidigeLine);
                                updatenOrderlines.Add(huidigeLine);
                            }
                            else
                            {
                                updatenOrderlines.Add(huidigeLine);
                            }
                            
                        }
                        else
                        {
                            toevoegenOrderlines.Remove(huidigeLine);
                            toevoegenOrderlines.Add(huidigeLine);
                        }
                    }
                }
                if (aangepast == false)
                {
                    dataGrid.Items.Add(nieuweOrderline);
                    toevoegenOrderlines.Add(nieuweOrderline);
                }
            }
            else if (dataGrid.Items.Count == 0)
            {
                dataGrid.Items.Add(nieuweOrderline);
                toevoegenOrderlines.Add(nieuweOrderline);
            }

            btnPlus.IsEnabled = true;
            btnMin.IsEnabled = true;
            veranderTotaalBedrag();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex >= 0)
            {
                OrderLine tijdelijk = (OrderLine)dataGrid.SelectedItem;
                dataGrid.Items.Remove(dataGrid.SelectedItem); ;
                verwijderenOrderlines.Add(tijdelijk);
            }
            veranderTotaalBedrag();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedIndex != -1)
            {
                OrderLine oude = (OrderLine)dataGrid.SelectedItem;
                oude.Amount = oude.Amount + 1;
                oude.Total = oude.Amount * oude.Price;

                dataGrid.Items.Remove(oude);
                dataGrid.Items.Add(oude);
                int index = dataGrid.SelectedIndex;
                if (!toevoegenOrderlines.Contains(oude))
                {
                    updatenOrderlines.Add(oude);
                }
                else
                {
                    toevoegenOrderlines.Remove(oude);
                    updatenOrderlines.Add(oude);
                }
            }
            veranderTotaalBedrag();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                OrderLine oude = (OrderLine)dataGrid.SelectedItem;
                if (oude.Amount > 2)
                {
                    oude.Amount = oude.Amount - 1;
                }
                else
                {
                    oude.Amount = 1;
                }
                oude.Total = oude.Amount * oude.Price;
                dataGrid.Items.Remove(oude);
                dataGrid.Items.Add(oude);
                if(!toevoegenOrderlines.Contains(oude))
                {
                    updatenOrderlines.Add(oude);
                }
                else
                {
                    toevoegenOrderlines.Remove(oude);
                    updatenOrderlines.Add(oude);
                }
              
            }
            veranderTotaalBedrag();
        }

        private void btnBetalen_Click(object sender, RoutedEventArgs e)
        {
            int orderId = 0;

            if (orderService.OrderExists(tafel.Id) == -1)
            {
                Console.WriteLine("BESTAAT NIEEEEEEEETTTTTTTTTTTTT");
                orderTijdelijk.TafelId = orderTijdelijk.TafelId;
                orderTijdelijk.TafelName = orderTijdelijk.TafelName;
                orderTijdelijk.CreatedDate = orderTijdelijk.CreatedDate;
                orderTijdelijk.Status = 1;
                Console.WriteLine(orderTijdelijk.TafelName);
                orderId = orderService.Add(orderTijdelijk);
            }
            else
            {
                Console.WriteLine("BESTAAAAAAAAAT");
                orderId = orderService.OrderExists(tafel.Id);
                orderTijdelijk = orderService.getOrderObject(orderId);
                orderTijdelijk.Status = 1;
                orderService.Update(orderTijdelijk);
            }

            if (toevoegenOrderlines.Count != 0)
            {
                foreach (var item in toevoegenOrderlines)
                {
                    item.OrderId = orderId;
                    orderlineService.Add(item);
                }
            }
            if (updatenOrderlines != null)
            {
                foreach (var item in updatenOrderlines)
                {
                    item.OrderId = orderId;
                    item.Id = orderlineService.GetId(item.OrderId, item.ArticleId);
                    Console.WriteLine("UPDATE");
                    Console.WriteLine(item.Id);
                    orderlineService.Update(item);
                }
            }
            if (verwijderenOrderlines != null)
            {
                foreach (var item in verwijderenOrderlines)
                {
                    item.OrderId = orderId;
                    item.Id = orderlineService.GetId(item.OrderId, item.ArticleId);
                    Console.WriteLine("REMOVE");
                    Console.WriteLine(item.Id);
                    orderlineService.Remove(item);
                }
            }

            
            for(int i = 0; i < dataGrid.Items.Count; i++)
            {
                dataGrid.Items.RemoveAt(i);
            }

            Detailscherm detailscherm = new Detailscherm(orderTijdelijk); //Create object of Page2
            detailscherm.Show(); //Show page2
            this.Close(); //this will close Page1
        }


        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            int orderId = 0;

            if (dataGrid.Items.Count != 0)
            {
                if (orderService.OrderExists(tafel.Id) == -1)
                {
                    Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHHHHHHHH");
                    Console.WriteLine(orderTijdelijk.TafelId);
                    orderTijdelijk.TafelId = tafel.Id;
                    Console.WriteLine(orderTijdelijk.TafelId);
                    orderTijdelijk.TafelName = tafel.Name;
                    Console.WriteLine(orderTijdelijk.TafelName);
                    orderTijdelijk.CreatedDate = orderTijdelijk.CreatedDate;
                    Console.WriteLine(orderTijdelijk.CreatedDate);
                    orderTijdelijk.Status = orderTijdelijk.Status;
                    Console.WriteLine(orderTijdelijk.Status);

                    orderId = orderService.Add(orderTijdelijk);
                }
                else
                {
                    orderId = orderService.OrderExists(tafel.Id);
                }
            }

            if(toevoegenOrderlines.Count != 0)
            {
                foreach (var item in toevoegenOrderlines)
                {
                    item.OrderId = orderId;
                    orderlineService.Add(item);
                }
            }
            if(updatenOrderlines != null)
            {
                foreach (var item in updatenOrderlines)
                {
                    item.OrderId = orderId;
                    item.Id = orderlineService.GetId(item.OrderId, item.ArticleId);
                    Console.WriteLine("UPDATE");
                    Console.WriteLine(item.Id);
                    orderlineService.Update(item);
                }
            }
            if(verwijderenOrderlines != null)
            {
                foreach (var item in verwijderenOrderlines)
                {
                    item.OrderId = orderId;
                    item.Id = orderlineService.GetId(item.OrderId, item.ArticleId);
                    Console.WriteLine("REMOVE");
                    Console.WriteLine(item.Id);
                    orderlineService.Remove(item);
                }
            }

            if(dataGrid.Items.Count == 0 && orderTijdelijk != null)
            {
                Order order = orderService.getOrderObject(orderId);
                orderService.Remove(order);
            }

            //if (dataGrid.Items.Count != 0)
            //{
            //    for (int i = 0; i < toevoegenOrderlines.Count; i++)
            //    {
            //        OrderLine huidig = toevoegenOrderlines[i];
            //        huidig.OrderId = id;
            //        huidig.ArticleId = huidig.ArticleId;
            //        huidig.ArticleName = huidig.ArticleName;
            //        huidig.Amount = huidig.Amount;
            //        huidig.Price = huidig.Price;
            //        huidig.CreatedDate = huidig.CreatedDate;
            //        orderlineService.Add(huidig);
            //    }

            //    for (int i = 0; i < updatenOrderlines.Count; i++)
            //    {
            //        OrderLine huidig = updatenOrderlines[i];
            //        huidig.OrderId = id;
            //        huidig.ArticleId = huidig.ArticleId;
            //        huidig.ArticleName = huidig.ArticleName;
            //        huidig.Amount = huidig.Amount;
            //        huidig.Price = huidig.Price;
            //        huidig.CreatedDate = huidig.CreatedDate;
            //        orderlineService.Update(huidig);
            //    }

            //    for (int i = 0; i < verwijderenOrderlines.Count; i++)
            //    {
            //        OrderLine huidig = verwijderenOrderlines[i];
            //        huidig.OrderId = id;
            //        huidig.ArticleId = huidig.ArticleId;
            //        huidig.ArticleName = huidig.ArticleName;
            //        huidig.Amount = huidig.Amount;
            //        huidig.Price = huidig.Price;
            //        huidig.CreatedDate = huidig.CreatedDate;
            //        if (!toevoegenOrderlines.Contains(huidig))
            //        {
            //            orderlineService.Remove(huidig);
            //        }
            //        else
            //        {
            //            toevoegenOrderlines.Remove(huidig);
            //            orderlineService.Remove(huidig);
            //        }
            //    }
            //}

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
            //int aantal = orderlines.Count();

            //List<OrderlineViewModel> nieuweViews = vanOrderlineNaarView(orderlines);

            //foreach (var item in orderlines)
            //{
            //    OrderlineViewModel nieuweView = new OrderlineViewModel();
            //    nieuweView.OrderId = item.OrderId;
            //    nieuweView.ArticleId = item.ArticleId;
            //    nieuweView.ArticleName = item.ArticleName;
            //    nieuweView.Amount = item.Amount;
            //    nieuweView.Price = item.Price;
            //    nieuweView.CreatedDate = item.CreatedDate;
            //    nieuweView.Total = item.Amount * item.Price;
            //    nieuweViews.Add(nieuweView);
            //}

            foreach (var item in orderlines)
            {
                dataGrid.Items.Add(item);
            }

            btnPlus.IsEnabled = true;
            btnMin.IsEnabled = true;
            veranderTotaalBedrag();
        }

        //private List<OrderlineViewModel> vanOrderlineNaarView(IEnumerable<OrderLine> orderlines)
        //{
        //    List<OrderlineViewModel> nieuweViews = new List<OrderlineViewModel>();

        //    foreach (var item in orderlines)
        //    {
        //        OrderlineViewModel nieuweView = new OrderlineViewModel();
        //        nieuweView.OrderId = item.OrderId;
        //        nieuweView.ArticleId = item.ArticleId;
        //        nieuweView.ArticleName = item.ArticleName;
        //        nieuweView.Amount = item.Amount;
        //        nieuweView.Price = item.Price;
        //        nieuweView.CreatedDate = item.CreatedDate;
        //        nieuweView.Total = item.Amount * item.Price;
        //        nieuweViews.Add(nieuweView);
        //    }
        //    return nieuweViews;
        //}

        //private OrderLine vanViewNaarOrderline(OrderlineViewModel huidigeView)
        //{
        //    OrderLine huidigeOrderline = new OrderLine();
        //    huidigeOrderline.OrderId = huidigeView.OrderId;
        //    huidigeOrderline.ArticleId = huidigeView.ArticleId;
        //    huidigeOrderline.ArticleName = huidigeView.ArticleName;
        //    huidigeOrderline.Amount = huidigeView.Amount;
        //    huidigeOrderline.Price = huidigeView.Price;
        //    huidigeOrderline.CreatedDate = huidigeView.CreatedDate;
        //    huidigeView.Total = huidigeOrderline.Amount * huidigeOrderline.Price;

        //    return huidigeOrderline;
        //}

        private void veranderTotaalBedrag()
        {
            float prijs = 0;
            if (dataGrid.Items.Count != 0)
            {
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    OrderLine line = (OrderLine)dataGrid.Items.GetItemAt(i);
                    prijs += (float)line.Total;
                }
                if(orderTijdelijk != null)
                {
                    orderTijdelijk.Total = prijs;
                }
                lblTotaal.Content = "Totaalprijs: €" + prijs;
                Console.WriteLine(prijs);
            }
        }




    }
}






//TO DO:
//order is op één of andere manier leeg als je een paar keer terug gaat tijdens het order

