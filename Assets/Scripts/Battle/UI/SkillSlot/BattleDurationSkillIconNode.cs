using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleDurationSkillIconNode : UIBase
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("지속성 스킬 아이콘")]
    Image Duration_Skill_Icon;

    [SerializeField, Tooltip("같은 지속성 스킬 중첩 횟수")]
    TMP_Text Duplication_Count;

    BattleDurationSkillData Duration_Data;

    public void SetBattleDurationSkillData(BattleDurationSkillData skill)
    {
        Duration_Data = skill;
        UpdateDurationSkillIcon();
    }

    void UpdateDurationSkillIcon()
    {
        if (Duration_Data == null)
        {
            return;
        }
        //  icon
        string icon_path = Duration_Data.GetIconPath();
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(icon_path, (spr) =>
        {
            Duration_Skill_Icon.sprite = spr;
        });

        //  duplication count
        Duplication_Count.text = string.Empty;
    }

    public override void Despawned()
    {
        Duration_Data = null;
    }
}
