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
    /// Interaction logic for Detailscherm.xaml
    /// </summary>
    public partial class Detailscherm : Window
    {
        private Order order;
        private OrderService orderService;
        private OrderLineService orderlineService;
        private IEnumerable<OrderLine> orderlines;
        private Kassa kassa;
        public Detailscherm(Order order)
        {
            this.order = order;
            orderService = new OrderService();
            orderlineService = new OrderLineService();
            InitializeComponent();
            vulGrid();
        }

        public void vulGrid()
        {
            Console.WriteLine(order.TafelName);
            lblTafelnaam.Content = order.TafelName;
            orderlines = orderlineService.GetAllOrderlinesFromOrder(order.Id);

            List<OrderlineViewModel> nieuweViews = vanOrderlineNaarView(orderlines);

            foreach (var item in nieuweViews)
            {
                dataGrid.Items.Add(item);
            }
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

        private void btnAfronden_Click(object sender, RoutedEventArgs e)
        {
            Startscherm startscherm = new Startscherm();
            startscherm.Show();
            this.Close();
        }
    }
}
