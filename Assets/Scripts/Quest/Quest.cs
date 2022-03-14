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

        public int QUEST_LINE_IND = -1;

        public bool isDone = false;

        public Quest(string n, bool b, QuestLine qline)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
           // secondarySkill = null;
            checklist = new List<string>();
        }
        public Quest(string n, bool b, QuestLine qline, int ind)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
           // secondarySkill = null;
            checklist = new List<string>();
            QUEST_LINE_IND = ind;
        }

        public void SetQLine(QuestLine ql)
        {
            questLine = ql;
        }
        public void SetQuestInd(int i)
        {
            QUEST_LINE_IND = i;
        }

        public void SetDone() 
        {
            isDone = true;
        }
    }
}