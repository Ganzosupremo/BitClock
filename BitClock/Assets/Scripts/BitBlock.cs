using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Threading;

namespace BitClock.Bitcoin
{
    public class BitBlock
    {
        public BitBlock()
        {
            Id = string.Empty;
            Height = 0;
            Timestamp = 0;
            Size = 0;
            Weight = 0;
            PreviousBlockHash = string.Empty;
            Nonce = 0;
            Difficulty = 0;
        }

        public string Id { get; set; }
        public long Height { get; set; }
        public long Timestamp { get; set; }
        public int Size { get; set; }
        public int Weight { get; set; }
        public string PreviousBlockHash { get; set; }
        public long Nonce { get; set; }
        public long Difficulty { get; set; }

        /// <summary>
        /// Sends an API request to Mempool.space and retrieves the current blockheigth.
        /// </summary>
        /// <returns>Returns the current block heigth as an int</returns>
        public static async UniTask<int> GetBlockHeight(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return 0;

            try
            {
                using (var client = new HttpClient())
                {
                    // Time to wait before the request timeouts
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage message = await client.GetAsync("https://mempool.space/api/blocks/tip/height");

                    // Check the API response
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string
                        string ResponseString = await message.Content.ReadAsStringAsync();
                        
                        client.Dispose();
                        return Convert.ToInt32(ResponseString);
                    }
                    client.Dispose();
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// Sends an API request to Mempool.space and gets the data about a block.
        /// You need to specify the hash of the block, otherwise it wont work.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async UniTask<BitBlock> GetBlockAsync(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return null;

            BitBlock blockToReturn = new BitBlock();
            try
            {
                using (var client = new HttpClient())
                {
                    // Time to wait before the request timeouts
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage message = await client.GetAsync(url);

                    // Check the API response
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string
                        string ResponseString = await message.Content.ReadAsStringAsync();
                        blockToReturn = JsonConvert.DeserializeObject<BitBlock>(ResponseString);
                        client.Dispose();
                        return blockToReturn;
                    }
                    client.Dispose();
                    return blockToReturn;
                }
            }
            catch (Exception)
            {
                return blockToReturn;
            }
        }

        /// <summary>
        /// Sends an API request to Mempool.space and retrieves the hash of the chain tip block
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async UniTask<string> GetTipHash(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return null;

            string stringToReturn = "Could not Send a Request...";
            try
            {
                using (var client = new HttpClient())
                {
                    // Time to wait before the request timeouts
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage message = await client.GetAsync(url);

                    // Check the API response
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string
                        string ResponseString = await message.Content.ReadAsStringAsync();

                        client.Dispose();
                        return ResponseString;
                    }
                    client.Dispose();
                    return stringToReturn;
                }
            }
            catch (Exception)
            {
                return stringToReturn;
            }
        }
    }
}
