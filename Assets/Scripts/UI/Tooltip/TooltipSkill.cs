using UnityEngine;
using TMPro;

public class TooltipSkill : Tooltip
{
    [SerializeField]
    TMP_Text Type = null;

    [SerializeField]
    TMP_Text Level = null;

    [SerializeField]
    TMP_Text Cooltime = null;

    [SerializeField]
    PartySelectSkillNode Skill_Card = null;

    public void Initialize(Rect hole, UserHeroSkillData skill_data)
    {
        Initialize(hole, skill_data.Name, skill_data.GetDescription());

        Skill_Card.Initialize(skill_data);

        Type.text = skill_data.GetSkillTypeText();
        Level.text = $"Lv.{skill_data.GetLevel()}";
        Cooltime.text = $"쿨타임 {skill_data.GetSkillGroupData().skill_use_delay}초";
    }
}
