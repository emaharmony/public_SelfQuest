using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLine
{
    enum QuestType
    {
        RECURSIVE,
        MAIN,
        SECONDARY
    }

    List<Quest> subquests;

    QuestType questType;

    Skill[] primarySkills;

    string name, description, giver;

    BigReward reward;

    public void AddQuest(Quest newQ) 
    {
        subquests.Add(newQ);
    }



}
