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
                HttpResponseMessage response = await client.GetAsync(target).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex) when
                (ex.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Forbidden && ignore45)
            {
                // ���� 4xx �� 5xx ����Ӧ���󣬸��� ignore45 ���� true
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
