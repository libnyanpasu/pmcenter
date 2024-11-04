using System;
using System.IO;

namespace pmcenter
{
    public static partial class Methods
    {
        public static string GetDateTimeString(bool removeInvalidChar = false)
        {
            string result = DateTime.Now.ToString("o");
            if (!removeInvalidChar)
            {
                return result;
            }

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                result = result.Replace(c, '-');
            }

            return result;
        }
    }
}
