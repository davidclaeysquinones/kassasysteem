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
using Kassa.Service;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for Kassa.xaml
    /// </summary>
    public partial class Kassa : Window
    {
        private ArticleService articleService;
        public Kassa()
        {
            InitializeComponent();
            vulGrid();
        }

        public void vulGrid()
        {
            articleService = new ArticleService();
            int aantal = articleService.getAantal();
        }
    }
}
