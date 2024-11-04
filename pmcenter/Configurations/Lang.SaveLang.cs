using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static pmcenter.Methods.Logging;

namespace pmcenter
{
    public static partial class Lang
    {
        public static async Task<bool> SaveLang(bool isInvalid = false)
        {
            // DO NOT HANDLE ERRORS HERE.
            string text = JsonConvert.SerializeObject(Vars.CurrentLang, Formatting.Indented);
            StreamWriter writer = new StreamWriter(File.Create(Vars.LangFile), Encoding.UTF8);
            await writer.WriteAsync(text).ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
            writer.Close();
            if (isInvalid)
            {
                Log("We've detected an invalid language file and have reset it.", "LANG", LogLevel.Warning);
                Log("Please reconfigure it and try to start pmcenter again.", "LANG", LogLevel.Warning);
                Vars.RestartRequired = true;
            }

            return true;
        }
    }
}
