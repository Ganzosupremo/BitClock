using BitClock.Bitcoin;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
using System.Windows.Shapes;
using static BitClock.Bitcoin.CoinPrice;

namespace BitClock
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class PriceWindow : Window
    {
        private CoinPrice _CoinPrice = new CoinPrice();
        private DataTable _PriceTable = new DataTable();
        private string _SelectedCurrency = "USD";
        private MainWindow _MainWindow;

        private List<FrameworkElement> _TextsList = new List<FrameworkElement>();
        private List<FrameworkElement> _QuotesHeaders = new List<FrameworkElement>();

        private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public PriceWindow()
        {
            InitializeComponent();

            PopulateLists();
            EnableHeaders(Visibility.Hidden);
            InitializeComboBoxAsync();

            Deactivated += PriceWindow_Deactivated;
            Activated += PriceWindow_Activated;
        }

        private void PriceWindow_Activated(object sender, EventArgs e)
        {
            _CancellationTokenSource = new CancellationTokenSource();
            Deactivated += PriceWindow_Deactivated;
        }

        private void PriceWindow_Deactivated(object sender, EventArgs e)
        {
            _CancellationTokenSource.Cancel();
            //_CancellationTokenSource.Dispose();
            Deactivated -= PriceWindow_Deactivated;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _CoinPrice = await GetCoinPrice(_CancellationTokenSource.Token);
            UpdatePrices();

            //MessageBox.Show($"USD: {_CoinPrice.USD}.\n Market cap USD: {_CoinPrice.Marketcap.USD}.\n Sats price USD: {_CoinPrice.Sats.USD}.", "Information", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
        }

        private void CmbCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCurrency.SelectedItem != null)
            {
                _SelectedCurrency = ((DataRowView)cmbCurrency.SelectedItem)["Text"].ToString();
            }
        }

        private async void InitializeComboBoxAsync()
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

            cmbCurrency.ItemsSource = _PriceTable.DefaultView;

            cmbCurrency.DisplayMemberPath = "Text";
            cmbCurrency.SelectedValuePath = "Value";
            cmbCurrency.SelectedIndex = 0;
        }

        private void PopulateLists()
        {
            _TextsList.Add(PriceText);
            _TextsList.Add(MarketcapText);
            _TextsList.Add(SatsText);
            _TextsList.Add(QuoteText);
            _TextsList.Add(SpeakerText);
            _TextsList.Add(URLText);
            _TextsList.Add(DateText);
            _TextsList.Add(IndexText);

            _QuotesHeaders.Add(SpeakerHeader);
            _QuotesHeaders.Add(SpeakerText);
            _QuotesHeaders.Add(QuoteHeader);
            _QuotesHeaders.Add(QuoteText);
            _QuotesHeaders.Add(URLHeader);
            _QuotesHeaders.Add(URLText);
            _QuotesHeaders.Add(DateHeader);
            _QuotesHeaders.Add(DateText);
            _QuotesHeaders.Add(IndexHeader);
            _QuotesHeaders.Add(IndexText);
        }

        private void EnableHeaders(Visibility visibility)
        {
            foreach (FrameworkElement block in _QuotesHeaders)
            {
                block.Visibility = visibility;
            }
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
                    SatsText.Text = _CoinPrice.Sats.USD.ToString();
                    break;
                case "EUR":
                    PriceText.Text = _CoinPrice.EUR.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.EUR.ToString();
                    SatsText.Text = _CoinPrice.Sats.EUR.ToString();
                    break;
                case "GBP":
                    PriceText.Text = _CoinPrice.GBP.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.GBP.ToString();
                    SatsText.Text = _CoinPrice.Sats.GBP.ToString();
                    break;
                case "XAU":
                    PriceText.Text = _CoinPrice.XAU.ToString();
                    MarketcapText.Text = _CoinPrice.Marketcap.XAU.ToString();
                    SatsText.Text = _CoinPrice.Sats.XAU.ToString();
                    break;
                default:
                    PriceText.Text = "NA";
                    MarketcapText.Text = "NA";
                    SatsText.Text = "NA";
                    break;
            }
        }

        private async void Fun_Click(object sender, RoutedEventArgs e)
        {
            QuoteBase fun = await APIRequester.SendRequestAsync<QuoteBase>("https://bitcoinexplorer.org/api/quotes/random", _CancellationTokenSource.Token);

            EnableHeaders(Visibility.Visible);

            QuoteText.Text = fun.Text;
            SpeakerText.Text = fun.Speaker;
            URLText.Text = fun.URL;
            DateText.Text = fun.Date.ToString("dd.MM.yyyy");
            IndexText.Text = fun.QuoteIndex.ToString();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            //_MainWindow = new MainWindow();
            //_MainWindow.Show();
            //_CancellationTokenSource.Dispose();
            this.Close();
        }
    }
}
