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
        public string playerName { get; private set; }
       public int overallLvl { get; private set; }
        public int currExp { get; private set; } 
        public int nextLvlEXP { get; private set; } 
        public int currGold { get; private set; }

        const string PREF_LEVEL = "playerLvl", CURR_EXP_PREF = "current_exp", PREF_GOLD = "gold", PREF_NXT_LVL= "nxt_lvl_exp";

        private void Awake()
        {
            INSTANCE = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            //grab values from pref
            overallLvl = 1; currExp = 0; nextLvlEXP = 0; currGold = 0;
        }


        public void GivePlayerRewards(int primarySkill, int secondarySkill, int exp, int gold)
        {
            currExp += exp;
            currGold += gold;
            SkillManager.INSTANCE.LevelSkill(primarySkill, exp);
                
            if (secondarySkill != -1) SkillManager.INSTANCE.LevelSkill(secondarySkill, exp);

        }

        public void GivePlayerRewards(Skill primarySkill, Skill secondarySkill, int exp, int gold)
        {
            Debug.Log("Rewards: gold ->" + gold + " exp -> " + exp);
            currExp += exp;
            currGold += gold;
            primarySkill.AddEXP(exp);
            if (secondarySkill != null) secondarySkill.AddEXP(exp);

        }
        public void EditName(string s)
        {
            playerName = s;   
        }
    }
}
