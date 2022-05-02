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

            StartCoroutine(PrefManager.INSTANCE.SaveAllPrefs());

        }

        public void RemoveQuest(QuestLine q)
        {
            if (q == null || NoQuests()) return;

            pool.Remove(q);
            StartCoroutine(PrefManager.INSTANCE.SaveAllPrefs());

        }

        public void RemoveCurrentQuestLine() 
        {
            RemoveQuest(currentQuestLine);
            ScrollManager.INSTANCE.PopulateQuests();
        }

        public void RemoveCurrentQuest() 
        {
            pool[currentQuestLine].ListOfQuests.Remove(chosenQuest);
            ScrollManager.INSTANCE.PopulateQuests();
        }

        public void RemoveQuest(int q)
        {
            if (NoQuests() || q < 0) return;
            pool.Remove(pool[q]);
            StartCoroutine(PrefManager.INSTANCE.SaveAllPrefs());

        }

        public void ChangeQuestLine(int dir)
        {
            ScrollManager.INSTANCE.CloseScroll(2f);
            currentQuestLine += (dir > 0 ? 1 : -1);
            currentQuestLine = (currentQuestLine < 0 ? pool.Count - 1 : currentQuestLine % pool.Count);
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
        }

        public void FinishQuestLine() 
        {
            SoundManager.INSTANCE.FinishQuest();
            PlayerManager.INSTANCE.GivePlayerRewards(pool[currentQuestLine].Skill, /*null,*/ pool[currentQuestLine].Reward.EXP, pool[currentQuestLine].Reward.GOLD);
            RemoveQuest(pool[currentQuestLine]);
            currentQuestLine = 0;
            ScrollManager.INSTANCE.PopulateQuests();
        }

    }

}