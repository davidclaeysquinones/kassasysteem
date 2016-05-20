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
            berekenTotaal();
        }

        public void vulGrid()
        {
            Console.WriteLine(order.TafelName);
            Console.WriteLine(order.Id);
            lblTafelnaam.Content = order.TafelName;
            orderlines = orderlineService.GetAllOrderlinesFromOrder(order.Id);

            foreach (var item in orderlines)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void btnAfronden_Click(object sender, RoutedEventArgs e)
        {
            Startscherm startscherm = new Startscherm();
            startscherm.Show();
            this.Close();
        }

        private void berekenTotaal()
        {
            lblTotaalBedrag.Content = "Totaalbedrag: €" + order.Total;
        }

        private void DataGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
