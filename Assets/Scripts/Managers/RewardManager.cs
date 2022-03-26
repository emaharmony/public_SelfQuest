using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    /// <summary>
    /// Controls rewards that are given are given. Make it like loot boxes?*** 
    /// </summary>
    public class RewardManager : MonoBehaviour
    {
        [SerializeField] int minEXP= 10, maxEXP = 100, minGold=10, maxGold=100;  
        public static RewardManager INSTANCE { get; private set; }

        public void Awake()
        {
            INSTANCE = this;
        }

        //eventually item if possible
        public Reward CreateReward() 
        {
            return new Reward(Random.Range(minEXP, maxEXP), Random.Range(minGold, maxGold));
        }

        public Reward CreateBigReward() 
        {
            return new Reward(Random.Range(10* minEXP, 5* maxEXP), Random.Range(10* minGold, 5* maxGold));
        }

    }
}
