using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class SkillManager : MonoBehaviour
    {

        public static SkillManager INSTANCE;

        List<Skill> pool; //save these in the persistent data

        private void Awake()
        {
            INSTANCE = this;
        }
        private void Start()
        {
            //Get pool of added Skills;

        }

        public void AddSkill(Skill s)
        {
            pool.Add(s);
        }

        public Skill GetSkill(int i)
        {
            return pool[i];
        }
    }
}