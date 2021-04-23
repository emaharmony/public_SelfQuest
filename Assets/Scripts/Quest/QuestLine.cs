using System.Collections;
using System.Collections.Generic;

namespace SelfQuest
{
    public class QuestLine
    {
        public enum QuestType
        {
            RECURSIVE,
            MAIN,
            SECONDARY
        }

        List<Quest> subquests;

        QuestType questType;

        List<Skill> primarySkills;

        string name, giver;

        BigReward reward;


        public QuestLine(string n, QuestType t, string g)
        {
            name = n;
            giver = g;
            questType = t;
            reward = new BigReward();
            subquests = new List<Quest>();
            primarySkills = new List<Skill>();
        }

        public void AddQuest(Quest newQ)
        {
            newQ.SetQLine(this);
            subquests.Add(newQ);
        }

        public string Name { get { return name; } }

        public string Giver { get { return giver; } }

        public QuestType Qtype { get { return questType; } }

        public List<Quest> ListOfQuests { get { return subquests; } set { subquests = value; } }

        public List<Skill> Skills { get { return primarySkills; } set { primarySkills = value; } }

        public BigReward Reward { get { return reward; } set { reward = value; } }
    }
}