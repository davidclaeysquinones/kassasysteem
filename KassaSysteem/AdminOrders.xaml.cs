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
using KassaSysteem.ViewModel;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for AdminOrders.xaml
    /// </summary>
    public partial class AdminOrders : Window
    {
        private IEnumerable<Order> alleOrders;
        private OrderService orderService;
        private OrderLineService orderlineService;
        public AdminOrders()
        {
            InitializeComponent();
            orderService = new OrderService();
            orderlineService = new OrderLineService();
            alleOrders = orderService.GetAllOrders();
            vulScherm(alleOrders);
            dataGridLines.Visibility = Visibility.Collapsed;
        }

        private void vulScherm(IEnumerable<Order> alleOrders)
        {
            foreach (var item in alleOrders)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dataGridLines.Items.Clear();

            Order order = (Order)dataGrid.SelectedItem;
            int id = order.Id;
            Console.WriteLine(id);
            IEnumerable<OrderLine> orderlines = orderlineService.GetAllOrderlinesFromOrder(id);
            List<OrderlineViewModel> nieuweViews = vanOrderlineNaarView(orderlines);

            foreach (var item in nieuweViews)
            {
                dataGridLines.Items.Add(item);
                Console.WriteLine(item.ArticleName);
            }

            berekenTotaalBedrag();

            dataGridLines.Visibility = Visibility.Visible;
        }

        private List<OrderlineViewModel> vanOrderlineNaarView(IEnumerable<OrderLine> orderlines)
        {
            List<OrderlineViewModel> nieuweViews = new List<OrderlineViewModel>();

            foreach (var item in orderlines)
            {
                OrderlineViewModel nieuweView = new OrderlineViewModel();
                nieuweView.OrderId = item.OrderId;
                nieuweView.ArticleId = item.ArticleId;
                nieuweView.ArticleName = item.ArticleName;
                nieuweView.Amount = item.Amount;
                nieuweView.Price = item.Price;
                nieuweView.CreatedDate = item.CreatedDate;
                nieuweView.Total = item.Amount * item.Price;
                nieuweViews.Add(nieuweView);
            }
            return nieuweViews;
        }

        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnVerwijderOrder_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedIndex != -1)
            {
                Order order = (Order)dataGrid.SelectedItem;
                orderService.Remove(order);
                dataGrid.Items.RemoveAt(dataGridLines.SelectedIndex);
            }
        }

        private void berekenTotaalBedrag()
        {
            float prijs = 0;
            for (int i = 0; i < dataGridLines.Items.Count; i++)
            {
                OrderlineViewModel line = (OrderlineViewModel)dataGridLines.Items.GetItemAt(i);
                prijs += line.Total;
            }
            lblTotaalBedrag.Content = "Totaalprijs: €" + prijs;
        }
    }
}
