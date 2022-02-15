using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelfQuest {
    public class SkillButton : MonoBehaviour
    {
        public int ID = -1;
        public void EditSkill()
        {
            SkillManager.INSTANCE.GetSkill(ID);
        }
    }
}