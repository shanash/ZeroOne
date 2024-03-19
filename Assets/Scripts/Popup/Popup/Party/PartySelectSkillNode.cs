using Cysharp.Text;
using Cysharp.Threading.Tasks;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySelectSkillNode : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Skill Type")]
    SKILL_TYPE Skill_Type = SKILL_TYPE.NONE;

    [SerializeField, Tooltip("Skill Type Text")]
    TMP_Text Skill_Type_Text;

    [SerializeField, Tooltip("Skill Icon")]
    Image Skill_Icon;

    [SerializeField, Tooltip("Skill Level")]
    TMP_Text Skill_Level;

    [SerializeField, Tooltip("Tooltip Button")]
    UIInteractiveButton Button;

    UserHeroData User_Data;
    UserHeroSkillData Skill_Data;

    void Start()
    {
        UpdateUI();
    }

    public void Initialize(UserHeroSkillData data)
    {
        Skill_Data = data;
        if (Skill_Type == SKILL_TYPE.NONE)
        {
            Skill_Type = (Skill_Data != null) ? Skill_Data.GetSkillType() : SKILL_TYPE.NONE;
        }
        UpdateSkillCard();
        UpdateUI();
        if (Button != null)
        {
            Button.Tooltip_Data = Skill_Data;
        }
    }

    public void Initialize(int pc_id, int pc_num, SKILL_TYPE type = SKILL_TYPE.NONE)
    {
        var gd = GameData.Instance;
        var skill_list = gd.GetUserHeroSkillDataManager().GetUserHeroSkillDataList(pc_id, pc_num).ToList();

        if (type != SKILL_TYPE.NONE)
        {
            Skill_Type = type;
        }

        UserHeroSkillData data = skill_list.Find(x => x.GetSkillType() == Skill_Type);
        Initialize(data);
    }

    void UpdateUI()
    {
        // text
        if (Skill_Data != null)
        {
            Skill_Type_Text.text = Skill_Data.GetSkillTypeText();
        }
        else
        {
            switch (Skill_Type)
            {
                case SKILL_TYPE.SKILL_01:
                    Skill_Type_Text.text = "액티브 스킬 1";
                    break;
                case SKILL_TYPE.SKILL_02:
                    Skill_Type_Text.text = "액티브 스킬 2";
                    break;
                case SKILL_TYPE.SPECIAL_SKILL:
                    Skill_Type_Text.text = "궁극기";
                    break;
                case SKILL_TYPE.NONE:
                    Skill_Type_Text.text = "EMPTY";
                    break;
            }
        }
    }

    void UpdateSkillCard()
    {
        //  icon
        if (Skill_Data != null)
        {
            CommonUtils.GetResourceFromAddressableAsset<Sprite>(Skill_Data.GetSkillGroupData().icon, (spr) =>
            {
                Skill_Icon.sprite = spr;
            });
        }
        else
        {
            Skill_Icon.sprite = null;
        }

        //  level
        Skill_Level.text = (Skill_Data != null) ? string.Format(GameDefine.GetLocalizeString("system_level_format"), Skill_Data.GetLevel()) : "EMPTY";
    }
}
