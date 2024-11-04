using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static void AddRateLimit(long uid)
        {
            if (IsRateDataTracking(uid))
            {
                int dataId = GetRateDataIndexByID(uid);
                Vars.RateLimits[dataId].MessageCount += 1;
            }
            else
            {
                RateData data = new RateData
                {
                    UID = uid,
                    MessageCount = 1
                };
                Vars.RateLimits.Add(data);
            }
        }
    }
}
