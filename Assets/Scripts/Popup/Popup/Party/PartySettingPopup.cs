using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    int Dungeon_ID;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }

        Game_Type = (GAME_TYPE)data[0];
        Dungeon_ID = (int)data[1];

        Party_Mng.SetGameType(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type);
        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        UpdateFilterType();
        InitPopupUI();

        return true;
    }

    void InitPopupUI()
    {
        //  cancel / confirm btn
        if (Game_Type == GAME_TYPE.NONE)
        {
            Confirm_Btn_Text.text = "확인";
            Cancel_Btn.gameObject.SetActive(false);
        }
        else
        {
            Confirm_Btn_Text.text = "진입";
            Cancel_Btn.gameObject.SetActive(true);
        }
    }

    protected override void ShowPopupAniEndCallback()
    {
        FixedUpdatePopup();
        UpdatePopup();
    }

    protected override void HidePopupAniEndCallback()
    {
        base.HidePopupAniEndCallback();
    }


    protected override void FixedUpdatePopup()
    {
        Character_LIst_View.Clear();

        const int column_count = 5;
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        List<UserHeroData> user_hero_list = new List<UserHeroData>();
        hero_mng.GetUserHeroDataList(ref user_hero_list);

        //  가로 컬럼 5개(보유중인 영웅 리스트 불러오기)
        int start = 0;
        int hero_count = user_hero_list.Count;
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

            if (start + column_count < hero_count)
            {
                new_data.SetUserHeroDataList(user_hero_list.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserHeroDataList(user_hero_list.GetRange(start, hero_count - start));
            }
            Character_LIst_View.InsertData(new_data);
        }

        //  파티 슬롯에 클릭 콜백 등록
        Party_Mng.SetSlotCardChoiceCallback(PartySlotCardChoiceCallback);

        UpdateFilterType();
    }

    void UpdateFilterType()
    {
        Filter_Name.text = ConstString.Hero.SORT_FILLTER[(int)Filter_Type];
    }

    public override void UpdatePopup()
    {
        Party_Mng.UpdateUserPartySettings();
    }

    /// <summary>
    /// 영웅 리스트에서 영웅 선택시 콜백
    /// </summary>
    /// <param name="ud"></param>
    void SelectCharacterCallback(PartyCharacterListItem hero)
    {
        if (hero == null)
        {
            return;
        }
        if (hero.GetUserHeroData() == null)
        {
            return;
        }

        var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var user_deck = user_deck_mng.FindSelectedDeck(Game_Type == GAME_TYPE.NONE ? GAME_TYPE.STORY_MODE : Game_Type);
        var user_hero_data = hero.GetUserHeroData();

        //  선택 캐릭터 정보
        Selected_Info_Box.SetPlayerCharacterID(user_hero_data.GetPlayerCharacterID(), user_hero_data.Player_Character_Num);

        //  이미 덱에 포함되어 있는 영웅이라면 덱에서 제거
        if (user_deck.IsExistHeroInDeck(user_hero_data))
        {
            user_deck.RemoveHero(user_hero_data);
            Party_Mng.UpdateUserPartySettings();
            hero.UpdateCellItem();
        }
        else // 처음 선택되는 영웅이라면 덱에 빈 슬롯이 있는지 찾아보고, 빈 슬롯이 있으면 추가
        {
            ERROR_CODE code = user_deck.AddSlotHero(user_hero_data.GetPlayerCharacterID(), user_hero_data.Player_Character_Num);
            if (code != ERROR_CODE.SUCCESS)
            {
                if (code == ERROR_CODE.NOT_EXIST_EMPTY_SLOT)
                {
                    PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
                    {
                        popup.ShowPopup(1.5f, "슬롯에 빈 자리가 없습니다.");
                    });
                }
                return;
            }
            Party_Mng.UpdateUserPartySettings();
            hero.UpdateCellItem();
        }
        user_deck_mng.Save();


    }

    void PartySlotCardChoiceCallback(PartySlotNode slot)
    {
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
                Filter_Type = GameConfig.Instance.GetGameConfigValue<CHARACTER_SORT>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
                FixedUpdatePopup();
            });
            popup.ShowPopup();
        });
    }
    public void OnClickSort()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
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


    #endregion


    public override void Spawned()
    {
        base.Spawned();
        Selected_Info_Box.ShowInfoBox(false);
    }
    public override void Despawned()
    {
        base.Despawned();
    }
}
