using static pmcenter.Conf;

namespace pmcenter
{
    public static partial class Methods
    {
        public static void BanUser(long uid)
        {
            if (!IsBanned(uid))
            {
                BanObj banned = new BanObj
                {
                    UID = uid
                };
                Vars.CurrentConf.Banned.Add(banned);
            }
        }
    }
}
