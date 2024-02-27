using Cysharp.Text;
using TMPro;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    [SerializeField, Tooltip("스킬 레벨")]
    TMP_Text Skill_Level;

    [SerializeField, Tooltip("쿨타임")]
    TMP_Text Skill_Cooltime;

    [SerializeField, Tooltip("스킬 정보")]
    TMP_Text Skill_Info;

    BattlePcSkillGroup Skill_Group;

    public void SetBattlePcSkillGroup(BattlePcSkillGroup skill)
    {
        Skill_Group = skill;
        UpdateSkillInfo();
    }

    void UpdateSkillInfo()
    {
        if (Skill_Group == null)
        {
            ClearInfo();
            return;
        }
        //  level
        Skill_Level.text = ZString.Format("Lv.{0}", Skill_Group.GetSkillLevel());

        //  cooltime
        Skill_Cooltime.text = ZString.Format("쿨타임 {0}초", Skill_Group.GetCooltime());

        //  skill info [todo]

    }

    void ClearInfo()
    {
        //  level
        Skill_Level.text = "Lv.1";

        //  cooltime
        Skill_Cooltime.text = string.Empty;

        //  skill info [todo]
        Skill_Info.text = string.Empty;
    }


}
