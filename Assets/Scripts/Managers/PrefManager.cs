using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class PrefManager : MonoBehaviour
    {
        PlayerManager player;
        SkillManager skills;
        QuestManager quests;

        //Player Prefs
        const string NEW_PLAYER = "isNewPlayer", PREF_NAME = "playerName", PREF_LEVEL = "playerLvl", CURR_EXP_PREF = "current_exp", PREF_GOLD = "gold";
        
        //Skill Prefs
        const string PREF_SKILL_COUNT = "skillCount", PREF_SKILL_NAME = "skillName", PREF_SKILL_LEVEL = "skillLevel", PREF_SKILL_EXP = "skillEXP";

        //QuestPrefs

        const string PREF_QUESTLINE_SKILL = "QLINESkill", PREF_QUESTLINE_COUNT = "QLCount", PREF_QLNAME = "QLName", PREF_QLGIVER = "QLGiver", PREF_QCOUNT = "QCount", PREF_QNAME = "QName", PREF_QL_REWARD_GOLD = "QLRGOLD",
            PREF_QL_REWARD_EXP = "QLREXP", PREF_Q_REWARD_GOLD = "QRGold", PREF_Q_REWARD_EXP = "QREXP", PREF_QUESTLINE_QTYPE = "QLType";

        void Awake() 
        {
            player = PlayerManager.INSTANCE;
            skills = SkillManager.INSTANCE;
            quests = QuestManager.INSTANCE;

        }

        void Start() 
        {
            LoadAllPrefs();
        }

        public void LoadAllPrefs()
        {
            Debug.Log("PrefsLoaded");
            LoadUserPrefs();
            LoadSkillPrefs();
            LoadQuestPrefs();
        }

        public void LoadQuestPrefs()
        {
            if (PlayerPrefs.GetInt(PREF_QUESTLINE_COUNT, -1) <= -1) 
            {
                QuestLine test = new QuestLine("Commence a Self Quest", QuestLine.QuestType.MAIN, "Myself");
                List<Skill> skillz = new List<Skill>();
                skillz.Add(new Skill("", Color.green));
                test.Skill = skillz[0];
                test.AddQuest(new Quest("fuck you", true, test));
                test.ListOfQuests[0].reward = RewardManager.INSTANCE.CreateReward();
                test.Reward = RewardManager.INSTANCE.CreateBigReward();
                QuestManager.INSTANCE.AddQuest(test);
            } else{
                quests.pool = new List<QuestLine>();
                for (int i = 0; i < PlayerPrefs.GetInt(PREF_QUESTLINE_COUNT); i++)
                {
                    quests.pool.Add(new QuestLine());
                    quests.pool[i].Name = PlayerPrefs.GetString(PREF_QLNAME + i);
                    quests.pool[i].Giver = PlayerPrefs.GetString(PREF_QLGIVER + i);
                    quests.pool[i].Skill = skills.pool[PlayerPrefs.GetInt(PREF_QUESTLINE_SKILL + i)];
                    quests.pool[i].Qtype = (QuestLine.QuestType)PlayerPrefs.GetInt(PREF_QUESTLINE_QTYPE + i);
                    quests.pool[i].Reward = new Reward(PlayerPrefs.GetInt(PREF_QL_REWARD_EXP + i), PlayerPrefs.GetInt(PREF_QL_REWARD_EXP + i));
                    List<Quest> qList = new List<Quest>(PlayerPrefs.GetInt(PREF_QCOUNT + i));
                    for (int j = 0; j < qList.Count; j++)
                    {
                        Debug.Log(PREF_QNAME + "-" + i + "-" + j);
                        qList[j] = new Quest(PlayerPrefs.GetString(PREF_QNAME + "-" + i + "-" + j), false, quests.pool[i]);
                        qList[j].reward = new Reward(PlayerPrefs.GetInt(PREF_Q_REWARD_EXP + "-" + i + "-" + j), PlayerPrefs.GetInt(PREF_Q_REWARD_GOLD + "-" + i + "-" + j));
                    }

                    quests.pool[i].ListOfQuests = qList;
                }
            }
        }

        public void LoadSkillPrefs()
        {
            //Get pool of added Skills;
            if (PlayerPrefs.GetInt(PREF_SKILL_COUNT, -1) <= 0)
            {
                skills.AddSkill(new Skill("swole", Color.gray));
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
                    skills.pool[i].LVL = PlayerPrefs.GetInt(PREF_SKILL_LEVEL + i);
                }
            }
        }

        public void LoadUserPrefs()
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
                player.overallLvl = PlayerPrefs.GetInt(PREF_LEVEL);
                player.currExp = PlayerPrefs.GetInt(CURR_EXP_PREF);
                player.currGold = PlayerPrefs.GetInt(PREF_GOLD);
                player.playerName = PlayerPrefs.GetString(PREF_NAME);
            }
        }

        public void SaveAllPrefs() 
        {
            SaveUserPrefs();
            SaveSkillPrefs();
            SaveQuestPrefs();
        }

        public void SaveSkillPrefs()
        {
            if (skills == null) return;
            PlayerPrefs.SetInt(PREF_SKILL_COUNT, skills.pool.Count);
            for (int i = 0; i < skills.pool.Count; i++)
            {
                PlayerPrefs.SetString(PREF_SKILL_NAME + i, skills.pool[i].Name);
                PlayerPrefs.SetInt(PREF_SKILL_EXP + i, skills.pool[i].EXP);
                PlayerPrefs.SetInt(PREF_SKILL_LEVEL + i, skills.pool[i].LVL);
            }
        }

        public void SaveQuestPrefs() 
        {
            if (quests == null) return;
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
                    Debug.Log(PREF_QNAME + "-" + i + "-" + j);
                    PlayerPrefs.SetString((PREF_QNAME + "-" + i + "-" + j), quests.pool[i].ListOfQuests[j].name);
                    PlayerPrefs.SetInt(PREF_Q_REWARD_EXP + "-" + i + "-" + j, quests.pool[i].ListOfQuests[j].reward.EXP);
                    PlayerPrefs.SetInt(PREF_Q_REWARD_GOLD + "-" + i + "-" + j, quests.pool[i].ListOfQuests[j].reward.GOLD);
                }
            }
        }

        public void SaveUserPrefs() 
        {
            if (player == null) return;
            PlayerPrefs.SetInt(NEW_PLAYER, 1);
            PlayerPrefs.SetInt(PREF_LEVEL, player.overallLvl);
            PlayerPrefs.SetInt(PREF_GOLD, player.currGold);
            PlayerPrefs.SetInt(CURR_EXP_PREF, player.currExp);
            PlayerPrefs.SetString(PREF_NAME, player.playerName == null ? "" : player.playerName);
        }

        void OnApplicationQuit()
        {
            SaveAllPrefs();
        }
    }
}