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
        [SerializeField] TMP_Dropdown numOfComplete;
        [SerializeField] TMP_Dropdown skill;
        [SerializeField] TMP_InputField qname;
        [SerializeField] TMP_InputField qDescription;
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
            making = new Quest(qname.text, qDescription.text, isBonus, ql, numOfComplete.value + 1);

        }

        public void EditChosenQuest() 
        {
            SetQuest(QuestManager.INSTANCE.chosenQuest);
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
            making = new Quest(qname.text, qDescription.text, isBonus, questLine, numOfComplete.value + 1); ;
           // making.secondarySkill = SkillManager.INSTANCE.GetSkill(ChosenSkill);
            
        }

        public void CreateQuest()
        {
            bool edit = false; 
            if (making == null)
                making = new Quest(qname.text, qDescription.text, isBonus, QuestManager.INSTANCE.GetQuestLine(ChosenQline), numOfComplete.value + 1);
            else 
            {
                edit = true;
                making.SetName(qname.text);
                making.description = qDescription.text;
            }

            //foreach (TMP_InputField io in checkListItems)
            //    making.checklist.Add(io.text);
            ScrollManager.INSTANCE.OpenNewQuestLineUIMenu();
            NewEditQuestLineUI.INSTANCE.AddNewSubQuest(making, edit);
            ClearAllInfo();
        }

        public void SetQuest(Quest q)
        {
            making = q;
            qname.text = q.name;
            isBonus.isOn = q.isBonus;

        }

        public void ClearAllInfo() 
        {
            making = null;
            qname.text = "";
            isBonus.isOn = false;
            numOfComplete.value = 0;
            qDescription.text = "";
            
        }

    }
}
