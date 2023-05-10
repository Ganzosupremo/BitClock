using BitClock.Bitcoin;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private Timer _Timer;
        private int _IntervalMiliseconds = 600000;
        private readonly List<TextBlock> TextList = new List<TextBlock>();
        private DifficultyData _DifficultyWindow = new DifficultyData();
        private PriceWindow _PriceWindow = new PriceWindow();

        public MainWindow()
        {
            InitializeComponent();

            PopulateList();

            MakeAPIRequest();

            _Timer = new Timer(_IntervalMiliseconds);
            _Timer.Elapsed += Timer_Elapsed;
            _Timer.Start();
        }



        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _TipHash = await BitBlock.GetTipHash("https://mempool.space/api/blocks/tip/hash");

            BitBlock newBlock = await BitBlock.GetBlockAsync($"https://mempool.space/api/block/{_TipHash}");

            MessageBox.Show($"Data Requested:\n {newBlock.Id}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            SetTextBlocks(newBlock);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAPIRequest();
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

        private void Difficulty_Click(object sender, RoutedEventArgs e)
        {
            _DifficultyWindow.Show();
            this.Close();
        }

        private async void MakeAPIRequest()
        {
            // Gets the hash of the tip chain block
            _TipHash = await BitBlock.GetTipHash("https://mempool.space/api/blocks/tip/hash");

            // Gets all the data from the tip block using the _TipHash
            BitBlock block = await BitBlock.GetBlockAsync($"https://mempool.space/api/block/{_TipHash}");

            // For Debugging purposes
            //MessageBox.Show("Data Requested", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            SetTextBlocks(block);
        }

        private void PopulateList()
        {
            TextList.Add(IdText);
            TextList.Add(PreviousBlockhashText);
            TextList.Add(HeightText);
            TextList.Add(TimestampText);
            TextList.Add(NonceText);
            TextList.Add(DifficultyText);
        }

        private void SetTextBlocks(BitBlock block)
        {
            ClearList();

            Dispatcher.Invoke(() =>
            {
                IdText.Text = block.Id;
                PreviousBlockhashText.Text = block.Previousblockhash;
                HeightText.Text = block.Height.ToString();
                TimestampText.Text = block.Timestamp.ToString();
                NonceText.Text = block.Nonce.ToString();
                DifficultyText.Text = block.Difficulty.ToString();
            });
        }

        private void ClearList()
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var item in TextList)
                {
                    item.Text = "";
                }
            });
        }

        private void Price_Click(object sender, RoutedEventArgs e)
        {
            _PriceWindow.Show();
            this.Close();
        }
    }
}
