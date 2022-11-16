using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class PrefManager : MonoBehaviour
    {
        public static PrefManager INSTANCE { get; private set; }
        PlayerManager player;
        SkillManager skills;
        QuestManager quests;

        [SerializeField]GameObject iphoneNew, iphoneOld;

        //Player Prefs
        const string NEW_PLAYER = "isNewPlayer", PREF_NAME = "playerName", PREF_LEVEL = "playerLvl", CURR_EXP_PREF = "current_exp", PREF_GOLD = "gold";
        
        //Skill Prefs
        const string PREF_SKILL_COUNT = "skillCount", PREF_SKILL_NAME = "skillName", PREF_SKILL_LEVEL = "skillLevel", PREF_SKILL_EXP = "skillEXP", PREF_SKILL_COLOR = "skillColor";

        //QuestPrefs

        const string PREF_QUESTLINE_SKILL = "QLINESkill", PREF_QUESTLINE_COUNT = "QLCount", PREF_QLNAME = "QLName", PREF_QLGIVER = "QLGiver", PREF_QCOUNT = "QCount", PREF_QNAME = "QName", PREF_QL_REWARD_GOLD = "QLRGOLD",
            PREF_QL_REWARD_EXP = "QLREXP", PREF_Q_REWARD_GOLD = "QRGold", PREF_Q_REWARD_EXP = "QREXP", PREF_QUESTLINE_QTYPE = "QLType", PREF_QDescr="QDescr", PREF_QMAX = "QMax", PREF_QCURR="QCurr";

        const string SETTINGS_NOTIFICATION = "SET_NOTI";
        bool _userDone = false, _skillDone = false, _questDone = false;
        void Awake() 
        {
            INSTANCE = this;
            player = PlayerManager.INSTANCE;
            skills = SkillManager.INSTANCE;
            quests = QuestManager.INSTANCE;
#if UNITY_IOS
            float phoneType = float.Parse(SystemInfo.deviceModel.Substring(6).Replace(',', '.'));
            if(phoneType < 10)
            {
                Destroy(iphoneNew);
                iphoneOld.SetActive(true);
            } else 
            {
                Destroy(iphoneOld);
                iphoneNew.SetActive(true);
            }
#else
            Destroy(iphoneOld);
            iphoneNew.SetActive(true);
#endif



        }

        void Start() 
        {
           StartCoroutine( LoadAllPrefs());
        }

        public IEnumerator LoadAllPrefs()
        {
            while(!LoadSkillPrefs()) yield return null;
            while(!LoadUserPrefs()) yield return null;
            while(!LoadQuestPrefs()) yield return null;
           // NotificationManager.INSTANCE.hours = PlayerPrefs.GetInt(SETTINGS_NOTIFICATION);

        }

        public bool LoadQuestPrefs()
        {
            if (!_skillDone) return false;

            if (PlayerPrefs.GetInt(PREF_QUESTLINE_COUNT, -1) <= 0) 
            {
                QuestLine test = new QuestLine("Commence a Self Quest", QuestLine.QuestType.MAIN, "Myself");
                test.Skill = skills.pool[0];
                test.AddQuest(new Quest("Look At this Quest", "IF you can read it you've done it! Now just click the 'Done' button at the bottom to complete the Quest.", true, test));
                test.AddQuest(new Quest("Touch Blue Flag", "Look to the top left of your phone next to the title of the quest line. This is your Stats screen, you will find your level, gold, skills and the settings in this menu.", true, test));
                test.AddQuest(new Quest("Make New Skill", "Going back to the blue Flag there is a button at the bottom left of the menu. This is where you will find your skill menu. Click the Gauntlet to ctreate your own Skill.", true, test));
                test.AddQuest(new Quest("Create New Quest Line", "Now on the other side of the Title there is a '+' sign. Click it to create a new QuestLine. Within the QuestLine menu there is a yellow '+' that is used to create subquests."  , true, test));
                test.AddQuest(new Quest("Finish Quest Line", "You've got the very basics done! When youre down go to the Scroll Menu make sure you are at the correct quest line and click the chest at the bottom of the screen to obtain your reward! Happy Questing! :)", true, test));
                QuestManager.INSTANCE.AddQuest(test);
            } else{
                quests.pool = new List<QuestLine>();
                int qCount = PlayerPrefs.GetInt(PREF_QUESTLINE_COUNT);
                for (int i = 0; i < qCount; i++)
                {
                    quests.pool.Add(new QuestLine());
                    quests.pool[i].Name = PlayerPrefs.GetString(PREF_QLNAME + i);
                    quests.pool[i].Giver = PlayerPrefs.GetString(PREF_QLGIVER + i);
                    quests.pool[i].Skill = skills.pool[PlayerPrefs.GetInt(PREF_QUESTLINE_SKILL + i) < 0 ? 0 : PlayerPrefs.GetInt(PREF_QUESTLINE_SKILL + i)];
                    quests.pool[i].Qtype = (QuestLine.QuestType)PlayerPrefs.GetInt(PREF_QUESTLINE_QTYPE + i);
                    quests.pool[i].Reward = new Reward(PlayerPrefs.GetInt(PREF_QL_REWARD_EXP + i), PlayerPrefs.GetInt(PREF_QL_REWARD_EXP + i));
                    int count = PlayerPrefs.GetInt(PREF_QCOUNT + i);
                    List<Quest> qList = new List<Quest>();
                    for (int j = 0; j < count; j++)
                    {
                        qList.Add( new Quest(PlayerPrefs.GetString(PREF_QNAME + "-" + i + "-" + j), PlayerPrefs.GetString(PREF_QDescr + "-" + i + "-" + j), false, quests.pool[i], PlayerPrefs.GetInt(PREF_QMAX + "-" + i + "-" + j), PlayerPrefs.GetInt(PREF_QCURR + "-" + i + "-" + j)));
                        qList[j].reward = new Reward(PlayerPrefs.GetInt(PREF_Q_REWARD_EXP + "-" + i + "-" + j), PlayerPrefs.GetInt(PREF_Q_REWARD_GOLD + "-" + i + "-" + j));
                    }

                    quests.pool[i].ListOfQuests = qList;
                }
            }
            _questDone = true;

            return true;
        }

        public bool LoadSkillPrefs()
        {
           
            //Get pool of added Skills;
            if (PlayerPrefs.GetInt(PREF_SKILL_COUNT, -1) <= 0)
            {
                skills.AddSkill(new Skill("Self-Improvement", Color.gray));
            }
            else
            {
                int count = PlayerPrefs.GetInt(PREF_SKILL_COUNT);
                skills.pool = new List<Skill>();
                for (int i = 0; i < count; i++)
                {
                    skills.pool.Add(new Skill("", Color.green));
                    skills.pool[i].Name = PlayerPrefs.GetString(PREF_SKILL_NAME + i);
                    skills.pool[i].EXP = PlayerPrefs.GetInt(PREF_SKILL_EXP + i);
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(PREF_SKILL_COLOR + i), out color);
                    skills.pool[i].SkillColor = color;
                }
            }
            _skillDone = true;

            return true;
        }

        public bool LoadUserPrefs()
        {
            if (PlayerPrefs.GetInt(NEW_PLAYER, 0) == 0)
            {
                player.returningPlayer = 1;
                player.overallLvl = 1;
                player.currExp = 0;
                player.nextLvlEXP = 0;
                player.currGold = 0;
            }
            else             //grab values from pref
            {
                player.returningPlayer = PlayerPrefs.GetInt(NEW_PLAYER);
                player.currExp = PlayerPrefs.GetInt(CURR_EXP_PREF);
                player.currGold = PlayerPrefs.GetInt(PREF_GOLD);
                player.playerName = PlayerPrefs.GetString(PREF_NAME);
            }
            _userDone = true;
            return true;
        }

        public IEnumerator SaveAllPrefs() 
        {
            while (!SaveUserPrefs()) yield return new WaitForEndOfFrame();
            while (!SaveSkillPrefs()) yield return new WaitForEndOfFrame();
            while(!SaveQuestPrefs()) yield return new WaitForEndOfFrame();
      //      PlayerPrefs.SetInt(SETTINGS_NOTIFICATION, NotificationManager.INSTANCE.hours);
        }

        public bool SaveSkillPrefs()
        {

            if (skills == null || !_skillDone) return false;
            PlayerPrefs.SetInt(PREF_SKILL_COUNT, skills.pool.Count);
            for (int i = 0; i < skills.pool.Count; i++)
            {
                PlayerPrefs.SetString(PREF_SKILL_NAME + i, skills.pool[i].Name);
                PlayerPrefs.SetInt(PREF_SKILL_EXP + i, skills.pool[i].EXP);
                PlayerPrefs.SetInt(PREF_SKILL_LEVEL + i, skills.pool[i].LVL);
                PlayerPrefs.SetString(PREF_SKILL_COLOR + i, ColorUtility.ToHtmlStringRGB(skills.pool[i].SkillColor)); // without alpha
            }

            return true;
        }

        public bool SaveQuestPrefs() 
        {
            if (quests == null || !_questDone) return false;
            PlayerPrefs.SetInt(PREF_QUESTLINE_COUNT, quests.pool.Count); 
            for (int i = 0; i < quests.pool.Count; i++)
            {
                PlayerPrefs.SetString(PREF_QLNAME + i, quests.pool[i].Name);
                PlayerPrefs.SetString(PREF_QLGIVER + i, quests.pool[i].Giver);
                PlayerPrefs.SetInt(PREF_QUESTLINE_SKILL + i, skills.pool.IndexOf(quests.pool[i].Skill));
                PlayerPrefs.SetInt(PREF_QUESTLINE_QTYPE + i, (int)quests.pool[i].Qtype);
                PlayerPrefs.SetInt(PREF_QL_REWARD_EXP + i, quests.pool[i].Reward.EXP);
                PlayerPrefs.SetInt(PREF_QL_REWARD_GOLD + i, quests.pool[i].Reward.GOLD);
                PlayerPrefs.SetInt(PREF_QCOUNT + i, quests.pool[i].ListOfQuests.Count);
                for (int j = 0; j < quests.pool[i].ListOfQuests.Count; j++)
                {
                    PlayerPrefs.SetString((PREF_QNAME + "-" + i + "-" + j), quests.pool[i].ListOfQuests[j].name);
                    PlayerPrefs.SetString((PREF_QDescr + "-" + i + "-" + j), quests.pool[i].ListOfQuests[j].description);
                    PlayerPrefs.SetInt((PREF_QMAX + "-" + i + "-" + j), quests.pool[i].ListOfQuests[j].numberOfcomplete);
                    PlayerPrefs.SetInt((PREF_QCURR + "-" + i + "-" + j), quests.pool[i].ListOfQuests[j].currentNum);
                    PlayerPrefs.SetInt(PREF_Q_REWARD_EXP + "-" + i + "-" + j, quests.pool[i].ListOfQuests[j].reward.EXP);
                    PlayerPrefs.SetInt(PREF_Q_REWARD_GOLD + "-" + i + "-" + j, quests.pool[i].ListOfQuests[j].reward.GOLD);
                }
            }

            return true;
        }

        public bool SaveUserPrefs() 
        {
            if (player == null || !_userDone) return false;
            PlayerPrefs.SetInt(NEW_PLAYER, 1);
            PlayerPrefs.SetInt(PREF_GOLD, player.currGold);
            PlayerPrefs.SetInt(CURR_EXP_PREF, player.currExp);
            PlayerPrefs.SetString(PREF_NAME, player.playerName == null ? "" : player.playerName);

            return true;
        }

        void OnApplicationQuit() { StartCoroutine (SaveAllPrefs()); }


        private void OnApplicationFocus(bool focus)
        {
            StartCoroutine(SaveAllPrefs());
        }
    }
}