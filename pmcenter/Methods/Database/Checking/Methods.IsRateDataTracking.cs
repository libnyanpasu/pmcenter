namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsRateDataTracking(long uid)
        {
            foreach (Conf.RateData data in Vars.RateLimits)
            {
                if (data.UID == uid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
