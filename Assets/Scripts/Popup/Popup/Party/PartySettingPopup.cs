using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySettingPopup : PopupBase
{
    [Header("Top")]
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Close Btn")]
    UIButtonBase Close_Btn;

    [Header("Middle")]
    [SerializeField, Tooltip("Selected Info")]
    PartySelectInfo Selected_Info_Box;

    [SerializeField, Tooltip("Filter Popup Btn")]
    UIButtonBase Filter_Popup_Btn;
    [SerializeField, Tooltip("Filter Name")]
    TMP_Text Filter_Name;

    [SerializeField, Tooltip("Sort Btn")]
    UIButtonBase Sort_Btn;
    [SerializeField, Tooltip("Arrow Icon")]
    Image Arrow_Icon;

    [SerializeField, Tooltip("Character List View")]
    InfiniteScroll Character_LIst_View;

    [Header("Bottom")]

    [SerializeField, Tooltip("Synergy Show Btn")]
    RectTransform Synergy_Show_Btn;

    [SerializeField, Tooltip("Synergy Icons")]
    List<Image> Synergy_Icon_List;


    [SerializeField, Tooltip("Auto Party Btn")]
    UIButtonBase Auto_Party_Btn;

    [SerializeField, Tooltip("Party List Manager")]
    PartyListManager Party_Mng;

    [SerializeField, Tooltip("Cancel Btn")]
    UIButtonBase Cancel_Btn;
    [SerializeField, Tooltip("Confirm Btn")]
    UIButtonBase Confirm_Btn;
    [SerializeField, Tooltip("Confirm Btn Text")]
    TMP_Text Confirm_Btn_Text;

    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;
    SORT_ORDER Sort_Order = SORT_ORDER.ASC;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    int Dungeon_ID;

    bool Is_Animation_End;
    bool Is_Load_Complete;

    List<BattlePcData> Battle_Hero_Data_List = new List<BattlePcData>();

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }

        Game_Type = (GAME_TYPE)data[0];
        Dungeon_ID = (int)data[1];

        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        Party_Mng.SetGameType(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type, Filter_Type);
        Sort_Order = (SORT_ORDER)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.SORT_ORDER_TYPE, SORT_ORDER.ASC);

        InitAssets();
        UpdateFilterType();
        InitPopupUI();

        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        List<UserHeroData> user_hero_list = new List<UserHeroData>();

        hero_mng.GetUserHeroDataList(ref user_hero_list);

        for (int i = 0; i < user_hero_list.Count; i++)
        {
            var user_hero = user_hero_list[i];
            var found = Battle_Hero_Data_List.Find(x => x.GetUnitID() == user_hero.GetPlayerCharacterID() && x.GetUnitNum() == user_hero.Player_Character_Num);
            if (found == null)
            {
                found = new BattlePcData();
                found.SetUnitID(user_hero.GetPlayerCharacterID(), user_hero.Player_Character_Num);
                Battle_Hero_Data_List.Add(found);
            }

        }

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
            Is_Load_Complete = true;
            if (Is_Load_Complete && Is_Animation_End)
            {
                FixedUpdatePopup();
                return;
            }
        }
    }

    void InitPopupUI()
    {
        //  팝업 타이틀
        Popup_Title.text = GameDefine.GetLocalizeString("system_party_title");

        //  cancel / confirm btn
        if (Game_Type == GAME_TYPE.NONE)
        {
            //  확인
            Confirm_Btn_Text.text = GameDefine.GetLocalizeString("system_answer_ok");
            Cancel_Btn.gameObject.SetActive(false);
        }
        else
        {
            //  진입
            Confirm_Btn_Text.text = GameDefine.GetLocalizeString("system_entrance");
            Cancel_Btn.gameObject.SetActive(true);
        }
    }

    protected override void ShowPopupAniEndCallback()
    {
        Is_Animation_End = true;
        if (Is_Animation_End && Is_Load_Complete)
        {
            FixedUpdatePopup();
            UpdatePopup();
        }
    }


    protected override void FixedUpdatePopup()
    {
        Character_LIst_View.Clear();

        const int column_count = 6;
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();

        //  sort
        if (Sort_Order == SORT_ORDER.ASC)
        {
            switch (Filter_Type)
            {
                case CHARACTER_SORT.NAME:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetUnitName().CompareTo(b.GetUnitName()));
                    break;
                case CHARACTER_SORT.LEVEL_CHARACTER:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetLevel().CompareTo(b.GetLevel()));
                    break;
                case CHARACTER_SORT.STAR:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetStarGrade().CompareTo(b.GetStarGrade()));
                    break;
                case CHARACTER_SORT.DESTINY:
                    Debug.Assert(false);
                    break;
                case CHARACTER_SORT.EX_SKILL_LEVEL:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetSpecialSkillLevel().CompareTo(b.GetSpecialSkillLevel()));
                    break;
                case CHARACTER_SORT.ATTACK:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetTotalAttackPoint().CompareTo(b.GetTotalAttackPoint()));
                    break;
                case CHARACTER_SORT.DEFEND:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetTotalDefensePoint().CompareTo(b.GetTotalDefensePoint()));
                    break;
                case CHARACTER_SORT.RANGE:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetApproachDistance().CompareTo(b.GetApproachDistance()));
                    break;
                case CHARACTER_SORT.LIKEABILITY:
                    //  not m2
                    break;
                case CHARACTER_SORT.ATTRIBUTE:
                    //  전기/베리타리움/요력/마력(오름 차순)
                    Battle_Hero_Data_List.Sort((a, b) => a.GetAttributeType().CompareTo(b.GetAttributeType()));
                    break;
                case CHARACTER_SORT.BATTLEPOWER:
                    Battle_Hero_Data_List.Sort((a, b) => a.GetCombatPoint().CompareTo(b.GetCombatPoint()));
                    break;
            }
        }
        else
        {
            switch (Filter_Type)
            {
                case CHARACTER_SORT.NAME:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetUnitName().CompareTo(a.GetUnitName()));
                    break;
                case CHARACTER_SORT.LEVEL_CHARACTER:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetLevel().CompareTo(a.GetLevel()));
                    break;
                case CHARACTER_SORT.STAR:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetStarGrade().CompareTo(a.GetStarGrade()));
                    break;
                case CHARACTER_SORT.DESTINY:
                    Debug.Assert(false);
                    break;
                case CHARACTER_SORT.EX_SKILL_LEVEL:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetSpecialSkillLevel().CompareTo(a.GetSpecialSkillLevel()));
                    break;
                case CHARACTER_SORT.ATTACK:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetTotalAttackPoint().CompareTo(a.GetTotalAttackPoint()));
                    break;
                case CHARACTER_SORT.DEFEND:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetTotalDefensePoint().CompareTo(a.GetTotalDefensePoint()));
                    break;
                case CHARACTER_SORT.RANGE:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetApproachDistance().CompareTo(a.GetApproachDistance()));
                    break;
                case CHARACTER_SORT.LIKEABILITY:
                    //  not m2
                    break;
                case CHARACTER_SORT.ATTRIBUTE:
                    //  전기/베리타리움/요력/마력(오름 차순)
                    Battle_Hero_Data_List.Sort((a, b) => b.GetAttributeType().CompareTo(a.GetAttributeType()));
                    break;
                case CHARACTER_SORT.BATTLEPOWER:
                    Battle_Hero_Data_List.Sort((a, b) => b.GetCombatPoint().CompareTo(a.GetCombatPoint()));
                    break;

            }
        }

        //  가로 컬럼 6개(보유중인 영웅 리스트 불러오기)
        int start = 0;
        int hero_count = Battle_Hero_Data_List.Count;
        int rows = hero_count / column_count;
        if (hero_count % column_count > 0)
        {
            rows += 1;
        }
        for (int r = 0; r < rows; r++)
        {
            start = r * column_count;

            var new_data = new PartyCharacterListData();
            new_data.Click_Hero_Callback = SelectCharacterCallback;
            new_data.SetGameType(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type);
            new_data.SetFilterType(Filter_Type);
            if (start + column_count < hero_count)
            {
                new_data.SetBattleHeroDataList(Battle_Hero_Data_List.GetRange(start, column_count));
            }
            else
            {
                new_data.SetBattleHeroDataList(Battle_Hero_Data_List.GetRange(start, hero_count - start));
            }
            Character_LIst_View.InsertData(new_data);
        }

        //  파티 슬롯에 클릭 콜백 등록
        Party_Mng.SetSlotCardChoiceCallback(PartySlotCardChoiceCallback);

        UpdateFilterType();
    }

    void UpdateFilterType()
    {
        Filter_Name.text = GetFilterString(Filter_Type);

        var arrow_scale = Arrow_Icon.transform.localScale;
        arrow_scale.y = Sort_Order == SORT_ORDER.ASC ? -1 : 1;
        Arrow_Icon.transform.localScale = arrow_scale;
    }

    string GetFilterString(CHARACTER_SORT ftype)
    {
        string filter = string.Empty;
        switch (ftype)
        {
            case CHARACTER_SORT.NAME:
                filter = GameDefine.GetLocalizeString("system_sorting_name_01");
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                filter = GameDefine.GetLocalizeString("system_sorting_name_02");
                break;
            case CHARACTER_SORT.STAR:
                filter = GameDefine.GetLocalizeString("system_sorting_name_03");
                break;
            case CHARACTER_SORT.DESTINY:
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                filter = GameDefine.GetLocalizeString("system_sorting_name_05");
                break;
            case CHARACTER_SORT.ATTACK:
                filter = GameDefine.GetLocalizeString("system_sorting_name_06");
                break;
            case CHARACTER_SORT.DEFEND:
                filter = GameDefine.GetLocalizeString("system_sorting_name_07");
                break;
            case CHARACTER_SORT.RANGE:
                filter = GameDefine.GetLocalizeString("system_sorting_name_08");
                break;
            case CHARACTER_SORT.LIKEABILITY:
                break;
            case CHARACTER_SORT.ATTRIBUTE:
                filter = GameDefine.GetLocalizeString("system_sorting_name_09");
                break;
            case CHARACTER_SORT.BATTLEPOWER:
                filter = GameDefine.GetLocalizeString("system_stat_battlepower");
                break;
            default:
                Debug.Assert(false);
                break;
        }

        return filter;
    }

    public override void UpdatePopup()
    {
        Party_Mng.UpdateUserPartySettings();
    }

    /// <summary>
    /// 영웅 리스트에서 영웅 선택시 콜백
    /// </summary>
    /// <param name="ud"></param>
    void SelectCharacterCallback(TOUCH_RESULT_TYPE type, Func<bool, Rect> hole, object hero_obj)
    {
        if (type != TOUCH_RESULT_TYPE.CLICK || hero_obj == null || hero_obj is not PartyCharacterListItem)
        {
            return;
        }

        var hero = hero_obj as PartyCharacterListItem;

        var unit_data = hero.GetBattleHeroData();
        if (unit_data == null)
        {
            return;
        }

        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var user_deck = user_deck_mng.FindSelectedDeck(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type);

        //  선택 캐릭터 정보
        Selected_Info_Box.SetPlayerCharacterID(unit_data.GetUnitID(), unit_data.GetUnitNum());

        //  이미 덱에 포함되어 있는 영웅이라면 덱에서 제거
        if (user_deck.IsExistHeroInDeck(unit_data.GetUnitID(), unit_data.GetUnitNum()))
        {
            user_deck.RemoveHero(unit_data.GetUnitID(), unit_data.GetUnitNum());
            Party_Mng.UpdateUserPartySettings();
            hero.UpdateCellItem_V2();
        }
        else // 처음 선택되는 영웅이라면 덱에 빈 슬롯이 있는지 찾아보고, 빈 슬롯이 있으면 추가
        {
            RESPONSE_TYPE code = user_deck.AddSlotHero(unit_data.GetUnitID(), unit_data.GetUnitNum());
            if (code != RESPONSE_TYPE.SUCCESS)
            {
                if (code == RESPONSE_TYPE.NOT_EXIST_EMPTY_SLOT)
                {
                    PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
                    {
                        popup.ShowPopup(1.5f, "슬롯에 빈 자리가 없습니다.");
                    });
                }
                return;
            }
            Party_Mng.UpdateUserPartySettings();
            hero.UpdateCellItem_V2();
        }
        user_deck_mng.Save();
    }

    void PartySlotCardChoiceCallback(TOUCH_RESULT_TYPE type, Func<bool, Rect> hole, object slot_obj)
    {
        if (type != TOUCH_RESULT_TYPE.CLICK || slot_obj == null || slot_obj is not PartySlotNode)
        {
            return;
        }

        var slot = slot_obj as PartySlotNode;
        if (slot.GetUserHeroDeckMountData() == null)
        {
            return;
        }
        var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var user_deck = user_deck_mng.FindSelectedDeck(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type);
        user_deck.RemoveHero(slot.GetUserHeroDeckMountData());

        Party_Mng.UpdateUserPartySettings();

        //  영웅 리스트 업데이트
        Character_LIst_View.UpdateAllData();
        user_deck_mng.Save();
    }

    #region OnClick Funcs
    public void OnClickClosePartySettings()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickFilterPopup()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/FilterPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.SetHideCompleteCallback(() =>
            {
                Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
                Party_Mng.SetGameType(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type, Filter_Type);
                Party_Mng.UpdateUserPartySettings();
                FixedUpdatePopup();
            });
            popup.ShowPopup();
        });
    }
    public void OnClickSort()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        if (Sort_Order == SORT_ORDER.ASC)
        {
            Sort_Order = SORT_ORDER.DESC;
        }
        else
        {
            Sort_Order = SORT_ORDER.ASC;
        }
        GameConfig.Instance.SetGameConfig<int>(GAME_CONFIG_KEY.SORT_ORDER_TYPE, (int)Sort_Order);
        FixedUpdatePopup();
    }

    public void OnClickTeamSynergy()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var user_deck = user_deck_mng.FindSelectedDeck(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type);
        if (user_deck != null)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/TeamSynergyEffectTooltipPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type, user_deck.Deck_Number, (Vector2)Synergy_Show_Btn.position);
            });
        }
    }
    public void OnClickTeamSynergyHelp()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/AttributePowerCompatibilityPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup();
        });
    }
    public void OnClickAutoParty()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }
    public void OnClickConfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        if (Game_Type == GAME_TYPE.NONE)
        {
            HidePopup();
        }
        else
        {
            var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
            var user_deck = user_deck_mng.FindSelectedDeck(Game_Type);
            //  덱에 영웅들이 세팅되어 있는지 여부 체크
            if (!user_deck.IsExistHeroInDeck())
            {
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
                {
                    popup.ShowPopup(2f, "파티 영웅을 지정해 주세요.");
                });
                return;
            }

            var board = BlackBoard.Instance;
            board.SetBlackBoard(BLACK_BOARD_KEY.DUNGEON_ID, Dungeon_ID);
            board.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, Game_Type);
           
            SCManager.I.ChangeScene(SceneName.battle);
        }
    }

    public void OnShowSkillTooltip(TOUCH_RESULT_TYPE type, Func<bool,Rect> hole, object data)
    {
        switch (type)
        {
            case TOUCH_RESULT_TYPE.LONG_PRESS:
                UserHeroSkillData skill_data = null;

                if (data == null || data is not UserHeroSkillData)
                {
                    Debug.LogWarning("표시 가능한 스킬 정보가 없습니다!");
                }
                else
                {
                    skill_data = data as UserHeroSkillData;
                }

                TooltipManager.I.Add("Assets/AssetResources/Prefabs/UI/SkillTooltip", hole(false), skill_data);
                break;
        }
    }

    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
    #endregion

    public override void Spawned()
    {
        base.Spawned();
        Selected_Info_Box.ShowDefaultInfoBox(true);
    }
    public override void Despawned()
    {
        base.Despawned();
    }
}
