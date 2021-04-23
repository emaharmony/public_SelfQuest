using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelfQuest {
    public class NewQuestUI : MonoBehaviour
    {
        [SerializeField] Toggle isBonus;
        [SerializeField] Dropdown qLine;
        [SerializeField] Dropdown skill;
        [SerializeField] InputField qname;
        [SerializeField] Transform checkList;
        [SerializeField] GameObject checkListPrefab;

        Quest making;
        List<Skill> skills = new List<Skill>();
        
        public void MakeNewQuest(QuestLine ql)
        {
            making = new Quest(qname.text, isBonus, ql);

        }

        public void MakeNewQuest(int i)
        {
            if (qname.text == "")
            {
                //Please out a name for the Quest~ 
                return;
               
            }

            QuestLine questLine = QuestManager.INSTANCE.GetQuestLine(i);
            making = new Quest(qname.text, isBonus, questLine);
            making.affected = skills;
        }

        public void AddSkill(int i)
        {
            skills.Add(SkillManager.INSTANCE.GetSkill(i));
        }
    }
}
