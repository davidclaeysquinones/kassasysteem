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
    /// Interaction logic for Detailscherm.xaml
    /// </summary>
    public partial class Detailscherm : Window
    {
        private Order order;
        private OrderService orderService;
        private OrderLineService orderlineService;
        private IEnumerable<OrderLine> orderlines;
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
            lblTafelnaam.Content = "Tafel: " + order.TafelName;
            orderlines = orderlineService.GetAllOrderlinesFromOrder(order.Id);

            foreach (var item in orderlines)
            {
                dataGrid.Items.Add(item);
            }

            //for (int i = 0; i < aantal; i++)
            //{
            //    OrderLine oude = orderlines.ElementAt(i);
            //    OrderLine ol = new OrderLine();
            //    ol.OrderId = oude.OrderId;
            //    ol.ArticleId = oude.ArticleId;
            //    ol.ArticleName = oude.ArticleName;
            //    ol.Amount = oude.Amount;
            //    ol.Price = oude.Price;
            //    ol.CreatedDate = oude.CreatedDate;

            //    lvLijst.Items.Add(ol);
            //}
        }
    }
}
