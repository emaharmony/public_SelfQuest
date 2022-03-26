using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager INSTANCE { get; private set; }
        int currentQuestLine = 0;
        public List<QuestLine> pool;

        public Quest chosenQuest { get; set; }

        private void Awake()
        {
            INSTANCE = this;
            pool = new List<QuestLine>();

        }

        public bool NoQuests() 
        {
            return pool.Count == 0;
        }

        public void AddQuest(QuestLine newQ)
        {
            if (pool.Contains(newQ)) return;
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
            if (i < 0 || pool.Count <= 0) return null;

            return pool[i];
        }

        public QuestLine selectedQuestLine
        {
            get { if (pool.Count > 0) return pool[currentQuestLine]; else return null; }
        }

        public void NextQuestLine()
        {
            currentQuestLine = (currentQuestLine + 1) % pool.Count;
            ScrollManager.INSTANCE.PopulateQuests();
        }

        public void PrevQuestLine()
        {
            currentQuestLine = Mathf.Abs( (currentQuestLine - 1) % pool.Count);
            ScrollManager.INSTANCE.PopulateQuests();
        }
        public void FinishQuest() 
        {
           if (chosenQuest.isDone) return;

           PlayerManager.INSTANCE.GivePlayerRewards(pool[currentQuestLine].Skill, /*chosenQuest.secondarySkill,*/ chosenQuest.reward.EXP, chosenQuest.reward.GOLD);
           pool[currentQuestLine].ListOfQuests.Remove(chosenQuest);
           ScrollManager.INSTANCE.PopulateQuests();
           chosenQuest.SetDone();
            SoundManager.INSTANCE.FinishQuest();
        }

        public void FinishQuestLine() 
        {
           
            PlayerManager.INSTANCE.GivePlayerRewards(pool[currentQuestLine].Skill, /*null,*/ pool[currentQuestLine].Reward.EXP, pool[currentQuestLine].Reward.GOLD);
            RemoveQuest(pool[currentQuestLine]);
            ScrollManager.INSTANCE.PopulateQuests();
            SoundManager.INSTANCE.FinishQuest();

        }
    }

}