using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class Reward
    {
        protected int exp, gold;
        public Reward(int e, int g)
        {
            exp = e;
            gold = g;
        }
    }
}