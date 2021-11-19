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

        List<GameObject> questListItems = new List<GameObject>();

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

        [Space(2)]
        [Header("New Quest UI")]
        [SerializeField] GameObject newLineUI;
        [SerializeField] GameObject newSubUI;
        

        private void Awake()
        {
            INSTANCE = this;
            
        }

        private void Start()
        {
            QuestLine test = new QuestLine("Test", QuestLine.QuestType.MAIN, "Myself");
            List<Skill> skillz = new List<Skill>();
            skillz.Add(new Skill(name, null));
            test.Skill = skillz[0];
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

            if (questListItems.Count > 0)
            {
                for (int i = 0; i < questListItems.Count; i++)
                    Destroy(questListItems[i]);

                questListItems = new List<GameObject>();
            }

            foreach( Quest q in QuestManager.INSTANCE.selectedQuestLine.ListOfQuests)
            {
                GameObject go = Instantiate(questListItemPrefab.gameObject);
                go.transform.SetParent(questListParent);
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                go.GetComponent<QuestListItem>().Questy = q;
                questListItems.Add(go);
            }

            OpenQuestList();
        }

        public void OpenQuestList()
        {
            questLog.alpha = 1;
            questLog.interactable = questLog.blocksRaycasts = true;
            questInfoPanel.alpha = 0;
            questInfoPanel.blocksRaycasts = questInfoPanel.interactable = false;
            CloseSubScroll(2f);
            Invoke("OpenScroll",0.5f);

        }

        public void OpenQuestInfo(Quest q)
        {
            questLog.alpha = 0;
            questLog.interactable = questLog.blocksRaycasts = false;
            questType.text = (q.isBonus ? "Normal" : "Bonus");
            title.text = q.name;
            questInfoPanel.alpha = 1;
            questInfoPanel.blocksRaycasts = questInfoPanel.interactable = true;
            
        }

        public void CloseQuestInfo()
        {
            questLog.alpha = 1;
            questLog.interactable = questLog.blocksRaycasts = true;
            questInfoPanel.alpha = 0;
            questInfoPanel.blocksRaycasts = questInfoPanel.interactable = false;
        }

        public void OpenScroll()
        {
            mainAnimate.speed = 1;
            mainAnimate.SetBool("isOpen", true);
        }

        public void CloseScroll()
        {
            mainAnimate.speed = 1;
            mainAnimate.SetBool("isOpen", false);
        }

        public void OpenScroll(float s)
        {
            mainAnimate.speed = s;
            mainAnimate.SetBool("isOpen", true);
        }

        public void CloseScroll(float s)
        {
            mainAnimate.speed = s;
            mainAnimate.SetBool("isOpen", false);

        }

        public void OpenSubScroll()
        {

            subAnimate.speed = 1;
            subAnimate.SetBool("isOpen", true);
        }

        public void CloseSubScroll()
        {
            subAnimate.speed = 1;
            subAnimate.SetBool("isOpen", false);
        }

        public void OpenSubScroll(float s)
        {
            subAnimate.speed = s;
            subAnimate.SetBool("isOpen", true);
        }

        public void CloseSubScroll(float s)
        {
            subAnimate.speed = s;
            subAnimate.SetBool("isOpen", false);
        }
        public void OpenNewQuestLineUIMenu()
        {
            CloseScroll();
            Invoke("OpenSubScroll", 0.5f);
            newLineUI.SetActive(true);
            newSubUI.SetActive(false);

        }

        public void OpenNewSkillMenu()
        {

        }

        public void CloseNewSkillMenu()
        {

        }

        public void TurnOnNewQuestLine()
        {
            newLineUI.SetActive(true);
            newSubUI.SetActive(false);
        }

        public void TurnOnNewQuest()
        {
            newLineUI.SetActive(false);
            newSubUI.SetActive(true);
        }
    }
}