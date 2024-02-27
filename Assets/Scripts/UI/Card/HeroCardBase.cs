using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroCardBase : MonoBehaviour, IPoolableComponent
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

    [SerializeField, Tooltip("Touch Btn")]
    protected UILongTouchButtonBase Long_Touch_Btn;

    protected Player_Character_Data Data;
    protected Player_Character_Battle_Data Battle_Data;

    public delegate void TouchDownAction(PointerEventData evt);
    public delegate void TouchUpAction(PointerEventData evt);
    public delegate void ClickAction(PointerEventData evt);

    public event TouchDownAction TouchDown;
    public event TouchUpAction TouchUp;
    public event ClickAction Click;

    public void SetHeroData(UserHeroData data, CHARACTER_SORT filter_type)
    {
        Data = data.GetPlayerCharacterData();
        Battle_Data = data.GetPlayerCharacterBattleData();

        SetLevel(data.GetLevel());
        // star
        SetStarGrade(data.GetStarGrade());
        // role type
        SetRoleType(data.GetPlayerCharacterData().role_type);
        // 
        SetName(GameDefine.GetLocalizeString(data.GetPlayerCharacterData().name_id));

        SetFilter(data, filter_type);

        UpdateCardBase();
    }

    public void SetHeroDataID(int hero_data_id)
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_PlayerCharacterData(hero_data_id);
        Battle_Data = m.Get_PlayerCharacterBattleData(Data.battle_info_id, Data.default_star);

        UpdateCardBase();
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

    public void SetName(string name)
    {
        Name_Text.text = name;
    }

    public void SetFilter(UserHeroData data, CHARACTER_SORT filter_type)
    {
        // Filter
        Filter_Box.gameObject.SetActive(filter_type != CHARACTER_SORT.NAME);
        string filter_text = "미구현";
        switch (filter_type)
        {
            case CHARACTER_SORT.LEVEL_CHARACTER:
                filter_text = $"Lv.{data.GetLevel()}";
                break;
            case CHARACTER_SORT.STAR:
                filter_text = new string('★', data.GetStarGrade());
                break;
            case CHARACTER_SORT.NAME:
                Filter_Text.text = GameDefine.GetLocalizeString(Data.name_id);
                break;
            default:
                Filter_Text.text = "미구현";
                break;
        }

        Filter_Text.text = filter_text;
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
        TouchDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        TouchUp?.Invoke(eventData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click?.Invoke(eventData);
    }

    public void AddTouchEventCallback(UnityAction<TOUCH_RESULT_TYPE> cb)
    {
        Long_Touch_Btn.AddTouchCallback(cb);
    }
    public void RemoveTouchEventCallback(UnityAction<TOUCH_RESULT_TYPE> cb)
    {
        Long_Touch_Btn.RemovTouchCallback(cb);
    }
}
