using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest
{
    public class ScrollManager : MonoBehaviour
    {
        public static ScrollManager INSTANCE { get; private set; }
        bool isOpen = false;

        [Header("Animation/Info")]
        [SerializeField] Animator mainAnimate;
        [SerializeField] Animator subAnimate;
        [SerializeField] float speed = 1;


        //UI Elements
        [Space(2)]
        [Header("Quest Line UI")]
        [SerializeField] TextMeshProUGUI questLineName;
        [SerializeField] TextMeshProUGUI giver;
        [SerializeField] CanvasGroup questLog;
        [SerializeField] QuestListItem questListItemPrefab;
        [SerializeField] Transform questListParent;


        [Space(2)]
        [Header("Quest Info UI")]
        [SerializeField] CanvasGroup questInfoPanel; 
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform checkListParent;
        [SerializeField] TextMeshProUGUI questType;
        [SerializeField] TextMeshProUGUI r_gold;
        [SerializeField] TextMeshProUGUI r_xp;
        [SerializeField] Image rewardIcon;

        private void Awake()
        {
            INSTANCE = this;
            
        }

        private void Start()
        {
            QuestLine test = new QuestLine("Test", QuestLine.QuestType.MAIN, "Myself");
            List<Skill> skillz = new List<Skill>();
            skillz.Add(new Skill(name, null));
            test.Skills = skillz;
            test.AddQuest(new Quest("fuck you", true, test));
            QuestManager.INSTANCE.AddQuest(test);
            PopulateQuests();
        }

        public void PopulateQuests()
        {
            if (QuestManager.INSTANCE == null) return;
            if (QuestManager.INSTANCE.selectedQuestLine == null) return;

            //CloseQuestInfo();
            QuestLine l = QuestManager.INSTANCE.selectedQuestLine;
            questLineName.text = l.Name;
            giver.text = l.Giver;

            foreach( Quest q in QuestManager.INSTANCE.selectedQuestLine.ListOfQuests)
            {
                GameObject go = Instantiate(questListItemPrefab.gameObject);
                go.transform.parent = questListParent;
                go.GetComponent<QuestListItem>().Questy = q;
            }

            OpenQuestList();


        }

        public void OpenQuestList()
        {
            questLog.alpha = 1;
            questLog.interactable = questLog.blocksRaycasts = true;
            questInfoPanel.alpha = 0;
            questInfoPanel.blocksRaycasts = questInfoPanel.interactable = false;
            OpenScroll(2f);
        }

        public void OpenQuestInfo(Quest q)
        {
            CloseScroll(2f);
            questLog.alpha = 0;
            questLog.interactable = questLog.blocksRaycasts = false;
            questType.text = (q.isBonus ? "Normal" : "Bonus");
            title.text = q.name;
            questInfoPanel.alpha = 1;
            questInfoPanel.blocksRaycasts = questInfoPanel.interactable = true;
            OpenScroll(2f);
        }

        public void CloseQuestInfo()
        {
            CloseScroll(2f);
            questLog.alpha = 1;
            questLog.interactable = questLog.blocksRaycasts = true;
            questInfoPanel.alpha = 0;
            questInfoPanel.blocksRaycasts = questInfoPanel.interactable = false;
            OpenScroll(2f);
        }

        public void OpenScroll()
        {
            mainAnimate.speed = 1;
            mainAnimate.SetTrigger("open");
        }

        public void CloseScroll()
        {
            mainAnimate.speed = 1;
            mainAnimate.SetTrigger("close");
        }

        public void OpenScroll(float s)
        {
            mainAnimate.speed = s;
            mainAnimate.SetTrigger("open");
        }

        public void CloseScroll(float s)
        {
            mainAnimate.speed = s;
            mainAnimate.SetTrigger("close");

        }
    }
}