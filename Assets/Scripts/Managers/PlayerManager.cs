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
        int overallLvl = 0, currExp = 0, nextLvlEXP = 0, currGold = 0;

        const string PREF_LEVEL = "playerLvl", CURR_EXP_PREF = "current_exp", PREF_GOLD = "gold", PREF_NXT_LVL= "nxt_lvl_exp";

        // Start is called before the first frame update
        void Start()
        {
            //grab values from pref
        }


        public void GivePlayerRewards(int skillIndex, int exp, int gold)
        {
            currExp += exp;
            SkillManager.INSTANCE.LevelSkill(exp, skillIndex);
            currGold += gold;
        }
    }
}
