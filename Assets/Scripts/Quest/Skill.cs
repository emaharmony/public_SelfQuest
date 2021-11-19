using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SelfQuest
{
    [Serializable]
    public class Skill
    {
        [SerializeField]string skillName;
        [SerializeField] Sprite icon;
        [SerializeField] int exp, expTillNextLvl;
        [SerializeField] int lvl;

        public Skill(string name, Sprite i)
        {
            skillName = name;
            icon = i;
            exp = 0;
            lvl = 1;
            expTillNextLvl = 5;
        }

        public void AddEXP(int e) 
        {
            exp += e;
            if (exp >= expTillNextLvl)
                LevelSkill();
        }

        void LevelSkill() 
        {
            expTillNextLvl *= 2; //find better formula
            lvl++;
        }
    }
}