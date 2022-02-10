using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SelfQuest
{
    public class QuestListItem : MonoBehaviour
    {

        Quest linkedQuest;
        public Quest Questy{ get { return linkedQuest; }
            set { linkedQuest = value;  Name.text = value.name;  } }

        [SerializeField]TextMeshProUGUI Name;
        Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void SetQuest(int i)
        {
        }

        public void PrepQuestInfoScreen()
        {
            ScrollManager.INSTANCE.OpenQuestInfo(linkedQuest);
        }

    }
}