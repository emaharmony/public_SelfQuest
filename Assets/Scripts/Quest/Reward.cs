using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
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