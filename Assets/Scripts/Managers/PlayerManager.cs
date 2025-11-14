
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
        // Total accumulated experience for the player (capped by MAX_XP)
        public static PlayerManager INSTANCE { get; private set; }
        public string playerName;

        public int currGold;
        public int returningPlayer;

        bool _euaAccepted = true;

        private void Awake()
        {
            INSTANCE = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            //init levels
        }


        public void GivePlayerRewards(int primarySkill, /*int secondarySkill,*/ int exp, int gold)
        {
            currExp += exp;
            currGold += gold;
            SkillManager.INSTANCE.LevelSkill(primarySkill, exp);
            if (currExp >= nextLvlEXP && overallLvl < MAX_LEVEL)
                StartCoroutine(LevelUp());
            //if (secondarySkill != -1) SkillManager.INSTANCE.LevelSkill(secondarySkill, exp);
            PrefManager.INSTANCE.SaveUserPrefs();
        }

        public void GivePlayerRewards(Skill primarySkill,/* Skill secondarySkill,*/ int exp, int gold)
        {
            Debug.Log("Rewards: gold ->" + gold + " exp -> " + exp);
            currGold += gold;
            primarySkill.AddEXP(exp);
            AddEXP(exp);

            //if (secondarySkill != null) secondarySkill.AddEXP(exp);
            PrefManager.INSTANCE.SaveUserPrefs();

        }

        public void EditName(string s)
        {
            playerName = s;
            PrefManager.INSTANCE.SaveUserPrefs();
        }

    }
}
