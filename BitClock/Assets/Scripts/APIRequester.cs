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
    public static class APIRequester
    {
        public static async UniTask<T> SendRequestAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return default;

            T valueToReturn = default;
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
                        string responseString = await message.Content.ReadAsStringAsync();
                        valueToReturn = JsonConvert.DeserializeAnonymousType(responseString, valueToReturn);

                        client.Dispose();
                        return valueToReturn;
                    }
                    client.Dispose();
                    return valueToReturn;
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"An Exception ocurred: \n{e}");
                return default;
            }
        }
    }
}
