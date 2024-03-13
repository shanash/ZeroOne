using TMPro;
using UnityEngine;

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

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2 || data[0] is not Rect)
        {
            return false;
        }

        UserHeroSkillData skill = null;

        if (data[1] != null && data[1] is UserHeroSkillData)
        {
            skill = data[1] as UserHeroSkillData;
        }

        Initialize((Rect)data[0], skill);

        return true;
    }

    public void Initialize(Rect hole, UserHeroSkillData skill_data, bool is_screen_modify = true)
    {
        string name = "EMPTY";
        string desc = "EMPTY";
        string type = "EMPTY";
        string level = "EMPTY";
        string cooltime = "EMPTY";

        if (skill_data != null)
        {
            name = skill_data.Name;
            desc = skill_data.GetDescription();
            type = skill_data.GetSkillTypeText();

            level = string.Format(GameDefine.GetLocalizeString("system_level_format"), skill_data.GetLevel());
            cooltime = string.Format(GameDefine.GetLocalizeString("system_cooltime_format"), skill_data.GetSkillGroupData().skill_use_delay);
        }

        Skill_Card.Initialize(skill_data);
        Initialize(hole, name, desc, is_screen_modify);

        Type.text = type;
        Level.text = level;
        Cooltime.text = cooltime;
    }
}
