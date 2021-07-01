using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest
{
    public class NewSkillUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField skillName;
        [SerializeField] Image skillIcon;

        Skill skill;


        public void CreateNewSkill()
        {
            if (skillName.text == "") return;
            skill = new Skill(skillName.text, skillIcon.sprite);
            SkillManager.INSTANCE.AddSkill(skill);
        }
    }
}