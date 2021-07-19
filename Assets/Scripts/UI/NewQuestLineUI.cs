using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewQuestLineUI : MonoBehaviour
{
    public TMP_InputField qname, giver;
    [SerializeField] TMP_Dropdown qtype;
    List<int> skillz;
    [SerializeField] Transform subQuestParent;
    [SerializeField] GameObject subQuestListPrefab;

    public void AddSkill(int i)
    {
        if(!skillz.Contains(i))
        skillz.Add(i);
    }
    
}
