using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class Skill
    {
        string skillName;
        Sprite icon;
        float exp, expTillNextLvl;
        int lvl;

        public Skill(string name, Sprite i)
        {
            skillName = name;
            icon = i;
            exp = 0;
            lvl = 1;
            expTillNextLvl = 10;
        }
    }
}