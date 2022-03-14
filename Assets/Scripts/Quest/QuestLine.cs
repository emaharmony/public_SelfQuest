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
            Reward = null;
            ListOfQuests = new List<Quest>();
        }

        public QuestLine(string n, QuestType t, string g)
        {
            Name = n;
            Giver = g;
            Qtype = t;
            ListOfQuests = new List<Quest>();
        }

        public void AddQuest(Quest newQ)
        {
            newQ.SetQLine(this);
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