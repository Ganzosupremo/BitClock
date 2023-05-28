using BitClock.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitClock.Bitcoin
{
    public class BitcoinNetwork : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand GetHalvingDataCommand { get; set; }
        public RelayCommand GetCoinSupplyCommand { get; set; }
        public RelayCommand GetTxtVolumeCommand { get; set; }
        public RelayCommand GetHashrateCommand { get; set; }

        public TransactionVolume DayTransactionsVolume
        {
            get => _transactionsVolume;
            set
            {
                _transactionsVolume = value;
                OnPropertyChanged(nameof(DayTransactionsVolume));
            }
        }
        public CoinSupply SupplyOfCoins
        {
            get => _supplyOfCoins;
            set
            {
                _supplyOfCoins = value;
                OnPropertyChanged(nameof(SupplyOfCoins));
            }
        }
        public Halving HalvingData
        {
            get => _halvingData;
            set
            {
                _halvingData = value;
                OnPropertyChanged(nameof(HalvingData));
            }
        }
        public Hashrate HashrateData
        {
            get => _hashrate;
            set
            {
                _hashrate = value;
                OnPropertyChanged(nameof(HashrateData));
            }
        }

        public BitcoinNetwork()
        {
            GetHalvingDataCommand = new RelayCommand(HalvingDataCommand);
            GetCoinSupplyCommand = new RelayCommand(CoinSupplyCommand);
            GetTxtVolumeCommand = new RelayCommand(TxtVolumeCommand);
            GetHashrateCommand = new RelayCommand(HashrateCommand);

            DayTransactionsVolume = new TransactionVolume();
            SupplyOfCoins = new CoinSupply();
            HashrateData = new Hashrate();
            HalvingData = new Halving();
        }

        private TransactionVolume _transactionsVolume;
        private CoinSupply _supplyOfCoins;
        private Halving _halvingData;
        private Hashrate _hashrate;

        private async void CoinSupplyCommand(object obj)
        {
            SupplyOfCoins = await APIRequester.SendRequestAsync<CoinSupply>("https://bitcoinexplorer.org/api/blockchain/coins");
        }

        private async void TxtVolumeCommand(object obj)
        {
            DayTransactionsVolume = await APIRequester.SendRequestAsync<TransactionVolume>("https://bitcoinexplorer.org/api/tx/volume/24h");
        }

        private async void HalvingDataCommand(object obj)
        {
            HalvingData = await APIRequester.SendRequestAsync<Halving>("https://bitcoinexplorer.org/api/blockchain/next-halving");
        }

        private async void HashrateCommand(object obj)
        {
            Hashrate hashrate = await APIRequester.SendRequestAsync<Hashrate>("https://bitcoinexplorer.org/api/mining/hashrate");
            HashrateData = hashrate;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Serializable]
        public struct CoinSupply
        {
            [JsonProperty("supply")]
            public double Supply { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
        }

        [Serializable]
        public struct Halving
        {
            public int NextHalvingIndex { get; set; }
            public int NextHalvingBlock { get; set; }
            public double NextHalvingSubsidy { get; set; }
            public int BlocksUntilNextHalving { get; set; }
            public string TimeUntilNextHalving { get; set; }
            public DateTime NextHalvingEstimatedDate { get; set; }
        }

        [Serializable]
        public struct TransactionVolume
        {
            [JsonProperty("24h")]
            public long H24 { get; set; }
        }

        [Serializable]
        public struct Hashrate
        {
            [JsonProperty("1Day")]
            public TimeFrame OneDay { get; set; }
            [JsonProperty("7Day")]
            public TimeFrame SevenDays { get; set; }
            [JsonProperty("30Day")]
            public TimeFrame OneMonth { get; set; }
            [JsonProperty("90Day")]
            public TimeFrame ThreeMonths { get; set; }
            [JsonProperty("365Day")]
            public TimeFrame OneYear { get; set; }

            [Serializable]
            public struct TimeFrame
            {
                [JsonProperty("val")]
                public double Value { get; set; }
                [JsonProperty("unit")]
                public string Unit { get; set; }
                [JsonProperty("unitAbbreviation")]
                public string UnitAbbreviation { get; set; }
                [JsonProperty("unitExponent")]
                public int UnitExponent { get; set; }
                [JsonProperty("unitMultiplier")]
                public long UnitMultiplier { get; set; }

                public double DifficultyAdjustmentStimate { get; set; }

            }
        }
    }
}
