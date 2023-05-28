using BitClock.Bitcoin;
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
using System.Windows.Shapes;

namespace BitClock
{
    /// <summary>
    /// Interaction logic for DifficultyAdjustment.xaml
    /// </summary>
    public partial class DifficultyData : Window
    {
        private readonly List<TextBlock> _TextsList = new List<TextBlock>();
        private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public DifficultyData()
        {
            InitializeComponent();

            PopulateList();

            SendAPIRequest();

            Deactivated += DifficultyData_Deactivated;
            Activated += DifficultyData_Activated;
        }

        private void DifficultyData_Activated(object sender, EventArgs e)
        {
            _CancellationTokenSource = new CancellationTokenSource();
            Deactivated += DifficultyData_Deactivated;
        }

        private void DifficultyData_Deactivated(object sender, EventArgs e)
        {
            _CancellationTokenSource.Cancel();
            //_CancellationTokenSource.Dispose();
            Deactivated -= DifficultyData_Deactivated;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _TextsList)
            {
                item.Text = "NA";
            }
        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {
            SendAPIRequest();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            //_CancellationTokenSource.Dispose();
            this.Close();
        }

        private async void SendAPIRequest()
        {
            Difficulty data = await APIRequester.SendRequestAsync<Difficulty>("https://mempool.space/api/v1/difficulty-adjustment",  _CancellationTokenSource.Token);

            SetBlockTexts(data);
        }

        private void SetBlockTexts(Difficulty data)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressPercentText.Text = data.ProgressPercent.ToString();
                DifficultyChangeText.Text = data.DifficultyChange.ToString();
                EstimatedRetargetText.Text = data.EstimatedRetargetDate.ToString();
                RemainingBlocksText.Text = data.RemainingBlocks.ToString();
                RemainingTimeText.Text = data.RemainingTime.ToString();
                NextRetargetHeightText.Text = data.NextRetargetHeight.ToString();
                PreviousRetargetText.Text = data.PreviousRetarget.ToString();
                TimeAvgText.Text = data.TimeAvg.ToString();
                TimeOffsetText.Text = data.TimeOffset.ToString();
            });
        }

        private void PopulateList()
        {
            _TextsList.Add(ProgressPercentText);
            _TextsList.Add(DifficultyChangeText);
            _TextsList.Add(EstimatedRetargetText);
            _TextsList.Add(RemainingBlocksText);
            _TextsList.Add(RemainingTimeText);
            _TextsList.Add(PreviousRetargetText);
            _TextsList.Add(NextRetargetHeightText);
            _TextsList.Add(TimeAvgText);
            _TextsList.Add(TimeOffsetText);
        }
    }
}
