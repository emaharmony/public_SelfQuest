using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelfQuest
{
    public class SkillListItem : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI name, level;
        [SerializeField] Image color;
        public int attachedIndex;

        public void SetText(string nameS, string lvl)
        {
            name.text = nameS;
            level.text = lvl;
        }

        public void SetColor(Color c)
        {
            color.color = c;
        }

    }
}