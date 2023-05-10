using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitClock.Bitcoin
{
    public struct Difficulty
    {
        public Difficulty(string args)
        {
            ProgressPercent = 0;
            DifficultyChange = 0;
            EstimatedRetargetDate = 0;
            RemainingBlocks = 0;
            RemainingTime = 0;
            PreviousRetarget = 0;
            NextRetargetHeight = 0;
            TimeAvg = 0;
            TimeOffset = 0;
        }

        public double ProgressPercent { get; set; }
        public double DifficultyChange { get; set; }
        public long EstimatedRetargetDate { get; set; }
        public int RemainingBlocks { get; set; }
        public int RemainingTime { get; set; }
        public double PreviousRetarget { get; set; }
        public int NextRetargetHeight { get; set; }
        public int TimeAvg { get; set; }
        public int TimeOffset { get; set; }

        /// <summary>
        /// Sends an API request to Mempool.space and retrieves the current data on the network difficulty adjustment.
        /// </summary>
        /// <returns>Returns a struct with the data retrieved from the API request.</returns>
        public static async UniTask<Difficulty> GetDifficulty()
        {
            Difficulty difficulty = new Difficulty("args");
            try
            {
                using (var client = new HttpClient())
                {
                    // Time to wait before the request timeouts
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage message = await client.GetAsync("https://mempool.space/api/v1/difficulty-adjustment");

                    // Check the API response
                    if (message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string
                        string responseString = await message.Content.ReadAsStringAsync();
                        difficulty = JsonConvert.DeserializeObject<Difficulty>(responseString);
                        return difficulty;
                    }
                }
            }
            catch (Exception)
            {
                return difficulty;
            }
            return difficulty;
        }
    }
}
