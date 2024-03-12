using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : PopupBase
{
    [SerializeField]
    TMP_Text Title;

    [SerializeField]
    RawImage Hero_Stand;

    [SerializeField, Tooltip("InfoBox Tap Controller")]
    Gpm.Ui.TabController InfoBox_Tab_Controller;

    [SerializeField, Tooltip("Level Text")]
    TMP_Text Level_Text;

    [SerializeField, Tooltip("Name Text")]
    TMP_Text Name_Text;

    [SerializeField, Tooltip("Star Images")]
    List<GameObject> Star_Images;

    [SerializeField, Tooltip("Position Tag View")]
    Image Position_Tag;
    [SerializeField, Tooltip("Position Tag Sprites Resources")]
    List<Sprite> Position_Tag_Sprites;

    [SerializeField, Tooltip("Tribe Box View")]
    Image Tribe_Box;
    [SerializeField, Tooltip("Tribe Box Sprites Resources")]
    List<Sprite> Tribe_Box_Sprites;

    [SerializeField, Tooltip("Tribe Tag View")]
    Image Tribe_Tag;
    [SerializeField, Tooltip("Tribe Tag Sprites Resources")]
    List<Sprite> Tribe_Tag_Sprites;

    [SerializeField, Tooltip("Tribe Text")]
    TMP_Text Tribe_Text;

    [SerializeField, Tooltip("Role Icon View")]
    Image Role_Icon;
    [SerializeField, Tooltip("Role Icon Sprites Resources")]
    List<Sprite> Role_Icon_Sprites;

    [SerializeField, Tooltip("Role Text")]
    TMP_Text Role_Text;

    [SerializeField, Tooltip("Hero Basic Info Box")]
    HeroInfoBox Hero_Info_Box;

    List<BattlePcData> User_Hero_Datas;
    int Current_Hero_Data_Index;
    BattlePcData User_Hero_Battle_Data;

    RenderTexture Chara_Texture = null;
    Producer pd = null;
    GameObject Tooltip = null;

    void ResetUI()
    {
        User_Hero_Datas = null;
        Current_Hero_Data_Index = -1;
        User_Hero_Battle_Data = null;
    }

    protected override bool Initialize(object[] data)
    {
        ResetUI();

        if (data.Length < 2 || data[0] is not List<BattlePcData> || data[1] is not int)
        {
            return false;
        }

        int select_tab_index = 0;

        if (data.Length == 3 && data[2] is int)
        {
            select_tab_index = (int)data[2];
        }

        User_Hero_Datas = data[0] as List<BattlePcData>;
        SetData((int)data[1], select_tab_index);

        InitAssets();

        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/UI/SkillTooltip");

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            FixedUpdatePopup();
            return;
        }
    }

    void SetData(int hero_data_index, int select_tab_index = 0)
    {
        Current_Hero_Data_Index = hero_data_index;

        User_Hero_Battle_Data = User_Hero_Datas[Current_Hero_Data_Index];

        ReleaseProducer();

        pd = Factory.Instantiate<Producer>(User_Hero_Battle_Data.Data.lobby_basic_id, LOVE_LEVEL_TYPE.NORMAL, SPINE_CHARA_LOCATION_TYPE.HERO_INFO);

        Hero_Info_Box.SetData(User_Hero_Battle_Data, this, select_tab_index);
    }

    void ReleaseProducer()
    {
        if (pd != null)
        {
            pd.Release();
            pd = null;
        }
    }

    void SetRenderTextureAndCamera()
    {
        Chara_Texture = new RenderTexture(GameDefine.RESOLUTION_SCREEN_WIDTH, (int)(Screen.height * ((float)GameDefine.RESOLUTION_SCREEN_WIDTH / (float)Screen.width)), 16);
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.fieldOfView = Camera.main.fieldOfView;
        over_cam.targetTexture = Chara_Texture;

        //TODO: 일단 임시로 카메라로 위치를 세팅
        over_cam.transform.localPosition = new Vector3(4, 0, 0);

        Hero_Stand.enabled = true;
        Hero_Stand.texture = Chara_Texture;
    }

    protected override void FixedUpdatePopup()
    {
        Title.text = ConstString.HeroInfoUI.TITLE;

        UpdatePopup();
    }

    public override void UpdatePopup()
    {
        var Hero_Base_Data = (Player_Character_Data)User_Hero_Battle_Data.GetUnitData();
        var Unit_Data = (UserHeroData)User_Hero_Battle_Data.GetUserUnitData();

        Level_Text.text = $"LV. {User_Hero_Battle_Data.GetLevel()}";
        Name_Text.text = GameDefine.GetLocalizeString(Hero_Base_Data.name_id);

        int star_grade = Unit_Data.GetStarGrade();
        for (int i = 0; i < Star_Images.Count; ++i)
        {
            Star_Images[i].SetActive(i < star_grade);
        }

        // 포지션 태그 설정
        int index = (int)User_Hero_Battle_Data.GetPositionType() - 1;
        Position_Tag.sprite = Position_Tag_Sprites[index];

        // 종족 태그 설정
        index = (int)User_Hero_Battle_Data.GetTribeType() - 1;
        Tribe_Box.sprite = Tribe_Box_Sprites[index];
        Tribe_Tag.sprite = Tribe_Tag_Sprites[index];
        Tribe_Text.text = ConstString.Hero.TRIBES[(int)Hero_Base_Data.tribe_type];

        // 역할 태그 설정
        index = (int)Hero_Base_Data.role_type - 1;
        Role_Icon.sprite = Role_Icon_Sprites[index];
        Role_Text.text = ConstString.Hero.ROLE[(int)Hero_Base_Data.role_type];

        SetRenderTextureAndCamera();

        Hero_Info_Box.Refresh();
    }

    public void SetActivePd(bool value)
    {
        pd.SetActive(value);
    }

    public void TouchEventCallback(TOUCH_RESULT_TYPE result, System.Func<bool, Rect> hole, object data)
    {
        switch (result)
        {
            case TOUCH_RESULT_TYPE.LONG_PRESS:
                if (Tooltip != null)
                {
                    GameObjectPoolManager.Instance.UnusedGameObject(Tooltip);
                    Tooltip = null;
                }

                UserHeroSkillData skill_data = null;
                if (data == null || data is not UserHeroSkillData)
                {
                    Debug.LogWarning("표시 가능한 스킬 정보가 없습니다!");
                }
                else
                {
                    skill_data = data as UserHeroSkillData;
                }

                Tooltip = GameObjectPoolManager.Instance.GetGameObject("Assets/AssetResources/Prefabs/UI/SkillTooltip", transform.parent);
                var tooltip = Tooltip.GetComponent<SkillTooltip>();
                tooltip.Initialize(hole(false), skill_data, false);
                break;
            case TOUCH_RESULT_TYPE.RELEASE:
                if (Tooltip != null)
                {
                    GameObjectPoolManager.Instance.UnusedGameObject(Tooltip);
                    Tooltip = null;
                }
                break;
        }
    }

    public void OnClickProfileButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/ProfilePopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup((Player_Character_Data)User_Hero_Battle_Data.GetUnitData());
        });
    }

    public void OnClickMemorialButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public override void OnExit()
    {
        ReleaseProducer();
    }

    public void OnClickLeft()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        int index = Current_Hero_Data_Index - 1;
        if (index < 0)
        {
            index = User_Hero_Datas.Count -1;
        }

        SetData(index);
        UpdatePopup();
    }

    public void OnClickRight()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        int index = Current_Hero_Data_Index + 1;
        if (index == User_Hero_Datas.Count)
        {
            index = 0;
        }

        SetData(index);
        UpdatePopup();
    }

    void UpdateEventDispatchCallback(UPDATE_EVENT_TYPE etype)
    {
        if (etype == UPDATE_EVENT_TYPE.UPDATE_HERO_DETAIL_INFO)
        {
            UpdatePopup();
        }
    }
    public override void Spawned()
    {
        base.Spawned();
        UpdateEventDispatcher.Instance.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_HERO_DETAIL_INFO, UpdateEventDispatchCallback);
    }
    public override void Despawned()
    {
        base.Despawned();
        UpdateEventDispatcher.Instance.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_HERO_DETAIL_INFO);

        //TODO: 일단 임시로 카메라로 위치를 세팅
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.transform.localPosition = new Vector3(0, 0, 0);
    }
}
