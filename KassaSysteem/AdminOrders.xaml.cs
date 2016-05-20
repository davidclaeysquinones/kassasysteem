using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            vulScherm();
        }

        //Vult alle orders in de linkerdatagrid
        private void vulScherm()
        {
            foreach (var item in alleOrders)
            {
                if (!dataGrid.Items.Contains(item))
                {
                    dataGrid.Items.Add(item);
                }
               
            }
            btnTerug.Visibility = Visibility.Collapsed;
            dataGridLines.Visibility = Visibility.Collapsed;
            dataGridMaand.Visibility = Visibility.Collapsed;
        }

        //Bij een dubbelklik op een artikel zullen alle orderlines opgehaald worden 
        //uit de database en ingevuld worden in de rechter datagrid.
        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dataGridLines.Items.Clear();

            if(dataGrid.SelectedIndex != -1)
            {
                Order order = (Order)dataGrid.SelectedItem;
                int id = order.Id;
                Console.WriteLine(id);
                IEnumerable<OrderLine> orderlines = orderlineService.GetAllOrderlinesFromOrder(id);

                foreach (var item in orderlines)
                {
                    dataGridLines.Items.Add(item);
                    Console.WriteLine(item.ArticleName);
                }
                lblTotaalBedrag.Content = "Totaalprijs: €" + order.Total;
            }

            dataGridLines.Visibility = Visibility.Visible;
        }

        //Methode om de beginedit uit te schakelen tijdens dubbelklik in de datagrid
        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        //Bij het klikken op de "Toon" button zullen de 2 textbox'en gevalideerd worden en zal de input 
        //omgezet worden naar een Datetime om zo te kunnen vergelijke. met waarden uit de database.
        //Vervolgens worden alle order opgehaald die tussen die 2 datums liggen.
        private void btnToonMaand_Click(object sender, RoutedEventArgs e)
        {
            string boxBegin = txtBoxBegin.Text;
            string boxEind = txtBoxEind.Text;

            if (!(boxBegin.Equals("")) && !(boxEind.Equals("")))
            {
                if((Regex.IsMatch(boxBegin, @"^\d{4}[-/.]\d{1,2}[-/.]\d{1,2}$")) && (Regex.IsMatch(boxEind, @"^\d{4}[-/.]\d{1,2}[-/.]\d{1,2}$")))
                {
                    float totaalPrijs = 0;

                    dataGrid.Visibility = Visibility.Collapsed;
                    dataGridLines.Visibility = Visibility.Collapsed;
                    lblTitel.Visibility = Visibility.Collapsed;
                    btnToonMaand.Visibility = Visibility.Collapsed;
                    lblBeginMaand.Visibility = Visibility.Collapsed;
                    lblEindeMaand.Visibility = Visibility.Collapsed;
                    string dateBegin = txtBoxBegin.Text;
                    DateTime begin = Convert.ToDateTime(dateBegin);
                    string dateEind = txtBoxEind.Text;
                    DateTime eind = Convert.ToDateTime(dateEind);
                    txtBoxBegin.Visibility = Visibility.Collapsed;
                    txtBoxEind.Visibility = Visibility.Collapsed;
                    dataGridMaand.Visibility = Visibility.Visible;
                    btnTerug.Visibility = Visibility.Visible;
                    btnVorigeAdmin.Visibility = Visibility.Collapsed;

                    IEnumerable<Order> orders = orderService.getOrderMonth(begin, eind);

                    foreach (var item in orders)
                    {
                        dataGridMaand.Items.Add(item);
                        if (item.Total != null)
                        {
                            totaalPrijs += (float)item.Total;
                        }
                    }
                    lblTotaalBedrag.Content = "Totaalbedrag voor deze periode: €" + totaalPrijs;
                }
                else
                {
                    lblError.Content = "Datum moet van formaat yyyy/mm/dd zijn.";
                }
            }
            else
            {
                lblError.Content = "Inputvelden moeten ingevuld worden.";
            }
            
        }

        private void btnTerug_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.Visibility = Visibility.Visible;
            dataGridLines.Visibility = Visibility.Visible;
            lblTitel.Visibility = Visibility.Visible;
            btnToonMaand.Visibility = Visibility.Visible;
            lblBeginMaand.Visibility = Visibility.Visible;
            lblEindeMaand.Visibility = Visibility.Visible;
            txtBoxBegin.Visibility = Visibility.Visible;
            txtBoxEind.Visibility = Visibility.Visible;
            dataGridMaand.Visibility = Visibility.Visible;
            btnVorigeAdmin.Visibility = Visibility.Visible;
            lblTotaalBedrag.Content = "";

            vulScherm();
        }

        private void btnVorigeAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminScherm adminscherm = new AdminScherm();
            adminscherm.Show();
            this.Close();
        }
    }
}
