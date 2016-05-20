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
        private IEnumerable<OrderLine> oudeOrderlines; //lijst met orderlines van een openstaand order van een tafel
        private List<OrderLine> toevoegenOrderlines; //lijst met toe te voegen orderlines aan de database
        private List<OrderLine> verwijderenOrderlines; //lijst met te verwijderen orderlines uit de database
        private List<OrderLine> updatenOrderlines; //lijst met up te daten orderlines uit de database
        public Kassa(Tafel tafel)
        {
            this.tafel = tafel;
            InitializeComponent();
            vulGrid();
            toevoegenOrderlines = new List<OrderLine>();
            verwijderenOrderlines = new List<OrderLine>();
            updatenOrderlines = new List<OrderLine>();
            articleService = new ArticleService();
            orderService = new OrderService();
            orderlineService = new OrderLineService();
            btnMin.IsEnabled = false;
            btnPlus.IsEnabled = false;
            oudeOrderlines = orderlineService.GetOpenLinesTable(tafel.Id);
            vulDataGridMetCorrecteOrderLines(oudeOrderlines);
            
        }

        //Vult alle artikelen in de grid aan, op row 0 en column 0
        public void vulGrid()
        {
            
            int atlArtikelen = articleService.getAantal();
            int maxWidth = 4;
            int aantalRijen = atlArtikelen / maxWidth;
            for (int i = 0; i<=aantalRijen+1; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1.0, GridUnitType.Star);
                mainGrid.RowDefinitions.Add(rd);
            }

            IEnumerable<Article> articles = articleService.All();

            foreach (var item in articles)
            {
               
                Button b = new Button();
                b.Background = Brushes.Gray;
                b.Margin = new Thickness(10);
                b.Width = 100;
                b.Height = 100;
                b.Content = item.Name +"\n €"+ item.Price;
                b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
                b.Tag = item;
                if (item.MenuIndexY != null && item.MenuIndexX!=null)
                {
                    Grid.SetRow(b, (int)item.MenuIndexY);
                    Grid.SetColumn(b, (int)item.MenuIndexX);
                }
                else
                {
                    var y = mainGrid.RowDefinitions.Count;
                    var x = mainGrid.ColumnDefinitions.Count;

                    if (x == maxWidth)
                    {
                        y++;
                        x = 0;
                    }
                    Grid.SetRow(b, y);
                    Grid.SetColumn(b, x);
                }


                mainGrid.Children.Add(b);
            }
        }

        //Als men op een artikel drukt wordt er een orderline gemaakt.
        //Indien de datagrid nog leeg is wordt er een order gemaakt.
        //Er wordt gekeken als deze orderline reeds bestaat in de datagrid, indien deze bestaat wordt het aantal met 1 verhoogt
        //Indien deze niet wordt bestaat wordt er een nieuwe orderline gemaakt en in de datagrid gestoken.
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            OrderLine nieuweOrderline = new OrderLine();

            Article article = (Article)b.Tag;
            if (dataGrid.Items.Count == 0)
            {
                orderTijdelijk = new Order();
                orderTijdelijk.Status = 0;
                orderTijdelijk.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                orderTijdelijk.TafelId = tafel.Id;
                orderTijdelijk.TafelName = tafel.Name;
            }
            if (!dataGrid.Items.Contains(b))
            {
                nieuweOrderline.ArticleName = article.Name;
                nieuweOrderline.ArticleId = article.Id;
                nieuweOrderline.Amount = 1;
                nieuweOrderline.Price = article.Price;
                nieuweOrderline.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                nieuweOrderline.Total = nieuweOrderline.Amount * nieuweOrderline.Price;
            }


            if (dataGrid.Items.Count != 0)
            {
                Boolean aangepast = false;
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {

                    OrderLine oudeOrderline = (OrderLine)dataGrid.Items.GetItemAt(i);
                    if (oudeOrderline.ArticleName.Equals(nieuweOrderline.ArticleName))
                    {
                        OrderLine huidigeLine = (OrderLine)dataGrid.Items.GetItemAt(i);
                        huidigeLine.Amount = huidigeLine.Amount + 1;
                        huidigeLine.Total = huidigeLine.Amount * huidigeLine.Price;


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

        //Indien er een item uit de datagrid is geselecteerd wordt deze verwijderd uit de datagrid.
        //Deze wordt ook in een lijst gestoken met items die moeten verwijderd worden uit de database indien deze er in zouden zitten.
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

        //Indien er een item uit de datagrid is geselecteerd wordt de orderline uit de gatagrid gehaald en wordt het aantal met 1 verhoogd.
        //Dit item wordt ook in de lijst gestoken met items die een update moeten ondergaan in de database.
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

        //Indien er een item uit de datagrid is geselecteerd wordt de orderline uit de gatagrid gehaald en wordt het aantal met 1 verlaagd.
        //Dit item wordt ook in de lijst gestoken met items die een update moeten ondergaan in de database.
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

        //Eerst wordt er gekeken als er wel items in de datagrid zitten, als er niets in zit gebeurt er niets.
        //Vervolgens wordt er gekeken als er een openstaand order bestaat in de database bij deze tafel. 
        //Aan de hand van het antwoord wordt ofwel het id opgevraagd van dit order, ofwel wordt dit order toegevoegd aan de database
        //Vervolgens worden de 3 lijsten overlopen, die ofwel items verwijderen, updaten of toevoegen aan de database.
        private void btnBetalen_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.Items.Count != 0)
            {
                int orderId = 0;

                if (orderService.OrderExists(tafel.Id) == -1)
                {
                    orderTijdelijk.TafelId = orderTijdelijk.TafelId;
                    orderTijdelijk.TafelName = orderTijdelijk.TafelName;
                    orderTijdelijk.CreatedDate = orderTijdelijk.CreatedDate;
                    orderTijdelijk.Status = 1;
                    veranderTotaalBedrag();
                    orderId = orderService.Add(orderTijdelijk);
                }
                else
                {
                    orderId = orderService.OrderExists(tafel.Id);
                    orderTijdelijk = orderService.getOrderObject(orderId);
                    orderTijdelijk.Status = 1;
                    veranderTotaalBedrag();
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
                        orderlineService.Update(item);
                    }
                }
                if (verwijderenOrderlines != null)
                {
                    foreach (var item in verwijderenOrderlines)
                    {
                        item.OrderId = orderId;
                        item.Id = orderlineService.GetId(item.OrderId, item.ArticleId);
                        orderlineService.Remove(item);
                    }
                }


                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    dataGrid.Items.RemoveAt(i);
                }

                Detailscherm detailscherm = new Detailscherm(orderTijdelijk);
                detailscherm.Show();
                this.Close();
            }
        }

        //Tijdens het teruggaan moet alle data die reeds is ingegeven opgeslagen worden. 
        //Anders gaat deze verloren als men later terug komt naar dit openstaand order.
        //Ook hier wordt er gekeken als het order reeds bestaat in de database, 
        //aan de hand van dit antwoord wordt ofwel het id opgevraagd van dit order, ofwel wordt dit order in de database opgeslagen.
        //Ook hier moeten alle 3 de lijsten overlopen worden, die ofwel items verwijderen, updaten of toevoegen aan de database.
        //Tot slot wordt ook het order verwijderd indien er een order is gemaakt en er geen items meer in de datagrid zitten.
        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            int orderId = 0;

            if (dataGrid.Items.Count != 0 && orderService.OrderExists(tafel.Id) == -1)
            {
                orderTijdelijk.TafelId = tafel.Id;
                orderTijdelijk.TafelName = tafel.Name;
                orderTijdelijk.CreatedDate = orderTijdelijk.CreatedDate;
                orderTijdelijk.Status = orderTijdelijk.Status;

                orderId = orderService.Add(orderTijdelijk);
            }
            else
            {
                orderId = orderService.OrderExists(tafel.Id);
            }

            if (toevoegenOrderlines.Count != 0)
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
                    orderlineService.Update(item);
                }
            }
            if(verwijderenOrderlines != null)
            {
                foreach (var item in verwijderenOrderlines)
                {
                    item.OrderId = orderId;
                    item.Id = orderlineService.GetId(item.OrderId, item.ArticleId);
                    orderlineService.Remove(item);
                }
            }

            if(dataGrid.Items.Count == 0 && orderTijdelijk != null)
            {
                Order order = orderService.getOrderObject(orderId);
                orderService.Remove(order);
            }

     

            Startscherm startscherm = new Startscherm();
            startscherm.Show();
            this.Close();
        }

        //Als men reeds een order heeft gemaakt, en deze werd nog niet betaald, worden alle orderlines van dit openstaand order
        //opgehaald en getoont in de datagrid.
        private void vulDataGridMetCorrecteOrderLines(IEnumerable<OrderLine> orderlines)
        {
 

            foreach (var item in orderlines)
            {
                dataGrid.Items.Add(item);
            }

            btnPlus.IsEnabled = true;
            btnMin.IsEnabled = true;
            veranderTotaalBedrag();
        }

        //Telkens er een wijziging gebeurd in de database moet de totaalprijs voor dit order berekend worden.
        private void veranderTotaalBedrag()
        {
            float prijs = 0;
            if (dataGrid.Items.Count != 0)
            {
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    OrderLine line = (OrderLine)dataGrid.Items.GetItemAt(i);
                    if (line.Total != null)
                    {
                        prijs += (float)line.Total;
                    }
     
                }
                if(orderTijdelijk != null)
                {
                    orderTijdelijk.Total = prijs;
                }
                lblTotaal.Content = "Totaalprijs: €" + prijs;
            }
        }


        private void DataGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}