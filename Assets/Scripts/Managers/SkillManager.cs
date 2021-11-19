using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    /// <summary>
    ///Skill pool is managed and accessed here. 
    /// 
    /// </summary>
    public class SkillManager : MonoBehaviour
    {

        public static SkillManager INSTANCE;

        [SerializeField] List<Skill> pool; //save these in the persistent data

        private void Awake()
        {
            INSTANCE = this;
        }
        private void Start()
        {
            //Get pool of added Skills;
            AddSkill(new Skill("fuck me", null));

        }

        public void AddSkill(Skill s)
        {
            pool.Add(s);
        }

        public Skill GetSkill(int i)
        {
            return pool[i];
        }

        public void LevelSkill(int index, int exp) 
        {
            pool[index].AddEXP(exp);
        }

        public void UpdateUI() 
        {

        }

    }
}