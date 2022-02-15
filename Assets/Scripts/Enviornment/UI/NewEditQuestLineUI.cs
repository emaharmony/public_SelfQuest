using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SelfQuest.UI
{
    /// <summary>
    /// View section of making a new questline 
    /// </summary>
    public class NewEditQuestLineUI : MonoBehaviour
    {
        public static NewEditQuestLineUI INSTANCE { get; private set; }
        public TMP_InputField qname, giver;
        [SerializeField] TMP_Dropdown qtype;
        int skillz;
        [SerializeField] Transform subQuestParent;
        [SerializeField] Button subQuestListPrefab;
        List<Transform> subQuestButtons = new List<Transform>();
        List<Quest> subQuests { get; set; }
        public int QuestType { get; set; }

        QuestLine chosenQuestLine = null;

        void Awake()
        {
            INSTANCE = this;
            subQuests = new List<Quest>();
        }

        public void PopulateLineInfo()
        {
            qname.text = chosenQuestLine.Name;
            giver.text = chosenQuestLine.Giver;
            for (int i = 0; i < chosenQuestLine.ListOfQuests.Count; i++)
            {
                Button b = Instantiate(subQuestListPrefab, subQuestParent).GetComponent<Button>();
                b.onClick.AddListener(() => EditSubQuest(i));
                subQuestButtons.Add(b.transform);
            }
        }

        public void ChangeSkill(int i)
        {
            skillz = (i);
        }
        
        public void CreateQuestLine()
        {     
            if (qname.text == "") return;
            if (chosenQuestLine == null)
            {
                chosenQuestLine = new QuestLine(qname.text, (QuestLine.QuestType)QuestType, giver.text);
                chosenQuestLine.Reward = RewardManager.INSTANCE.CreateBigReward();
            }
            else
            {
                chosenQuestLine.Name = qname.text;
                chosenQuestLine.Qtype = (QuestLine.QuestType)QuestType;
                chosenQuestLine.Giver = giver.text;
            }

            QuestManager.INSTANCE.AddQuest(chosenQuestLine);
            ScrollManager.INSTANCE.PopulateQuests();
            qname.text = "";
            giver.text = "";

        }

        public void EditQuestLine(int i)
        {
            //set QuestLine
        }

        public void NewSubQuest()
        {
            if (chosenQuestLine == null)
                chosenQuestLine = new QuestLine();

            ScrollManager.INSTANCE.TurnOnNewQuest();
        }

        public void AddNewSubQuest(Quest q)
        {
            Button b = Instantiate(subQuestListPrefab, subQuestParent).GetComponent<Button>();
            b.onClick.AddListener(() => EditSubQuest(subQuestButtons.Count));
            subQuestButtons.Add(b.transform);
            chosenQuestLine.AddQuest(q);
        }

        public void EditSubQuest(int i)
        {
            NewEditQuestUI.INSTANCE.SetQuest(chosenQuestLine.ListOfQuests[i]);
            ScrollManager.INSTANCE.TurnOnNewQuest();
        }

    }
}