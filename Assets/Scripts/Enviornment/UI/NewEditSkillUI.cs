using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest.UI
{
    public class NewEditSkillUI : MonoBehaviour
    {
        public TMP_InputField skillName;
        public Color skillColor;

        Skill skill;

        int ind = -1;

        public void CreateNewSkill()
        {
            if (skillName.text == "") return;
            if (skill == null)
            {
                skill = new Skill(skillName.text, skillColor);
                SkillManager.INSTANCE.AddSkill(skill);
            }
            else 
            {
                skill.Name = skillName.text;
                skill.SkillColor = skillColor;
                
            }

            PrefManager.INSTANCE.SaveSkillPrefs();
            ScrollManager.INSTANCE.CloseNewSkillMenu();
        }

        public void EditSkill(int index) 
        {
            ind = index;
            Debug.Log(ind);
            ScrollManager.INSTANCE.OpenNewSkillMenu(false);
            skill = SkillManager.INSTANCE.pool[index];
            skillName.text = skill.Name;
            skillColor = skill.SkillColor;
        }

        public void DeleteSkill()
        {
            if(ind >= 0)
                SkillManager.INSTANCE.RemoveSkill(ind);
        }

        public void ChangeColor(Color c) 
        {
            skillColor = c;
        }

        public void ClearAllInfo() 
        {
            skill = null;
            skillName.text = "";
            skillColor = Color.black;
        }
    }
}