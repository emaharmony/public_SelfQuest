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
            skill = new Skill(skillName.text, skillColor);
            SkillManager.INSTANCE.AddSkill(skill);
            ScrollManager.INSTANCE.CloseNewSkillMenu();
        }

        public void EditSkill(int index) 
        {
            
        }

        public void ChangeColor(Color c) 
        {
            skillColor = c;
        }
    }
}