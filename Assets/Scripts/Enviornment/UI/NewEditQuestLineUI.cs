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
        int skillz;
        [SerializeField] Transform subQuestParent;
        [SerializeField] Button subQuestListPrefab;
        List<Transform> subQuestButtons = new List<Transform>();
        public int QuestType { get; set; }

        QuestLine chosenQuestLine = null;

        void Awake()
        {
            INSTANCE = this;
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
            ClearAllInfo();
        }

        public void EditQuestLine()
        {
            //set QuestLine
            ScrollManager.INSTANCE.OpenNewQuestLineUIMenu();
            chosenQuestLine = QuestManager.INSTANCE.selectedQuestLine;
            qname.text = chosenQuestLine.Name;
            giver.text = chosenQuestLine.Giver;
            foreach (Quest q in chosenQuestLine.ListOfQuests) 
            {
                Button b = Instantiate(subQuestListPrefab, subQuestParent).GetComponent<Button>();
                b.onClick.AddListener(() => EditSubQuest(q));
                subQuestButtons.Add(b.transform);
            }
        }

        public void NewSubQuest()
        {
            if (chosenQuestLine == null)
                chosenQuestLine = new QuestLine();

            ScrollManager.INSTANCE.TurnOnNewQuest();
        }

        public void AddNewSubQuest(Quest q)
        {
            if (chosenQuestLine == null)
                chosenQuestLine = new QuestLine();

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

        public void EditSubQuest(Quest q)
        {
            NewEditQuestUI.INSTANCE.SetQuest(q);
            ScrollManager.INSTANCE.TurnOnNewQuest();
        }

        public void ClearAllInfo() 
        {
            chosenQuestLine = null;
            qname.text = "";
            giver.text = "";
            foreach (Transform t in subQuestButtons)
            {
                Destroy(t.gameObject);
            }

            subQuestButtons = new List<Transform>(); ;
        }
    }
}