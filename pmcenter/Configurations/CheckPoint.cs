﻿namespace pmcenter
{
    public class CheckPoint
    {
        public string Name;
        public long Tick;

        public CheckPoint(long tick = 0, string name = "Unspecified")
        {
            Tick = tick;
            Name = name;
        }
    }
}
