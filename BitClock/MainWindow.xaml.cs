using BitClock.Bitcoin;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
using Timer = System.Timers.Timer;

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
        private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();

            //PopulateList();

            //MakeAPIRequest();

            //_Timer = new Timer(_IntervalMiliseconds);
            //_Timer.Elapsed += Timer_Elapsed;
            //Activated += MainWindow_Activated;
            //Deactivated += MainWindow_Deactivated;
            //_Timer.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (InvalidOperationException)
            {
                return;
            }

        }

        //private void MainWindow_Activated(object sender, EventArgs e)
        //{
        //    _CancellationTokenSource = new CancellationTokenSource();
        //    _Timer.Start();
        //    _Timer.Elapsed += Timer_Elapsed;
        //    Deactivated += MainWindow_Deactivated;
        //}

        //private void MainWindow_Deactivated(object sender, EventArgs e)
        //{
        //    _CancellationTokenSource.Cancel();
        //    _Timer.Stop();
        //    _Timer.Elapsed -= Timer_Elapsed;
        //    Deactivated -= MainWindow_Deactivated;
        //    //_CancellationTokenSource.Dispose();
        //}

        //private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    _TipHash = await APIRequester.SendRequestAsync<string>("https://mempool.space/api/blocks/tip/hash", _CancellationTokenSource.Token);

        //    BitBlock newBlock = await APIRequester.SendRequestAsync<BitBlock>($"https://mempool.space/api/block/{_TipHash}", _CancellationTokenSource.Token);

        //    MessageBox.Show($"Data Requested", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

        //    SetTextBlocks(newBlock);
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    MakeAPIRequest();
        //}

        //private void Clear_Click(object sender, RoutedEventArgs e)
        //{
        //    idText.Text = "NA";
        //    previousBlockhashText.Text = "NA";
        //    heightText.Text = "NA";
        //    timestampText.Text = "NA";
        //    nonceText.Text = "NA";
        //    difficultyText.Text = "NA";
        //}

        //private void Difficulty_Click(object sender, RoutedEventArgs e)
        //{
        //    DifficultyData difficultyData = new DifficultyData();
        //    difficultyData.Show();
        //}

        //private async void MakeAPIRequest()
        //{
        //    // Gets the hash of the tip chain block
        //    _TipHash = await BitBlock.GetTipHash("https://mempool.space/api/blocks/tip/hash", _CancellationTokenSource.Token);

        //    // Gets all the data from the tip block using the _TipHash
        //    BitBlock block = await BitBlock.GetBlockAsync($"https://mempool.space/api/block/{_TipHash}", _CancellationTokenSource.Token);

        //    // For Debugging purposes
        //    //MessageBox.Show("Data Requested", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

        //    SetTextBlocks(block);
        //}

        //private void PopulateList()
        //{
        //    TextList.Add(idText);
        //    TextList.Add(previousBlockhashText);
        //    TextList.Add(heightText);
        //    TextList.Add(timestampText);
        //    TextList.Add(nonceText);
        //    TextList.Add(difficultyText);
        //}

        //private void SetTextBlocks(BitBlock block)
        //{
        //    ClearList();

        //    Dispatcher.Invoke(() =>
        //    {
        //        idText.Text = block.Id;
        //        previousBlockhashText.Text = block.PreviousBlockHash;
        //        heightText.Text = block.Height.ToString();
        //        timestampText.Text = block.Timestamp.ToString();
        //        nonceText.Text = block.Nonce.ToString();
        //        difficultyText.Text = block.Difficulty.ToString();
        //    });
        //}

        //private void ClearList()
        //{
        //    Dispatcher.Invoke(() =>
        //    {
        //        foreach (var item in TextList)
        //        {
        //            item.Text = "";
        //        }
        //    });
        //}

        //private void Price_Click(object sender, RoutedEventArgs e)
        //{
        //    PriceWindow priceWindow = new PriceWindow();
        //    priceWindow.Show();
        //}
    }
}
