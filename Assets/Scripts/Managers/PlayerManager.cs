using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest 
{
    /// <summary>
    /// Controls everything that is player related: Level, exp, and gold, skill levels anything iun character screen  
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager INSTANCE { get; private set; }
        public string playerName;
        public int overallLvl;
        public int currExp;
        public int nextLvlEXP;
        public int currGold;
        public int returningPlayer;
        
        
        private void Awake()
        {
            INSTANCE = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            nextLvlEXP = 50 * (overallLvl ^ 2) - (50 * overallLvl);

        }


        public void GivePlayerRewards(int primarySkill, /*int secondarySkill,*/ int exp, int gold)
        {
            currExp += exp;
            currGold += gold;
            SkillManager.INSTANCE.LevelSkill(primarySkill, exp);
            if (currExp >= nextLvlEXP)
                StartCoroutine(LevelUp());     
            //if (secondarySkill != -1) SkillManager.INSTANCE.LevelSkill(secondarySkill, exp);

        }

        public void GivePlayerRewards(Skill primarySkill,/* Skill secondarySkill,*/ int exp, int gold)
        {
            Debug.Log("Rewards: gold ->" + gold + " exp -> " + exp);
            currExp += exp;
            currGold += gold;
            primarySkill.AddEXP(exp);
            if(currExp >= nextLvlEXP)
                StartCoroutine(LevelUp());
            //if (secondarySkill != null) secondarySkill.AddEXP(exp);

        }
        public void EditName(string s)
        {
            playerName = s;
        }

        public IEnumerator LevelUp()
        {
            while (currExp >= nextLvlEXP)
            {
                overallLvl++;
                nextLvlEXP = 50 * (overallLvl ^ 2) - (50 * overallLvl);
                yield return null;
            }

            yield return null;
        }


    }
}
