using BitClock.Bitcoin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
using static BitClock.Bitcoin.CoinPrice;

namespace BitClock.MVVM.View
{
    /// <summary>
    /// Interaction logic for PriceView.xaml
    /// </summary>
    public partial class PriceView : UserControl
    {
        private CoinPrice _CoinPrice = new CoinPrice();
        private DataTable _PriceTable = new DataTable();
        private string _SelectedCurrency = "USD";
        private CancellationTokenSource _CancellationTokenSource;

        public PriceView()
        {
            InitializeComponent();

            this.Loaded += PriceView_Loaded;
            this.Unloaded += PriceView_Unloaded;
        }

        private void PriceView_Unloaded(object sender, RoutedEventArgs e)
        {
            _CancellationTokenSource.Cancel();
        }

        private void PriceView_Loaded(object sender, RoutedEventArgs e)
        {
            _CancellationTokenSource = new CancellationTokenSource();
            InitialiseComboBoxAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //_CoinPrice = await CoinPrice.GetCoinPrice(_CancellationTokenSource.Token);
            UpdatePrices();
        }

        private async void InitialiseComboBoxAsync()
        {
            _CoinPrice = await APIRequester.SendRequestAsync<CoinPrice>("https://bitcoinexplorer.org/api/price", _CancellationTokenSource.Token);
            _CoinPrice.Marketcap = await APIRequester.SendRequestAsync<MarketCap>("https://bitcoinexplorer.org/api/price/marketcap", _CancellationTokenSource.Token);
            _CoinPrice.Sats = await APIRequester.SendRequestAsync<SatsPrice>("https://bitcoinexplorer.org/api/price/sats", _CancellationTokenSource.Token);

            _PriceTable.Columns.Add("Text");
            _PriceTable.Columns.Add("Value");

            _PriceTable.Rows.Add("Select Fiat", 0);
            _PriceTable.Rows.Add("USD", _CoinPrice.USD);
            _PriceTable.Rows.Add("EUR", _CoinPrice.EUR);
            _PriceTable.Rows.Add("GBP", _CoinPrice.GBP);
            _PriceTable.Rows.Add("XAU", _CoinPrice.XAU);

            ShitcoinComboBox.ItemsSource = _PriceTable.DefaultView;

            ShitcoinComboBox.DisplayMemberPath = "Text";
            ShitcoinComboBox.SelectedValuePath = "Value";
            ShitcoinComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the prices based on the selected currency
        /// </summary>
        private void UpdatePrices()
        {
            switch (_SelectedCurrency)
            {
                case "USD":
                    PriceText.Text = _CoinPrice.USD.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.USD.ToString();
                    PriceSatsText.Text = _CoinPrice.Sats.USD.ToString();
                    break;
                case "EUR":
                    PriceText.Text = _CoinPrice.EUR.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.EUR.ToString();
                    PriceSatsText.Text = _CoinPrice.Sats.EUR.ToString();
                    break;
                case "GBP":
                    PriceText.Text = _CoinPrice.GBP.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.GBP.ToString();
                    PriceSatsText.Text = _CoinPrice.Sats.GBP.ToString();
                    break;
                case "XAU":
                    PriceText.Text = _CoinPrice.XAU.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.XAU.ToString();
                    PriceSatsText.Text = _CoinPrice.Sats.XAU.ToString();
                    break;
                default:
                    PriceText.Text = "NA";
                    MarketcapText.Text = "NA";
                    PriceSatsText.Text = "NA";
                    break;
            }
        }

        private void ShitcoinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShitcoinComboBox.SelectedItem != null)
            {
                _SelectedCurrency = ((DataRowView)ShitcoinComboBox.SelectedItem)["Text"].ToString();
            }
        }
    }
}
