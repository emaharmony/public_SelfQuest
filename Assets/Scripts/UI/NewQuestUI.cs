using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest {
    public class NewQuestUI : MonoBehaviour
    {
        [SerializeField] Toggle isBonus;
        [SerializeField] TMP_Dropdown qLine;
        [SerializeField] TMP_Dropdown skill;
        [SerializeField] TMP_InputField qname;
        [SerializeField] Transform checkList;
        [SerializeField] GameObject checkListPrefab;

        Quest making;
        Skill secondSkill = null;
        
        public void MakeNewQuest(QuestLine ql)
        {
            making = new Quest(qname.text, isBonus, ql);

        }

        public void MakeNewQuest(int i)
        {
            if (qname.text == "")
            {
                //Please put a name for the Quest~ 
                return;
               
            }

            QuestLine questLine = QuestManager.INSTANCE.GetQuestLine(i);
            making = new Quest(qname.text, isBonus, questLine);
            making.secondarySkill = secondSkill;
        }

        public void AddSkill(int i)
        {
            secondSkill = (SkillManager.INSTANCE.GetSkill(i));
        }
    }
}
