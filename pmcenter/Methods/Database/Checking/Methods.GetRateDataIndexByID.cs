namespace pmcenter
{
    public static partial class Methods
    {
        public static int GetRateDataIndexByID(long uid)
        {
            foreach (Conf.RateData data in Vars.RateLimits)
            {
                if (data.UID == uid)
                {
                    return Vars.RateLimits.IndexOf(data);
                }
            }

            return -1;
        }
    }
}
