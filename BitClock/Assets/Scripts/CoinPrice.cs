using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitClock.Bitcoin
{
    /// <summary>
    /// Stores the price of 1 BTC, in USD, EUR, GBP, and XAU
    /// </summary>
    public struct CoinPrice
    {
        public double USD { get; set; }
        public double EUR { get; set; }
        public double GBP { get; set; }
        public double XAU { get; set; }
        public MarketCap Marketcap { get; set; }
        public SatsPrice Sats { get; set; }

        /// <summary>
        /// Send a request to bitcoinexplorer.org and 
        /// returns the price of 1 BTC, in USD, EUR, GBP, and XAU
        /// </summary>
        public static async UniTask<CoinPrice> GetCoinPrice(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return default;

            CoinPrice priceToReturn = new CoinPrice();
            try
            {
                using (var client = new HttpClient())
                {
                    // Time to wait before the request timeouts
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage message = await client.GetAsync("https://bitcoinexplorer.org/api/price");

                    // Check the API response
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string
                        string responseString = await message.Content.ReadAsStringAsync();
                        priceToReturn = JsonConvert.DeserializeObject<CoinPrice>(responseString);
                        
                        // Get the Market cap
                        priceToReturn.Marketcap = await GetMarketCap(cancellationToken);
                        
                        // Get the sats price
                        priceToReturn.Sats = await GetPriceSats(cancellationToken);
                        
                        client.Dispose();
                        return priceToReturn;
                    }
                    client.Dispose();
                    return priceToReturn;
                }
            }
            catch (Exception)
            {
                return priceToReturn;
            }
        }

        /// <summary>
        /// Sends a request to bitcoinexplorer.org and 
        /// returns the market cap of Bitcoin, in USD, EUR, GBP, XAU
        /// </summary>
        public static async UniTask<MarketCap> GetMarketCap(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return default;

            MarketCap market = new MarketCap();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync("https://bitcoinexplorer.org/api/price/marketcap");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        market = JsonConvert.DeserializeObject<MarketCap>(responseString);
                        client.Dispose();
                        return market;
                    }
                    client.Dispose();
                    return market;
                }
            }
            catch (Exception)
            {
                return market;
            }
        }

        public static async UniTask<SatsPrice> GetPriceSats(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return default;

            SatsPrice sats = new SatsPrice();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync("https://bitcoinexplorer.org/api/price/sats");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        sats = JsonConvert.DeserializeObject<SatsPrice>(responseString);
                        client.Dispose();
                        return sats;
                    }
                    client.Dispose();
                    return sats;
                }
            }
            catch (Exception)
            {
                return sats;
            }
        }

        /// <summary>
        /// Stores the market cap of Bitcoin, in USD, EUR, GBP, XAU
        /// </summary>
        public struct MarketCap
        {
            public double USD { get; set; }
            public double EUR { get; set; }
            public double GBP { get; set; }
            public double XAU { get; set; }
        }

        /// <summary>
        /// Stores the price of 1 unit of [USD, EUR, GBP, XAU] (e.g. 1 "usd") in satoshis
        /// </summary>
        public struct SatsPrice
        {
            public double USD { get; set; }
            public double EUR { get; set; }
            public double GBP { get; set; }
            public double XAU { get; set; }
        }
    }
}
