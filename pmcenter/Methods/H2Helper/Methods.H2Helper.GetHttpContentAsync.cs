﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class H2Helper
        {
            private static async Task<HttpContent> GetHttpContentAsync(Uri uri)
            {
                HttpClient client = new HttpClient { DefaultRequestVersion = new Version(2, 0) };
                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
                return response.Content;
            }
        }
    }
}
