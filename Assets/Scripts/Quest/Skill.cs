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
        [SerializeField] float exp, expTillNextLvl;
        [SerializeField] int lvl;

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