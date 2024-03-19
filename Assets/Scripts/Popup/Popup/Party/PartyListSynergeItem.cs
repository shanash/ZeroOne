using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyListSynergeItem : MonoBehaviour
{
    [SerializeField, Tooltip("On Object")]
    RectTransform On_Obj;

    [SerializeField, Tooltip("Attribute BG")]
    Image Attribute_BG;

    [SerializeField, Tooltip("Attribute Frame")]
    Image Attribute_Frame;

    [SerializeField, Tooltip("Enable Animator")]
    Animation Enable_Animation;

    

    [SerializeField, Tooltip("Property Text")]
    TMP_Text Property_Text;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;

    UserHeroDeckMountData User_Deck;
    UserHeroData User_Hero;

    BattlePcData Unit_Data;

    public void SetUserHeroDeckMountData(UserHeroDeckMountData ud, GAME_TYPE gtype, CHARACTER_SORT filter_type)
    {
        User_Deck = ud;
        Game_Type = gtype;
        Filter_Type = filter_type;
        if (User_Deck != null)
        {
            User_Hero = ud.GetUserHeroData();
            Unit_Data = new BattlePcData();
            Unit_Data.SetUnitID(User_Hero.GetPlayerCharacterID(), User_Hero.Player_Character_Num);
        }

        UpdateSynergyInfo();
    }

    void UpdateSynergyInfo()
    {
        if (User_Deck == null)
        {
            On_Obj.gameObject.SetActive(false);
            Property_Text.text = string.Empty;
            Attribute_BG.color = CommonUtils.ToRGBFromHex("59A7FF");
            return;
        }
        //  synergy on/off
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);

        bool is_synergy = deck.IsExistTeamSynergyAttribute(User_Hero.GetAttributeType());
        On_Obj.gameObject.SetActive(is_synergy);
        if (On_Obj.gameObject.activeSelf)
        {
            Enable_Animation.Play();
        }

        var attr_data = MasterDataManager.Instance.Get_AttributeIconData(User_Hero.GetAttributeType());
        var color = CommonUtils.ToRGBFromHex(attr_data.color);
        Attribute_BG.color = color;
        Attribute_Frame.color = color;

        //  filter
        Property_Text.text = GetFilterString(Filter_Type);
    }

    string GetFilterString(CHARACTER_SORT ftype)
    {
        string filter = string.Empty;
        switch (ftype)
        {
            case CHARACTER_SORT.NAME:
                filter = Unit_Data.GetUnitName();
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                filter = ZString.Format("Lv.{0}", Unit_Data.GetLevel());
                break;
            case CHARACTER_SORT.STAR:
                filter = User_Hero.GetStarGrade().ToString();
                break;
            case CHARACTER_SORT.DESTINY:
                filter = "0";
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                filter = Unit_Data.GetSumSkillsLevel().ToString();
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                {
                    var special_skill = Unit_Data.Skill_Mng.GetSpecialSkillGroup();
                    if (special_skill != null)
                    {
                        filter = ZString.Format("Lv.{0}", special_skill.GetSkillLevel());
                    }
                    else
                    {
                        filter = "Lv.0";
                    }
                }
                
                break;
            case CHARACTER_SORT.ATTACK:
                {
                    double attk_synergy = GetTeamSynergyPoint(STAT_MULTIPLE_TYPE.ATTACK_RATE);
                    double total_att_point = Unit_Data.GetTotalAttackPoint();
                    if (attk_synergy > 0)
                    {
                        total_att_point += total_att_point * attk_synergy;
                    }
                    filter = total_att_point.ToString("N0");
                }
                break;
            case CHARACTER_SORT.DEFEND:
                {
                    double def_synergy = GetTeamSynergyPoint(STAT_MULTIPLE_TYPE.DEFENSE_RATE);
                    double total_def_point = Unit_Data.GetTotalDefensePoint();
                    if (def_synergy > 0)
                    {
                        total_def_point += total_def_point * def_synergy;
                    }
                    filter = total_def_point.ToString("N0");
                }
                break;
            case CHARACTER_SORT.RANGE:
                filter = Unit_Data.GetApproachDistance().ToString();
                break;
            case CHARACTER_SORT.LIKEABILITY:
                filter = ZString.Format("Lv.{0}", User_Hero.GetLoveLevel());
                break;
            case CHARACTER_SORT.ATTRIBUTE:
                {
                    var attr_data = MasterDataManager.Instance.Get_AttributeIconData(Unit_Data.GetAttributeType());
                    filter = GameDefine.GetLocalizeString(attr_data.name_id);

                }
                break;
            case CHARACTER_SORT.BATTLEPOWER:
                {
                    var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
                    var deck = deck_mng.FindSelectedDeck(Game_Type);
                    var team_synergy = deck.GetTeamSynergyList();
                    double battle_point = Unit_Data.GetCombatPoint(team_synergy);
                    filter = battle_point.ToString("N0");
                }
                
                break;
            default:
                Debug.Assert(false);
                break;
        }
        return filter;
    }

    double GetTeamSynergyPoint(STAT_MULTIPLE_TYPE stat)
    {
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);
        var synergy_list = deck.GetTeamSynergyList();
        double point = synergy_list.Sum(x => x.multiple_type == stat ? x.add_damage_per : 0);
        return point;
    }

}
