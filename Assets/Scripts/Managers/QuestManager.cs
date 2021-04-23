using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager INSTANCE { get; private set; }
        int currentQuestLine = 0;

        List<QuestLine> pool;

        private void Awake()
        {
            INSTANCE = this;
            pool = new List<QuestLine>();

        }

        public void AddQuest(QuestLine newQ)
        {
            pool.Add(newQ);

        }

        public void RemoveQuest(QuestLine q)
        {
            pool.Remove(q);
        }

        public void RemoveQuest(int q)
        {
            pool.Remove(pool[q]);
        }

        public void ChangeQuestLine(int dir)
        {
            ScrollManager.INSTANCE.CloseScroll(2f);
            currentQuestLine += (dir > 0 ? 1 : -1);
            currentQuestLine %= pool.Count;

            //populate Quest UI
            ScrollManager.INSTANCE.PopulateQuests();
            ScrollManager.INSTANCE.OpenScroll();

        }

        public QuestLine GetQuestLine(int i)
        {
            return pool[i];
        }

        public QuestLine selectedQuestLine
        {
            get { if (pool.Count > 0) return pool[currentQuestLine]; else return null; }
        }
    }

}