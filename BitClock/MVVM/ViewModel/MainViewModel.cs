using BitClock.Core;
using System.Windows;
using System.Windows.Input;

namespace BitClock.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand PriceViewCommand { get; set; }
        public RelayCommand BlockDataViewCommand { get; set; }
        public RelayCommand NetworkDiffViewCommand { get; set; }
        public RelayCommand QuotesViewCommand { get; set; }
        public RelayCommand CloseAppCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public PriceViewModel PriceVM { get; set; }
        public BlockDataViewModel BlockDataVM { get; set; }
        public DifficultyViewModel DifficultyVM { get; set; }
        public QuotesViewModel QuotesVM { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private object _currentView;
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            PriceVM = new PriceViewModel();
            BlockDataVM = new BlockDataViewModel();
            DifficultyVM = new DifficultyViewModel();
            QuotesVM = new QuotesViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            PriceViewCommand = new RelayCommand(o =>
            {
                CurrentView = PriceVM;
            });

            BlockDataViewCommand = new RelayCommand(o =>
            {
                CurrentView = BlockDataVM;
            });

            NetworkDiffViewCommand = new RelayCommand(o =>
            {
                CurrentView = DifficultyVM;
            });

            QuotesViewCommand = new RelayCommand(o =>
            {
                CurrentView = QuotesVM;
            });

            CloseAppCommand = new RelayCommand(CloseApp);
        }

        private void CloseApp(object commandParameter)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeApp(object commandParameter)
        {
            
        }
    }
}
