using System;
using System.Collections;
using System.Collections.Generic;

namespace SelfQuest
{
    [Serializable]
    public class Quest
    {
        public string name { get; private set; }
        public List<string> checklist { get; set; }

        public bool isBonus = false;
//        public Skill secondarySkill { get; set; }
        public Reward reward { get; set; }
        public QuestLine questLine { get; private set; }

        public bool isDone = false;

        public Quest(string n, bool b, QuestLine qline)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
           // secondarySkill = null;
            checklist = new List<string>();
            reward = RewardManager.INSTANCE.CreateReward(); 
        }
        public Quest(string n, bool b, QuestLine qline, int ind)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
           // secondarySkill = null;
            checklist = new List<string>();
            reward = RewardManager.INSTANCE.CreateReward();
        }

        public void SetQLine(QuestLine ql)
        {
            questLine = ql;
        }

        public void SetName(string s)
        {
            name = s;
        }

        public void SetReward() 
        {
            reward = RewardManager.INSTANCE.CreateReward();

        }

        public void SetDone() 
        {
            isDone = true;
        }
    }
}