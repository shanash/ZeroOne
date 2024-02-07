using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PCDeckSettingUI : PopupBase
{
    [SerializeField, Tooltip("Hero List View")]
    ScrollRect Scroll;

    [SerializeField, Tooltip("Content View")]
    RectTransform Content_View;

    [SerializeField, Tooltip("Deck Manager")]
    DeckBoxManager Deck_Mng;

    [SerializeField, Tooltip("Battle Start Btn")]
    Button Battle_Start_Btn;

    [SerializeField, Tooltip("Battle Start Btn Text")]
    TMP_Text Battle_Start_Btn_Text;

    List<DeckHeroListCell> Used_Deck_Hero_List_Cells = new List<DeckHeroListCell>();

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    int Dungeon_ID;

    protected override bool Initialize(params object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }

        Game_Type = (GAME_TYPE)data[0];
        Dungeon_ID = (int)data[1];

        Deck_Mng.SetGameType(Game_Type);

        //  애니메이션을 사용하지 않을 경우 여기서 프리팹 체크
        if (!IsUseAnimation())
        {
            CheckPopupUsePrefabs();
        }

        return true;
    }

    /// <summary>
    /// 애니메이션 사용할 경우 여기서 프리팹 체크
    /// </summary>
    protected override void ShowPopupAniEndCallback()
    {
        CheckPopupUsePrefabs();
    }

    /// <summary>
    /// 해당 팝업에서 사용할 프리팹이 이미 로드되어 있는지 여부 체크<br/>
    /// 각 팝업마다 필요로하는 프리팹의 요소가 다르기 때문에<br/>
    /// 각 팝업마다 구현이 필요
    /// </summary>
    protected override void CheckPopupUsePrefabs()
    {
        List<string> preload_prefabs = new List<string>();

        preload_prefabs.Add("Assets/AssetResources/Prefabs/Popup/UI/Deck/DeckHeroListCell");

        CheckPreloadPrefabs(preload_prefabs, PreloadCompleteCallback);
    }


    protected void PreloadCompleteCallback()
    {
        FixedUpdatePopup();
        UpdatePopup();
    }
    /// <summary>
    /// 팝업 등장 후 최초 1회만 실행하기 위한 함수.
    /// 경우에 따라 필요한 조건에 만족하면 한번 더 호출하여 사용할 수도 있겠지만
    /// 가급적 최초 1회만 호출하는 조건을 위반하지는 말자.
    /// </summary>
    protected override void FixedUpdatePopup()
    {
        ClearListNodes();
        var pool = GameObjectPoolManager.Instance;
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        List<UserHeroData> user_hero_list = new List<UserHeroData>();
        hero_mng.GetUserHeroDataList(ref user_hero_list);

        //  가로 컬럼 8개 (보유중인 영웅 리스트 불러오기)
        int columns = 8;
        int start = 0;
        int hero_count = user_hero_list.Count;
        int rows = hero_count / columns;
        if (hero_count % columns > 0)
        {
            rows += 1;
        }
        for (int r = 0; r < rows; r++)
        {
            start = r * columns;
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/UI/Deck/DeckHeroListCell", Content_View);
            DeckHeroListCell cell = obj.GetComponent<DeckHeroListCell>();
            if (start + columns < hero_count)
            {
                cell.SetUserHeroDataList(user_hero_list.GetRange(start, columns));
            }
            else
            {
                cell.SetUserHeroDataList(user_hero_list.GetRange(start, hero_count - start));
            }
            cell.SetDeckHeroListCardChoiceCallback(HeroCardChoiceCallback);
            Used_Deck_Hero_List_Cells.Add(cell);
        }

        //  덱 슬롯에 클릭 콜백 등록
        Deck_Mng.SetSlotCardChoiceCallback(DeckSlotCardChoiceCallback);
    }



    public override void UpdatePopup()
    {
        Deck_Mng.UpdateUserDeckUpdate();
    }
    /// <summary>
    /// 덱 슬롯 선택 콜백
    /// </summary>
    /// <param name="slot"></param>
    void DeckSlotCardChoiceCallback(DeckSlotHeroCardNode slot)
    {
        if (slot.GetUserHeroDeckMountData() == null)
        {
            return;
        }
        var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var user_deck = user_deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        user_deck.RemoveHero(slot.GetUserHeroDeckMountData());

        Deck_Mng.UpdateUserDeckUpdate();

        //  영웅 리스트 업데이트
        int cnt = Used_Deck_Hero_List_Cells.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Deck_Hero_List_Cells[i].UpdateHeroListCardNodes();
        }
    }
    /// <summary>
    /// 영웅 리스트 선택시 콜백
    /// </summary>
    /// <param name="hero"></param>
    void HeroCardChoiceCallback(DeckHeroListCardNode hero)
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
        var user_deck = user_deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        var user_hero_data = hero.GetUserHeroData();

        //  이미 덱에 포함되어 있는 있는 영웅이라면 덱에서 제거
        if (user_deck.IsExistHeroInDeck(user_hero_data))
        {
            user_deck.RemoveHero(user_hero_data);
            Deck_Mng.UpdateUserDeckUpdate();
            hero.UpdateCardNode();
        }
        else // 처음 선택되는 영웅이라면 덱에 빈 슬롯이 있는지 찾아보고, 빈 슬롯이 있으면 추가
        {
            ERROR_CODE code = user_deck.AddSlotHero(user_hero_data.GetPlayerCharacterID(), user_hero_data.Player_Character_Num);
            if (code != ERROR_CODE.SUCCESS)
            {
                if (code == ERROR_CODE.NOT_EXIST_EMPTY_SLOT)
                {
                    Debug.Log("슬롯에 빈 자리가 없습니다.");
                }
                return;
            }

            Deck_Mng.UpdateUserDeckUpdate();
            hero.UpdateCardNode();

        }
        user_deck_mng.Save();
    }

    /// <summary>
    /// 모든 리스트 초기화
    /// </summary>
    void ClearListNodes()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Deck_Hero_List_Cells.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Deck_Hero_List_Cells[i].gameObject);
        }
        Used_Deck_Hero_List_Cells.Clear();
    }
    public void OnClickBattleStart()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        var user_deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var user_deck = user_deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);

        //  덱에 영웅들이 세팅되어 있는지 여부 체크
        if (!user_deck.IsExistHeroInDeck())
        {
            Debug.Log("덱에 영웅을 설정해 주세요.");
            return;
        }
        var board = BlackBoard.Instance;

        if (Game_Type == GAME_TYPE.STORY_MODE)
        {
            board.SetBlackBoard(BLACK_BOARD_KEY.DUNGEON_ID, Dungeon_ID);
        }
        board.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, Game_Type);

        SceneManager.LoadScene("battle");
    }

    public override void Despawned()
    {
        base.Despawned();
        Game_Type = GAME_TYPE.NONE;
        Dungeon_ID = 0;
        Deck_Mng.SetGameType(Game_Type);
    }
}
