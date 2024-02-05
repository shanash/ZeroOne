using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Text;
using System.Collections;

public class GameResultLosePopup : PopupBase
{
    [Header("Result Popup Vars")]
    [Space()]
    [SerializeField, Tooltip("Lose Text Ease Scale")]
    UIEaseScale Lose_Text_Ease;

    [Space()]
    [Header("Player Info")]
    [SerializeField, Tooltip("Player Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Player_Info_Ease_Alpha;
    [SerializeField, Tooltip("Player Info Ease Slide")]
    UIEaseSlide Player_Info_Ease_Slide;

    [SerializeField, Tooltip("Player Lv Title")]
    TMP_Text Player_Lv_Title;
    [SerializeField, Tooltip("Player Lv Text")]
    TMP_Text Player_Lv;

    [SerializeField, Tooltip("Player Exp Gauge")]
    Slider Player_Exp_Gauge;
    [SerializeField, Tooltip("Player Exp Title")]
    TMP_Text Player_Exp_Title;
    [SerializeField, Tooltip("Player Exp")]
    TMP_Text Player_Exp;

    [SerializeField, Tooltip("Stage Reward Icon")]
    Image Stage_Reward_Icon;
    [SerializeField, Tooltip("Stage Reward Gold")]
    TMP_Text Stage_Reward_Gold_Count;


    [Space()]
    [Header("Character Info")]
    [SerializeField, Tooltip("Character Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Character_Info_Ease_Alpha;
    [SerializeField, Tooltip("Character Info Ease Slide")]
    UIEaseSlide Character_Info_Ease_Slide;

    [SerializeField, Tooltip("Character Info Container")]
    RectTransform Character_Info_Container;


    [Space()]
    [Header("Home Btn")]
    [SerializeField, Tooltip("Home Btn Ease Alpha")]
    UIEaseCanvasGroupAlpha Home_Btn_Ease_Alpha;
    [SerializeField, Tooltip("Home Btn Ease Slide")]
    UIEaseSlide Home_Btn_Ease_Slide;

    List<GameResultPlayerCharacterInfo> Used_Player_Character_Info_List = new List<GameResultPlayerCharacterInfo>();

    BattleManager_V2 Battle_Mng;
    BattleDungeonData Dungeon;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }
        Battle_Mng = (BattleManager_V2)data[0];
        Dungeon = (BattleDungeonData)data[1];

        SetEnableEscKeyExit(false);
        InitAssets();

        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultPlayerCharacterInfo");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup");

        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        var hero_list = deck.GetDeckHeroes();

        for (int i = 0; i < hero_list.Count; i++)
        {
            var hero = hero_list[i];
            if (!asset_list.Contains(hero.GetUserHeroData().GetPlayerCharacterData().sd_prefab_path))
            {
                asset_list.Add(hero.GetUserHeroData().GetPlayerCharacterData().sd_prefab_path);
            }
        }

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


    protected override void FixedUpdatePopup()
    {
        //  일단 스테미너 사용 완료 하자. (패배시 스테미너 1 사용)
        int cost_stamina = 1;
        var stamina_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.STAMINA);
        if (stamina_item != null)
        {
            stamina_item.UseChargeItem(cost_stamina);
        }

        //  player info
        BeforePlayerInfo();

        //  character info
        AddCharacterInfoNodes();

        Lose_Text_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, LoseTextEaseComplete);
    }

    void BeforePlayerInfo()
    {
        var player_data = GameData.Instance.GetUserGameInfoDataManager().GetCurrentPlayerInfoData();
        int before_lv = player_data.GetLevel();
        float before_exp_per = player_data.GetExpPercentage();

        int gain_player_exp = 1;
        int gain_default_gold = 0;

        Player_Lv.text = before_lv.ToString();
        Player_Exp_Gauge.value = before_exp_per;
        Player_Exp.text = ZString.Format("+{0}", gain_player_exp);
        Stage_Reward_Gold_Count.text = gain_default_gold.ToString("N0");
    }

    /// <summary>
    /// 경험치 획득 후 플레이어 정보 업데이트
    /// </summary>
    void AfterPlayerInfo()
    {
        StartCoroutine(StartAfterPlayerInfo());
    }
    
    IEnumerator StartAfterPlayerInfo()
    {
        var player_data = GameData.Instance.GetUserGameInfoDataManager().GetCurrentPlayerInfoData();
        int before_lv = player_data.GetLevel();
        int gain_player_exp = 1;    //  패배시 플레이어 경험치 1(스테미너 1 사용)
        var result_code = player_data.AddExp(gain_player_exp);
        if (!(result_code == ERROR_CODE.SUCCESS || result_code == ERROR_CODE.LEVEL_UP_SUCCESS))
        {
            yield break;
        }

        int after_lv = player_data.GetLevel();
        int gauge_full_count = after_lv - before_lv;

        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  게이지 풀 횟수
        while (loop_count < gauge_full_count)
        {
            delta += Time.deltaTime;

            Player_Exp_Gauge.value = Mathf.Lerp(Player_Exp_Gauge.value, 1f, delta / duration);
            if (delta >= duration)
            {
                delta = 0f;
                Player_Exp_Gauge.value = 0f;
                ++loop_count;
                Player_Lv.text = (before_lv + loop_count).ToString();

                if (loop_count >= gauge_full_count)
                {
                    break;
                }
            }
            yield return wait;
        }

        //  남은 경험치 게이지 이동
        duration = 1f;
        delta = 0f;
        float last_exp = player_data.GetExpPercentage();
        if (last_exp > 0f)
        {
            while (delta < duration)
            {
                delta += Time.deltaTime;
                Player_Exp_Gauge.value = Mathf.Lerp(Player_Exp_Gauge.value, last_exp, delta / duration);
                yield return wait;
            }
        }

        if (result_code == ERROR_CODE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
            var stamina_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.STAMINA);
            stamina_item.FullChargeItem();
        }
    }


    /// <summary>
    /// 캐릭터 정보 추가하기
    /// </summary>
    void AddCharacterInfoNodes()
    {
        var pool = GameObjectPoolManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        var hero_list = deck.GetDeckHeroes();
        int cnt = hero_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var hero_data = hero_list[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultPlayerCharacterInfo", Character_Info_Container);
            var character_info = obj.GetComponent<GameResultPlayerCharacterInfo>();
            character_info.SetUserHeroData(hero_data.GetUserHeroData());
            Used_Player_Character_Info_List.Add(character_info);
        }
    }
    /// <summary>
    /// 추가된 캐릭터 정보 정리
    /// </summary>
    void ClearCharacterInfos()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Player_Character_Info_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Player_Character_Info_List[i].gameObject);
        }
        Used_Player_Character_Info_List.Clear();
    }


    void LoseTextEaseComplete()
    {
        Player_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, PlayerInfoEaseComplete);
        Player_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);

        Character_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Character_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void PlayerInfoEaseComplete()
    {
        AfterPlayerInfo();
        Home_Btn_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Home_Btn_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    public void OnClickHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        
        var board = BlackBoard.Instance;
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.GAME_TYPE);
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);

        SceneManager.LoadScene("home");
    }

    public override void Despawned()
    {
        //  lose text reset
        Lose_Text_Ease.ResetEase(new Vector2(0.5f, 0.5f));

        //  player info reset
        Player_Info_Ease_Alpha.ResetEase(0f);
        Vector2 player_info_pos = Player_Info_Ease_Slide.transform.localPosition;
        player_info_pos.y = 81;
        Player_Info_Ease_Slide.ResetEase(player_info_pos);

        //  character info reset
        Character_Info_Ease_Alpha.ResetEase(0f);
        Vector2 character_info_pos = Character_Info_Ease_Slide.transform.localPosition;
        character_info_pos.y = -384;
        Character_Info_Ease_Slide.ResetEase(character_info_pos);

        //  home btn
        Home_Btn_Ease_Alpha.ResetEase(0f);
        Vector2 home_btn_pos = Home_Btn_Ease_Slide.transform.localPosition;
        home_btn_pos.y = -160;
        Home_Btn_Ease_Slide.ResetEase(home_btn_pos);

        ClearCharacterInfos();
    }
}
