using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroCardBase : MonoBehaviour, IPoolableComponent, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("Box")]
    protected RectTransform Box;

    [Header("Card")]
    [SerializeField, Tooltip("Card BG")]
    protected Image Card_BG;

    [SerializeField, Tooltip("Hero Icon Image")]
    protected Image Hero_Icon_Image;

    [SerializeField, Tooltip("Card Frame")]
    protected Image Card_Frame;

    [Header("Filter")]
    [SerializeField, Tooltip("Filter Box")]
    protected RectTransform Filter_Box;

    [SerializeField, Tooltip("Filter Text")]
    protected TMP_Text Filter_Text;

    [Header("Spec")]
    [SerializeField, Tooltip("Spec Info Box")]
    protected RectTransform Spec_Info_Box;

    [SerializeField, Tooltip("Level Box")]
    protected RectTransform Level_Box;
    [SerializeField, Tooltip("Level Text")]
    protected TMP_Text Level_Text;

    [SerializeField, Tooltip("Star Box")]
    protected RectTransform Star_Box;
    [SerializeField, Tooltip("Star Text")]
    protected TMP_Text Star_Text;

    [SerializeField, Tooltip("Role")]
    protected RectTransform Role_Box;

    [SerializeField, Tooltip("Role Icons")]
    protected List<Image> Role_Icons;

    [Header("Party Select")]

    [SerializeField, Tooltip("Party Select Frame")]
    protected RectTransform Party_Select_Frame;

    [Header("Name")]
    [SerializeField, Tooltip("Name")]
    protected TMP_Text Name_Text;

    [SerializeField, Tooltip("Use Long Touch")]
    protected bool Use_Long_Touch;

    protected Player_Character_Data Data;
    protected Player_Character_Battle_Data Battle_Data;

    public delegate void TouchDownAction(TOUCH_STATES_TYPE etype);
    public delegate void TouchUpAction(TOUCH_STATES_TYPE etype);
    public delegate void ClickAction(TOUCH_STATES_TYPE etype);
    public delegate void TouchEventAction(TOUCH_STATES_TYPE etype);

    public event TouchDownAction TouchDown;
    public event TouchUpAction TouchUp;
    public event ClickAction Click;

    public event TouchEventAction Touch_Event;


    /// <summary>
    /// 롱터치 관련 변수
    /// </summary>
    TOUCH_STATES_TYPE Touch_State = TOUCH_STATES_TYPE.NONE;
    Coroutine Long_Touch_Coroutine;
    bool Is_Cancel;
    bool Is_Long_Press;
    float Touch_Delta;


    public void SetHeroDataID(int hero_data_id)
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_PlayerCharacterData(hero_data_id);
        Battle_Data = m.Get_PlayerCharacterBattleData(Data.battle_info_id);
        
        UpdateCardBase();
    }

    public void SetUseLongTouch(bool use)
    {
        Use_Long_Touch = use;
    }

    void UpdateCardBase()
    {
        if (Data == null)
        {
            return;
        }

        //  icon
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) => 
        { 
            Hero_Icon_Image.sprite = spr;
        });
    }

    /// <summary>
    /// 캐릭터 스펙 정보 프레임 On/Off
    /// </summary>
    /// <param name="show"></param>
    public void ShowCharacterSpec(bool show)
    {
        Spec_Info_Box.gameObject.SetActive(show);
    }

    public void SetLevel(int lv)
    {
        Level_Text.text = lv.ToString();
    }
    public void SetStarGrade(int star_grade)
    {
        Star_Text.text = star_grade.ToString();
    }
    public void SetRoleType(ROLE_TYPE rtype)
    {
        int idx = (int)rtype - 1;
        if (idx < 0)
        {
            idx = 0;
        }
        for (int i = 0; i < Role_Icons.Count; i++)
        {
            Role_Icons[i].gameObject.SetActive(i == idx);
        }
    }

    /// <summary>
    /// 파티 덱 선택 프레임 On/Off
    /// </summary>
    /// <param name="select"></param>
    public void SetPartySelect(bool select)
    {
        Party_Select_Frame.gameObject.SetActive(select);
    }


    public virtual void Despawned()
    {

    }

    public virtual void Spawned()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Use_Long_Touch)
        {
            Is_Cancel = false;
            Touch_Delta = 0f;
            Touch_State = TOUCH_STATES_TYPE.TOUCH_BEGIN;
            TouchDown?.Invoke(Touch_State);
        }
        else
        {
            TouchDown?.Invoke(Touch_State);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Use_Long_Touch)
        {
            Is_Long_Press = false;
            if (Touch_State == TOUCH_STATES_TYPE.TOUCH_BEGIN)
            {

            }
            else
            {

            }

            TouchUp?.Invoke(Touch_State);
        }
        else
        {
            TouchUp?.Invoke(Touch_State);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click?.Invoke(Touch_State);
    }
}
