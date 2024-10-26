using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async Task<TimeSpan> TestLatency(string target)
        {
            var reqSw = new Stopwatch();
            try
            {
                var client = new HttpClient();
                reqSw.Start();
                var response = await client.GetAsync(target).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                reqSw.Stop();
                return reqSw.Elapsed;
            }
            catch (HttpRequestException)
            {
                reqSw.Stop();
                return reqSw.Elapsed;
            }
            catch (Exception ex)
            {
                Log($"Latency test failed: {ex.Message}");
                reqSw.Reset();
                return new TimeSpan(0);
            }
        }
    }
}
