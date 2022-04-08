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
        public TMP_Dropdown skillDropDown;
        int skillz = 0;
        [SerializeField] Transform subQuestParent;
        [SerializeField] QuestListItem subQuestListPrefab;
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
            skillz = SkillManager.INSTANCE.pool.IndexOf(chosenQuestLine.Skill);
            skillDropDown.SetValueWithoutNotify(skillz);
            for (int i = 0; i < chosenQuestLine.ListOfQuests.Count; i++)
            {

                QuestListItem q = Instantiate(subQuestListPrefab, subQuestParent) as QuestListItem;
                q.GetComponent<Button>().onClick.AddListener(() => EditSubQuest(i));
                subQuestButtons.Add(q.transform);
                q.Questy = chosenQuestLine.ListOfQuests[i];
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
            chosenQuestLine.Skill = SkillManager.INSTANCE.pool[skillz];
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
            skillz = SkillManager.INSTANCE.pool.IndexOf(chosenQuestLine.Skill);
            skillDropDown.SetValueWithoutNotify(skillz);
            foreach (Quest q in chosenQuestLine.ListOfQuests) 
            {
                QuestListItem qlist = Instantiate(subQuestListPrefab, subQuestParent) as QuestListItem;
                qlist.GetComponent<Button>().onClick.AddListener(() => EditSubQuest(q));
                subQuestButtons.Add(qlist.transform);
                qlist.Questy = q;
            }
        }

        public void NewSubQuest()
        {
            if (chosenQuestLine == null)
                chosenQuestLine = new QuestLine();

            ScrollManager.INSTANCE.TurnOnNewQuest();
        }

        public void AddNewSubQuest(Quest q, bool edit)
        {
            if (edit) return;
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
            foreach (Transform t in subQuestParent)
            {
                Destroy(t.gameObject);
            }
            subQuestButtons.Clear();
            subQuestButtons = new List<Transform>(); ;
        }
    }
}