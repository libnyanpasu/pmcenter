using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class H2Helper
        {
            public static async Task<string> GetStringAsync(Uri uri)
            {
                using HttpContent content = await GetHttpContentAsync(uri).ConfigureAwait(false);
                return await content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}
