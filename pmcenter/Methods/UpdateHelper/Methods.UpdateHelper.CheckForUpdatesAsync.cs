using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Methods.H2Helper;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public static async Task<Update2> CheckForUpdatesAsync()
            {
                string response = await GetStringAsync(
                    new Uri(
                        Vars.UpdateInfo2URL.Replace("$channel",
                            Vars.CurrentConf == null ? "master" : Vars.CurrentConf.UpdateChannel)
                    )
                );
                return JsonConvert.DeserializeObject<Update2>(response);
            }
        }
    }
}
