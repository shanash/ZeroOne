using TMPro;
using UnityEngine;

namespace UI.SkillLevelPopup
{
    public class SkillInfo : MonoBehaviour
    {
        [SerializeField, Tooltip("스킬 레벨")]
        TMP_Text Skill_Level;

        UserHeroSkillData Data;

        public void SetData(UserHeroSkillData data)
        {
            Data = data;

            Skill_Level.text = $"Lv.{Data.GetLevel()}";
        }
    }
}
