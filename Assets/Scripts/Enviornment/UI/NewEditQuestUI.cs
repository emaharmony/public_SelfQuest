using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest.UI
{
    public class NewEditQuestUI : MonoBehaviour
    {
        public static NewEditQuestUI INSTANCE { get; private set; }

        [SerializeField] Toggle isBonus;
        [SerializeField] TMP_Dropdown qLine;
        [SerializeField] TMP_Dropdown skill;
        [SerializeField] TMP_InputField qname;
        [SerializeField] Transform checkList;
        [SerializeField] GameObject checkListPrefab;

        Quest making;
        List<TMP_InputField> checkListItems = new List<TMP_InputField>();

        public int ChosenSkill { get; set; }
        public int ChosenQline { get; set; }

        private void Awake()
        {
            INSTANCE = this;
        }

        public void MakeNewQuest(QuestLine ql)
        {
            making = new Quest(qname.text, isBonus, ql);

        }

        public void AddNewCheckListItem() 
        {
            GameObject go = Instantiate(checkListPrefab, checkList);
            checkListItems.Add(go.GetComponentInChildren<TMP_InputField>());
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
            making.reward = RewardManager.INSTANCE.CreateReward();
            foreach (TMP_InputField io in checkListItems)
                making.checklist.Add(io.text);

            ScrollManager.INSTANCE.OpenNewQuestLineUIMenu();
            NewEditQuestLineUI.INSTANCE.AddNewSubQuest(making);
            qname.text = "";
        }

        public void SetQuest(Quest q)
        {
            making = q;
            qname.text = q.name;
            isBonus.isOn = q.isBonus;
        }
    }
}
