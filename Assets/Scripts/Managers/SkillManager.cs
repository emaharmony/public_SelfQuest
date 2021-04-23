using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class SkillManager : MonoBehaviour
    {

        public static SkillManager INSTANCE;

        List<Skill> pool;

        private void Start()
        {
            //Get pool of added Skills;

        }

        public void AddSkill()
        {

        }

        public Skill GetSkill(int i)
        {
            return pool[i];
        }
    }
}