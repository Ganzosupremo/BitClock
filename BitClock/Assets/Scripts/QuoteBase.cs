using BitClock.Core;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitClock.Bitcoin
{
    /// <summary>
    /// Stores data about a Bitcoin quote
    /// </summary>
    public class QuoteBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Quote _CurrentQuote;

        public Quote CurrentQuote
        {
            get { return _CurrentQuote; }
            set 
            { 
                _CurrentQuote = value;
                OnPropertyChanged(nameof(CurrentQuote));
            }
        }

        [Obsolete]
        public string Text
        {
            get { return _Text; }
            set 
            { 
                _Text = value;
                OnPropertyChanged(nameof(Text)); 
            }
        }

        [Obsolete]
        public string Speaker
        {
            get { return _Speaker; }
            set 
            { 
                _Speaker = value;
                OnPropertyChanged(nameof(Speaker));
            }
        }

        [Obsolete]
        public string URL
        {
            get { return _Url; }
            set 
            { 
                _Url = value; 
                OnPropertyChanged(nameof(URL));
            }
        }

        [Obsolete]
        public DateTime Date
        {
            get { return _Date; }
            set 
            { 
                _Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        [Obsolete]
        public int QuoteIndex
        {
            get { return _QuoteIndex; }
            set 
            { 
                _QuoteIndex = value;
                OnPropertyChanged(nameof(QuoteIndex));
            }
        }

        public RelayCommand GetQuoteCommmand { get; set; }

        private string _Speaker = "Satoshi";
        private string _Text = "Placeholder";
        private string _Url;
        private DateTime _Date;
        private int _QuoteIndex;

        private const string _Dash = "- ";

        public QuoteBase()
        {
            GetQuoteCommmand = new RelayCommand(QuoteCommand);
        }

        private async void QuoteCommand(object obj)
        {
            Quote temp = await APIRequester.SendRequestAsync<Quote>("https://bitcoinexplorer.org/api/quotes/random");
            temp.Speaker = string.Format(_Dash + temp.Speaker);
            CurrentQuote = temp;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Serializable]
        public struct Quote
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("speaker")]
            public string Speaker { get; set; }
            [JsonProperty("url")]
            public string URL { get; set; }
            [JsonProperty("date")]
            public DateTime Date { get; set; }
            [JsonProperty("quoteIndex")]
            public int QuoteIndex { get; set; }
        }

        //public static async UniTask<Quote> GetQuote(CancellationToken cancellationToken = default)
        //{
        //    if (cancellationToken.IsCancellationRequested) return default;

        //    Quote funToReturn = new Quote();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.Timeout = TimeSpan.FromMinutes(1);
        //            HttpResponseMessage response = await client.GetAsync("https://bitcoinexplorer.org/api/quotes/random");

        //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                string responseString = await response.Content.ReadAsStringAsync();
        //                funToReturn = JsonConvert.DeserializeObject<Quote>(responseString);
        //                client.Dispose();
        //                return funToReturn;
        //            }
        //            client.Dispose();
        //            return funToReturn;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return funToReturn;
        //    }
        //}
    }
}
