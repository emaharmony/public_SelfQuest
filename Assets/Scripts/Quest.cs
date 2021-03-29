using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{

    string name;
    string[] checklist;
    string description;

    bool isBonus = false;
    Skill[] affected;
    Reward reward;
    QuestLine questLine;

}
