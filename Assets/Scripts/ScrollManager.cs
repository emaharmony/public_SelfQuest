using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    bool isOpen = false;
   
    [SerializeField]Animator animate;
    [SerializeField] float speed = 1;

    [SerializeField]QuestManager quest;

    //UI Elements
    [SerializeField]CanvasGroup questLog;


    private void Start()
    {
        
        PopulateQuests();
        OpenScroll();
    }

    void PopulateQuests() 
    {
        if (quest == null) return;
        
    }

    public void OpenScroll() 
    {
        animate.SetTrigger("open");

    }

    public void CloseScroll() 
    {
        animate.SetTrigger("close");
    }

}
