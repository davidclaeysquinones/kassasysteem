﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kassa.Model;
using Kassa.Service;

namespace KassaSysteem
{
    /// <summary>
    /// Interaction logic for Startscherm.xaml
    /// </summary>
    public partial class Startscherm : Window
    {
        private TafelService tafelService;
        public Startscherm()
        {
            InitializeComponent();
            vulCanvas();
        }

        //Vult alle tafels aan in de canvas
        public void vulCanvas()
        {
            tafelService = new TafelService();
            int atlTafels = tafelService.getAantal();

            IEnumerable<Tafel> tafels = tafelService.All();

            foreach (var item in tafels)
            {
                Button b = new Button();
                b.Width = item.Width;
                b.Height = item.Height;
                b.Background = new SolidColorBrush(Colors.Red);
                b.Click += new RoutedEventHandler(this.ButtonBase_OnClick);
                b.Content = item.Name;
                b.Tag = item;
                Canvas.SetLeft(b, item.PositionX);
                Canvas.SetTop(b, item.PositionY);
                canvas.Children.Add(b);
            }
        }

        //Tijdens het klikken op de tafel wordt een tafel object gemaakt en doorgegeven naar het kassa scherm.
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Tafel tafel = (Tafel)b.Tag;
            Kassa kassa = new Kassa(tafel);
            kassa.Show();
            this.Close();
        }

        private void GoToAdmin(object sender, RoutedEventArgs e)
        {
            PinScherm pinScherm = new PinScherm();
            pinScherm.Show();
            this.Close();
        }

        //Bij het scrollen op een bepaalde tafel kun u in- of uitzoomen.
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var element = sender as UIElement;
            var position = e.GetPosition(element);
            var transform = element.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);

            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            transform.Matrix = matrix;
        }
    }
}
