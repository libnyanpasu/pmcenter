using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public class Update2
            {
                public string Latest;
                public List<Update> UpdateCollection;
                public UpdateLevel UpdateLevel;

                public Update2()
                {
                    Latest = "0.0.0.0";
                    UpdateLevel = UpdateLevel.Optional;
                    UpdateCollection = new List<Update>();
                }
            }
        }
    }
}
