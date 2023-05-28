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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BitClock.MVVM.View
{
    /// <summary>
    /// Interaction logic for DifficultyView.xaml
    /// </summary>
    public partial class DifficultyView : UserControl
    {
        public BitcoinNetwork Network { get; set; }

        private CancellationTokenSource _cancellationTokenSource;

        public DifficultyView()
        {
            InitializeComponent();
            _cancellationTokenSource = new CancellationTokenSource();
            this.Loaded += DifficultyView_Loaded;
            this.Unloaded += DifficultyView_Unloaded;
        }

        private void DifficultyView_Unloaded(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void DifficultyView_Loaded(object sender, RoutedEventArgs e)
        {
            Network = (BitcoinNetwork)this.DataContext;
        }
    }
}
