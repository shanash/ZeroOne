using FluffyDuck.Util;
using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillInfoTab : MonoBehaviour
{
    [SerializeField, Tooltip("스킬 타입")]
    SKILL_TYPE Skill_Type = SKILL_TYPE.NONE;
    [SerializeField]
    string Default_Name = string.Empty;

    [SerializeField]
    CanvasGroup skill_contents_group = null;

    [SerializeField]
    GameObject lock_icon = null;

    [SerializeField]
    Image Icon = null;

    [SerializeField]
    TMP_Text Name = null;

    BattlePcSkillGroup Skill_Group;

    public void SetBattlePcSkillGroup(BattlePcSkillGroup skill_group)
    {
        Skill_Group = skill_group;
        UpdateSkillTab();
    }

    void UpdateSkillTab()
    {
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Skill_Group.GetSkillIconPath(), (spr) =>
        {
            Icon.sprite = spr;
        });

        Name.text = Skill_Group.GetSkillActionName();
    }

    public SKILL_TYPE GetSkillType()
    {
        return Skill_Type;
    }

    public void OnChangeValue(bool is_on)
    {
        skill_contents_group.alpha = is_on ? 1f : 0.3f;
    }

    public void OnChangeBlock(bool is_blocked)
    {
        lock_icon.SetActive(is_blocked);
    }
}
