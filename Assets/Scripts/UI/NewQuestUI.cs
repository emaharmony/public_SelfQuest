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
        
        public int ChosenSkill { get; set; }
        public int ChosenQline { get; set; }

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
            making.secondarySkill = SkillManager.INSTANCE.GetSkill(ChosenSkill);
        }



        public void CreateQuest()
        {
            making = new Quest(qname.text, isBonus, QuestManager.INSTANCE.GetQuestLine(ChosenQline));
            making.reward = new Reward();
        }

    }
}
