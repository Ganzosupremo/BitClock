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
    /// Interaction logic for DifficultyAdjustment.xaml
    /// </summary>
    public partial class DifficultyData : Window
    {
        private MainWindow _MainWindow;
        private readonly List<TextBlock> _TextsList = new List<TextBlock>();

        public DifficultyData()
        {
            InitializeComponent();

            PopulateList();

            SendAPIRequest();
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
            _MainWindow = new MainWindow();
            _MainWindow.Show();
            this.Close();
        }

        private async void SendAPIRequest()
        {
            Difficulty data = await Difficulty.GetDifficulty();

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
