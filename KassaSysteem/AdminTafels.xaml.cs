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
    /// Interaction logic for AdminTafels.xaml
    /// </summary>
    public partial class AdminTafels : Window
    {
        private TafelService tafelService;
        public AdminTafels()
        {
            tafelService=new TafelService();
            InitializeComponent();
            VullTafels();
        }

        private void VullTafels()
        {
            tafelService = new TafelService();


            IEnumerable<Tafel> tafels = tafelService.All();

            foreach (var item in tafels)
            {
                Button b = new Button();
                b.Width = item.Width;
                b.Height = item.Height;
                b.Background = new SolidColorBrush(Colors.Red);
                b.AllowDrop = true;
                b.Content = item.Name;
                b.Tag = item;
                Canvas.SetLeft(b, item.PositionX);
                Canvas.SetTop(b, item.PositionY);
                Tables.Children.Add(b);
            }
        }




    }
}
