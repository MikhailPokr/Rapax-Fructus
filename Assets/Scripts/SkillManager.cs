using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RapaxFructus
{
    [Serializable]
    internal class SkillListElement
    {
        public Skill Skill;
        public Image Button;
    }
    internal class SkillManager : MonoBehaviour
    {
        [SerializeField] private GameObject _skillsMenu;
        [SerializeField] private Transform _skillParent;
        public List<SkillListElement> SkillList;

        private void Start()
        {
            List<SkillListElement> skills = SkillList.FindAll(x => x.Skill.PowerUpObject.Unlock);
            for (int i = 0; i < skills.Count; i++)
            {
                Skill realSkill = Instantiate(skills[i].Skill, _skillParent);
                realSkill.SetButton(skills[i].Button);
                realSkill.OnResourseCountChanged();
                realSkill.Activate();
                SkillList.Find(x => x == skills[i]).Skill = realSkill;
            }
            if (skills.Count == 0)
            {
                _skillsMenu.SetActive(false);
            }
        }
        public void Click(Image image)
        {
            SkillList.Find(x => x.Button == image).Skill.Activate();
        }
    }
}