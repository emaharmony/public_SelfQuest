using System.Collections;
using System.Collections.Generic;

namespace SelfQuest
{
    public class Quest
    {
        public string name { get; private set; }
        public List<string> checklist { get; set; }

        public bool isBonus = false;
        public Skill secondarySkill { get; set; }
        public Reward reward { get; set; }
        public QuestLine questLine { get; private set; }

        public Quest(string n, bool b, QuestLine qline)
        {
            name = n;
            isBonus = b;
            SetQLine(qline);
            secondarySkill = null;
            checklist = new List<string>();
        }

        public void SetQLine(QuestLine ql)
        {
            questLine = ql;
        }
    }
}