using BitClock.Bitcoin;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
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

namespace BitClock.MVVM.View
{
    /// <summary>
    /// Interaction logic for BlockDataView.xaml
    /// </summary>
    public partial class BlockDataView : UserControl
    {
        private string _TipHash = "000000000000000000035c5d77449f404b15de2c1662b48b241659e92d3daa14";
        private System.Timers.Timer _Timer;
        private int _IntervalMiliseconds = 600000;
        private readonly List<TextBlock> TextList = new List<TextBlock>();
        private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public BlockDataView()
        {
            InitializeComponent();

            PopulateTextList();

            this.Loaded += BlockDataView_Loaded;
            this.Unloaded += BlockDataView_Unloaded;
        }

        private void BlockDataView_Unloaded(object sender, RoutedEventArgs e)
        {
            _Timer.Stop();
            _Timer.Elapsed -= Timer_Elapsed;
            _CancellationTokenSource.Cancel();
            this.Loaded -= BlockDataView_Loaded;
            this.Unloaded -= BlockDataView_Unloaded;
        }

        private void BlockDataView_Loaded(object sender, RoutedEventArgs e)
        {
            _Timer = new System.Timers.Timer(_IntervalMiliseconds);
            _Timer.Elapsed += Timer_Elapsed;
            _Timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendRequest();
        }

        private void PopulateTextList()
        {
            TextList.Add(BlockHashText);
            TextList.Add(PreviousBlockHashText);
            TextList.Add(BlockHeightText);
            TextList.Add(TimestampText);
            TextList.Add(NonceText);
        }

        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            SendRequest();
        }

        private async void SendRequest()
        {
            _TipHash = await BitBlock.GetTipHash($"https://mempool.space/api/blocks/tip/hash", _CancellationTokenSource.Token);

            BitBlock block = await APIRequester.SendRequestAsync<BitBlock>($"https://mempool.space/api/block/{_TipHash}", _CancellationTokenSource.Token);

            SetTextBlocks(block);
        }

        private void SetTextBlocks(BitBlock block)
        {
            ClearTextOnList();

            Dispatcher.Invoke(() =>
            {
                BlockHashText.Text = block.Id;
                PreviousBlockHashText.Text = block.PreviousBlockHash;
                BlockHeightText.Text = block.Height.ToString();
                TimestampText.Text = block.Timestamp.ToString();
                NonceText.Text = block.Nonce.ToString();
            });
        }

        private void ClearTextOnList()
        {
            Dispatcher.Invoke(() =>
            {
                BlockHashText.Text = string.Empty;
                PreviousBlockHashText.Text = string.Empty;
                BlockHeightText.Text = string.Empty;
                TimestampText.Text = string.Empty;
                NonceText.Text = string.Empty;
            });
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBlock block in TextList)
            {
                block.Text = "NA";
            }
        }
    }
}
