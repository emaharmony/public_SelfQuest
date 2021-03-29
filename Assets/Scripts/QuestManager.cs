using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    int currentQuestLine = 0;

    List<QuestLine> pool;


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

    public void ChangeQuest(int dir) 
    {
        currentQuestLine += (dir > 0 ? 1 : -1);
        //populate Quest UI

    }

}

