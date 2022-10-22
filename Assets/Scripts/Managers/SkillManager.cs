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

        public List<Skill> pool; //save these in the persistent data


        private void Awake()
        {
            INSTANCE = this;
        }

        public void AddSkill(Skill s)
        {
            pool.Add(s);
            PrefManager.INSTANCE.SaveSkillPrefs();
        }

        public Skill GetSkill(int i)
        {
            return pool[i];
        }

        public void RemoveSkill(int i) 
        {
            if (i < 0) return;
            pool.Remove(pool[i]);
            PrefManager.INSTANCE.SaveSkillPrefs();

        }

        public void LevelSkill(int index, int exp) 
        {
            pool[index].AddEXP(exp);
            PrefManager.INSTANCE.SaveSkillPrefs();
        }

        public void LevelSkill(Skill index, int exp)
        {
            index.AddEXP(exp);
            PrefManager.INSTANCE.SaveSkillPrefs();
        }
        public void UpdateUI() 
        {

        }
    }
}