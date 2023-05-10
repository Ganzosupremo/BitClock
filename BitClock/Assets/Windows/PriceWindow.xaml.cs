using BitClock.Bitcoin;
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

namespace BitClock
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class PriceWindow : Window
    {
        public PriceWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            CoinPrice coin = await CoinPrice.GetCoinPrice();
            MessageBox.Show($"USD: {coin.USD}.\n Market cap USD: {coin.Marketcap.USD}.\n Sats price USD: {coin.Sats.USD}.", "Information", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
        }
    }
}
