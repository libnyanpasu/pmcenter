using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Methods
    {
        public static async Task<bool> TestConnectivity(string target, bool ignore45 = false)
        {
            try
            {
                HttpClient client = new()
                {
                    Timeout = TimeSpan.FromMilliseconds(10000)
                };
                client.DefaultRequestVersion = new Version(2, 0);
                var response = await client.GetAsync(target).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Forbidden && ignore45)
            {
                // 忽略 4xx 和 5xx 的响应错误，根据 ignore45 返回 true
                return true;
            }
            catch (WebException ex)
            {
                Log($"Connectivity to {target} is unavailable: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Log($"Connectivity test failed: {ex.Message}");
                return false;
            }
        }
    }
}
