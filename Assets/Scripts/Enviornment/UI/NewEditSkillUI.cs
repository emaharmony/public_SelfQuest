using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest.UI
{
    public class NewEditSkillUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField skillName;
        [SerializeField] Button skillColor;

        Skill skill;


        public void CreateNewSkill()
        {
            if (skillName.text == "") return;
            skill = new Skill(skillName.text, skillColor.image.color);
            SkillManager.INSTANCE.AddSkill(skill);
            skillName.text = "";
        }

        public void EditSkill(Skill s, int index) 
        {
            
        }
    }
}