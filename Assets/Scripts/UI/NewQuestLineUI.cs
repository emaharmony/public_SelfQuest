using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SelfQuest {
    public class NewQuestLineUI : MonoBehaviour
    {
        public TMP_InputField qname, giver;
        [SerializeField] TMP_Dropdown qtype;
        int skillz;
        [SerializeField] Transform subQuestParent;
        [SerializeField] GameObject subQuestListPrefab;
        List<Quest> subQuests { get; set; }
        public int QuestType { get; set; }

        QuestLine chosenQuestLine;

        void Awake()
        {
            subQuests = new List<Quest>();
        }

        public void PopulateLineInfo()
        {
            qname.text = chosenQuestLine.Name;
            giver.text = chosenQuestLine.Giver;
            for (int i = 0; i < chosenQuestLine.ListOfQuests.Count; i++)
            {
                Instantiate(subQuestListPrefab, subQuestParent);
            }
        }

        public void ChangeSkill(int i)
        {
            skillz = (i);
        }
        
        public void CreateQuestLine()
        {     
            if (qname.text == "") return;
           chosenQuestLine = new QuestLine(qname.text, (QuestLine.QuestType)QuestType, giver.text);
            QuestManager.INSTANCE.AddQuest(chosenQuestLine);
            ScrollManager.INSTANCE.OpenQuestList();
        }

        public void CreateNewQuestLine()
        {
            chosenQuestLine = null;
        }

        public void EditQuestLine()
        {

        }

    }
}