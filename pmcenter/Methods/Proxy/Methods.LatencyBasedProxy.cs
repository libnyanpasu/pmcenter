using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Methods
    {
        public class LatencyBasedProxy : IWebProxy
        {
            private readonly List<WebProxy> _proxies;
            private readonly string _testUrl;
            private WebProxy _fastestProxy;
            private bool _isInitialized;

            public LatencyBasedProxy(List<WebProxy> proxies, string testUrl)
            {
                _proxies = proxies;
                _testUrl = testUrl;
            }

            public ICredentials Credentials { get; set; }

            // IWebProxy 的 GetProxy 实现
            public Uri GetProxy(Uri destination)
            {
                // 初始化并选择延迟最低的代理
                if (!_isInitialized)
                {
                    _fastestProxy = GetFastestProxyAsync().GetAwaiter().GetResult();
                    Logging.Log($"Selected fastest proxy: {_fastestProxy.Address}");
                    _isInitialized = true;
                }

                return _fastestProxy?.Address;
            }

            // 判断代理是否被绕过（此处直接返回 false，表示所有请求都走代理）
            public bool IsBypassed(Uri host) => false;

            private async Task<WebProxy> GetFastestProxyAsync()
            {
                WebProxy fastestProxy = null;
                long lowestLatency = long.MaxValue;

                foreach (var proxy in _proxies)
                {
                    var stopwatch = Stopwatch.StartNew();
                    try
                    {
                        var handler = new HttpClientHandler { Proxy = proxy };
                        using (var client = new HttpClient(handler))
                        {
                            var response = await client.GetAsync(_testUrl);
                            stopwatch.Stop();

                            if (response.IsSuccessStatusCode && stopwatch.ElapsedMilliseconds < lowestLatency)
                            {
                                fastestProxy = proxy;
                                lowestLatency = stopwatch.ElapsedMilliseconds;
                            }
                        }
                    }
                    catch
                    {
                        // 忽略失败的代理，继续尝试下一个
                    }
                }

                if (fastestProxy == null)
                {
                    throw new Exception(
                        "No proxy is available. Please check your proxy list and network connectivity."
                        );
                }

                return fastestProxy;
            }
        }
    }
}
