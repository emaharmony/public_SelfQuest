using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SelfQuest
{
    [Serializable]
    public class Skill
    {
        [SerializeField] string skillName;
        [SerializeField] Color color;
        [SerializeField] int exp, expTillNextLvl;
        [SerializeField] int lvl;

        public Skill(string name, Color i)
        {
            skillName = name;
            color = i;
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
            lvl++;
            expTillNextLvl = Mathf.FloorToInt((100 * (lvl ^ 3)));
            
        }

        public string Name { get { return skillName; } set { skillName = value; } }
        public int EXP { get { return exp; } set { exp = value; } }
        public int LVL { get { return lvl; } set { lvl = value; } }

        public Color SkillColor { get { return color; } set { color = value; } }
    }
}