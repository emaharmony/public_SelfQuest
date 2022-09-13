using System;
using System.Collections;
using System.Collections.Generic;

namespace SelfQuest
{
    [Serializable]
    public class Quest
    {
        public string name { get; private set; }
        public string description { get; set; }

        public bool isBonus = false;
//        public Skill secondarySkill { get; set; }
        public Reward reward { get; set; }
        public QuestLine questLine { get; private set; }

        public bool isDone = false;

        public int numberOfcomplete { get; set; }
        public int currentNum { get; set;  }

        public Quest(string n, string d, bool b, QuestLine qline)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
           // secondarySkill = null;
            description = d;
            reward = RewardManager.INSTANCE.CreateReward();
            currentNum = 0;
            numberOfcomplete = 1;
        }
        public Quest(string n, string d, bool b, QuestLine qline, int cum)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
            numberOfcomplete = cum;
            currentNum = 0;
            description = d;
            reward = RewardManager.INSTANCE.CreateReward();
        }

        public Quest(string n, string d, bool b, QuestLine qline, int cum, int curr)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
            numberOfcomplete = cum;
            currentNum = curr;
            description = d;
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