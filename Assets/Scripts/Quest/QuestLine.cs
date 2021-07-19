using System.Collections;
using System.Collections.Generic;
using System;

namespace SelfQuest
{
    public class QuestLine
    {
        [Serializable]
        public enum QuestType
        {
            RECURSIVE,
            MAIN,
            SECONDARY
        }

        public QuestLine(string n, QuestType t, string g)
        {
            Name = n;
            Giver = g;
            Qtype = t;
            Reward = new BigReward();
            ListOfQuests = new List<Quest>();
        }

        public void AddQuest(Quest newQ)
        {
            newQ.SetQLine(this);
            ListOfQuests.Add(newQ);
        }

        public string Name { get; }

        public string Giver { get; }

        public QuestType Qtype { get; }

        public List<Quest> ListOfQuests { get; set; }

        public Skill Skill { get; set; }

        public BigReward Reward { get; set; }
    }
}