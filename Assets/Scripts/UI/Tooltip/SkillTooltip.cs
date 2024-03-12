using UnityEngine;
using TMPro;
using DocumentFormat.OpenXml;

public class SkillTooltip : TooltipBase
{
    [SerializeField]
    TMP_Text Type = null;

    [SerializeField]
    TMP_Text Level = null;

    [SerializeField]
    TMP_Text Cooltime = null;

    [SerializeField]
    PartySelectSkillNode Skill_Card = null;

    public void Initialize(Rect hole, UserHeroSkillData skill_data, bool is_screen_modify = true)
    {
        string name = string.Empty;
        string desc = string.Empty;
        string type = string.Empty;
        string level = string.Empty;
        string cooltime = string.Empty;

        if (skill_data != null)
        {
            name = skill_data.Name;
            desc = skill_data.GetDescription();
            type = skill_data.GetSkillTypeText();

            level = string.Format(GameDefine.GetLocalizeString("system_level_format"), skill_data.GetLevel());
            cooltime = string.Format(GameDefine.GetLocalizeString("system_cooltime_format"), skill_data.GetSkillGroupData().skill_use_delay);

            // 툴팁 안의 아이콘 세팅
            Skill_Card.Initialize(skill_data);
        }

        Initialize(hole, name, desc, is_screen_modify);

        Type.text = type;
        Level.text = level;
        Cooltime.text = cooltime;
    }
}
