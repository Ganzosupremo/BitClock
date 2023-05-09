using BitClock.Blockchain;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BitClock
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _TipHash = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _TipHash = await BitBlock.GetTipHash("https://mempool.space/api/blocks/tip/hash");

            BitBlock block = await BitBlock.GetBlockAsync($"https://mempool.space/api/block/{_TipHash}");

            SetTextBlocks(block);
        }

        private void SetTextBlocks(BitBlock block)
        {
            IdText.Text = block.Id;
            PreviousBlockhashText.Text = block.Previousblockhash;
            HeightText.Text = block.Height.ToString();
            TimestampText.Text = block.Timestamp.ToString();
            NonceText.Text = block.Nonce.ToString();
            DifficultyText.Text = block.Difficulty.ToString();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            IdText.Text = "NA";
            PreviousBlockhashText.Text = "NA";
            HeightText.Text = "NA";
            TimestampText.Text = "NA";
            NonceText.Text = "NA";
            DifficultyText.Text = "NA";
        }

        private async UniTask MakeRequest(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Time to wait before the request timeouts
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage message = await client.GetAsync(url);

                    // Check the API response
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string
                        string ResponseString = await message.Content.ReadAsStringAsync();
                        // Deserialize the response string
                        //ExchangeRate ResponseObject = JsonConvert.DeserializeObject<ExchangeRate>(ResponseString);

                        MessageBox.Show($"Request was succesfull: {ResponseString}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Return the API response
                        //return ResponseObject;
                    }
                    //return exchangeRate;
                }
            }
            catch (Exception)
            {
                //return exchangeRate;
                return;
            }
        }
    }
}
