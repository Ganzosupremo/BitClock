using BitClock.Bitcoin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BitClock.MVVM.View
{
    /// <summary>
    /// Interaction logic for QuotesView.xaml
    /// </summary>
    public partial class QuotesView : UserControl
    {
        public QuoteBase CurrentQuote { get; set; }

        public QuotesView()
        {
            InitializeComponent();
            this.Loaded += QuotesView_Loaded;
        }

        private void QuotesView_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentQuote = (QuoteBase)DataContext;
        }
    }
}
