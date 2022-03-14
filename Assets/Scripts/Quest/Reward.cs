using System;

namespace SelfQuest
{
    [Serializable]
    public class Reward
    {
        public Reward(int e, int g)
        {
            EXP = e;
            GOLD = g;
        }

        public int EXP { get; internal set; }
        public int GOLD { get; internal set; }
    }
}