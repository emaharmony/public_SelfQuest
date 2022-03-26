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

            ScrollManager.INSTANCE.CloseNewSkillMenu();
        }

        public void EditSkill(int index) 
        {
            ScrollManager.INSTANCE.OpenNewSkillMenu();
            skill = SkillManager.INSTANCE.pool[index];
            skillName.text = skill.Name;
            skillColor = skill.SkillColor;
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