namespace pmcenter
{
    public static partial class Methods
    {
        public static bool IsUserRetractionAvailable(int userSessionMsgId)
        {
            if (!Vars.CurrentConf.EnableMsgLink)
            {
                return false;
            }

            lock (Vars.CurrentConf.MessageLinks)
            {
                foreach (Conf.MessageIDLink link in Vars.CurrentConf.MessageLinks)
                {
                    if (link.UserSessionMessageID == userSessionMsgId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
