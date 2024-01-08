using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SKILL_SLOT_EVENT_TYPE
{
    NONE = 0,

    HITTED,                     //  피격
    LIFE_UPDATE,                //  체력 게이지 업데이트
    DURATION_SKILL_ICON_UPDATE, //  지속성 효과 아이콘 업데이트

}

public class BattleSkillSlot : UIBase, IUpdateComponent
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box_Rect;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Buff Icon Container")]
    RectTransform Buff_Icon_Container;

    [SerializeField, Tooltip("Skill Ready")]
    RectTransform Skill_Ready;

    [SerializeField, Tooltip("Life Bar")]
    Image Life_Bar_Gauge;

    [SerializeField, Tooltip("Cooltime Gauge")]
    Image Cooltime_Gauge;

    [SerializeField, Tooltip("Cooltime Text")]
    TMP_Text Cooltime_Text;

    [SerializeField, Tooltip("Death")]
    RectTransform Death_Box;

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);

    UserHeroData User_Data;
    HeroBase_V2 Hero;


    public void SetHeroBase(HeroBase_V2 hero)
    {
        Hero = hero;
        Hero.Slot_Events += SkillSlotEventCallback;

        var battle_unit = Hero.GetBattleUnitData();
        User_Data = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(battle_unit.GetUnitID(), battle_unit.GetUnitNum());
        
        UpdateSkillSlot();
    }


    /// <summary>
    /// 캐릭터 ID/Num 비교
    /// </summary>
    /// <param name="pc_id"></param>
    /// <param name="pc_num"></param>
    /// <returns></returns>
    public bool IsEqualPlayerCharacter(int pc_id, int pc_num)
    {
        if (User_Data != null)
        {
            return User_Data.GetPlayerCharacterID() == pc_id && User_Data.Player_Character_Num == pc_num;
        }
        return false;
    }

    void UpdateSkillSlot()
    {
        Card.SetHeroDataID(User_Data.GetPlayerCharacterID());

        UpdateLifeBar();
    }

    void TouchEventCallback(TOUCH_RESULT_TYPE result)
    {
        if (result == TOUCH_RESULT_TYPE.CLICK)
        {
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            Hero?.SpecialSkillExec();

        }
        else if (result == TOUCH_RESULT_TYPE.LONG_PRESS)
        {
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            //  show skill tooltip
            Debug.Log("Skill Slot Long Touch");
        }
    }
    

    void SkillSlotEventCallback(SKILL_SLOT_EVENT_TYPE etype)
    {
        switch (etype)
        {
            case SKILL_SLOT_EVENT_TYPE.HITTED:
                UpdateHitted();
                break;
            case SKILL_SLOT_EVENT_TYPE.LIFE_UPDATE:
                UpdateLifeBar();
                break;
            case SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE:
                UpdateDurationSkillIcons();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    #region Slot Event Funcs

    void UpdateHitted()
    {

    }
    void UpdateLifeBar()
    {
        if (Hero == null)
        {
            return;
        }
        float per = (float)(Hero.Life / Hero.Max_Life);
        Life_Bar_Gauge.fillAmount = Mathf.Clamp01(per);
    }
    void UpdateDurationSkillIcons()
    {

    }


    #endregion

    private void OnEnable()
    {
        if (Card != null)
        {
            Card.AddTouchEventCallback(TouchEventCallback);
        }
    }

    private void OnDisable()
    {
        if (Card != null)
        {
            Card.RemoveTouchEventCallback(TouchEventCallback);
        }
        if (Hero != null)
        {
            Hero.Slot_Events -= SkillSlotEventCallback;
        }
        Hero = null;
    }

    public override void Spawned()
    {
        base.Spawned();
        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }

    public override void Despawned()
    {
        base.Despawned();
        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
        Hero = null;
    }

    public void OnUpdate(float dt)
    {
        if (Hero == null)
        {
            return;
        }
        
    }
}
