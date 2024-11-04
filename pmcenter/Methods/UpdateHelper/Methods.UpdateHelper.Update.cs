using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public class Update
            {
                public string Details;
                public List<string> LangCode;
                public string UpdateArchiveAddress;
                public string UpdateChannel;

                public Update()
                {
                    Details = "(Load failed.)";
                    LangCode = new List<string> { "en.integrated" };
                    UpdateChannel = "master";
                    UpdateArchiveAddress = "https://see.wtf/pmcenter-update";
                }
            }
        }
    }
}
