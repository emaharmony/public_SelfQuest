using System.Collections;
using System.Collections.Generic;
using System;

namespace SelfQuest
{
    [Serializable]
    public class QuestLine
    {
        /// <summary>
        /// Class that defines a questLine objects
        /// </summary>
        [Serializable]
        public enum QuestType
        {
            RECURSIVE,
            MAIN,
            SECONDARY
        }

        public QuestLine() 
        {
            Name = "";
            Giver = "";
            Qtype = QuestType.MAIN;
            ListOfQuests = new List<Quest>();

            if (RewardManager.INSTANCE != null)
                Reward = RewardManager.INSTANCE.CreateReward();
        }

        public QuestLine(string n, QuestType t, string g)
        {
            Name = n;
            Giver = g;
            Qtype = t;
            ListOfQuests = new List<Quest>();
            Reward = RewardManager.INSTANCE.CreateReward();
        }

        public void AddQuest(Quest newQ)
        {
            newQ.SetQLine(this);
            if(!ListOfQuests.Contains(newQ))
                ListOfQuests.Add(newQ);
        }

        public string Name;

        public string Giver;
        public QuestType Qtype;

        public List<Quest> ListOfQuests;

        public Skill Skill;

        public Reward Reward;
    }
}