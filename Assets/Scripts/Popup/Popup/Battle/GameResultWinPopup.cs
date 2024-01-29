using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultWinPopup : PopupBase
{
    [Space()]
    [Header("Result Popup Vars")]
    [SerializeField, Tooltip("Backlight Ease Alpha")]
    UIEaseCanvasGroupAlpha Backlight_Ease;

    [SerializeField, Tooltip("Win Text Ease Scale")]
    UIEaseScale WinText_Ease;

    [SerializeField, Tooltip("Result Info Container")]
    UIEaseCanvasGroupAlpha Result_Info_Container;

    [Space()]
    [Header("Star")]
    [SerializeField, Tooltip("Stars Container Ease Slide")]
    UIEaseSlide Stars_Container_Ease_Slide;

    [SerializeField, Tooltip("Star Container Ease Alpha")]
    UIEaseCanvasGroupAlpha Star_Container_Ease_Alpha;

    [SerializeField, Tooltip("Complete Scale Stars")]
    List<UIEaseScale> Complete_Mission_Star_Ease_Scale_List;

    [SerializeField, Tooltip("Complete Alpha Stars")]
    List<UIEaseCanvasGroupAlpha> Complete_Mission_Star_Ease_Alpha_List;

    [Space()]
    [Header("Player Info")]
    [SerializeField, Tooltip("Player Info Ease Slide")]
    UIEaseSlide Player_Info_Ease_Slide;
    [SerializeField, Tooltip("Player Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Player_Info_Ease_Alpha;

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
    [Header("Next Btn")]
    [SerializeField, Tooltip("Next Btn Ease Alpha")]
    UIEaseCanvasGroupAlpha Next_Btn_Ease_Alpha;
    [SerializeField, Tooltip("Next Btn Ease Slide")]
    UIEaseSlide Next_Btn_Ease_Slide;

    
    [Space()]
    [Header("Reward Info")]
    [SerializeField, Tooltip("Reward Info Container")]
    RectTransform Reward_Info_Container;

    [Space()]
    [SerializeField, Tooltip("Reward Box Ease Alpha")]
    UIEaseCanvasGroupAlpha Reward_Box_Ease_Alpha;
    [SerializeField, Tooltip("Reward Box Ease Scale")]
    UIEaseScale Reward_Box_Ease_Scale;

    
    [Space()]
    [Header("Reward Btn")]
    [SerializeField, Tooltip("Reward Btn Container Ease Slide")]
    UIEaseSlide Reward_Btn_Container_Ease_Slide;


    List<GameResultPlayerCharacterInfo> Used_Player_Character_Info_List = new List<GameResultPlayerCharacterInfo>();

    BattleManager_V2 Battle_Mng;
    BattleDungeonData Dungeon;

    public override void ShowPopup(params object[] data)
    {
        if (data.Length != 2)
        {
            HidePopup();
            return;
        }
        Battle_Mng = (BattleManager_V2)data[0];
        Dungeon = (BattleDungeonData)data[1];

        base.ShowPopup(data);
        SetEnableEscKeyExit(false);
        InitAssets();
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultPlayerCharacterInfo");

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
        //  player info
        BeforePlayerInfo();

        //  character info

        AddPlayerInfoNodes();

        //  backlight 밝아지기
        Backlight_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        //  승리 텍스트 등장
        WinText_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, WinTextEaseComplete);
    }

    /// <summary>
    /// 캐릭터 정보 추가<br/>
    /// 덱에 포함되어 있는 캐릭터들의 정보를 보여준다
    /// </summary>
    void AddPlayerInfoNodes()
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

    /// <summary>
    /// Win 텍스트 등장 완료 후, 별등급/플레이어 정보 박스 등장
    /// </summary>
    void WinTextEaseComplete()
    {
        Stars_Container_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, StarContainerEaseComplete);
        Star_Container_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }
    /// <summary>
    /// 별 등급 등장 애니 완료<br/>
    /// 몇개의 별 등급을 받을지 확인 후 해당 수 만큼의 별 도장 꽝꽝!!
    /// </summary>
    void StarContainerEaseComplete()
    {
        //  최대 별 갯수 3개
        int star_count = 3;
        var team_mng = Battle_Mng.FindTeamManager(TEAM_TYPE.LEFT);
        if (team_mng != null)
        {
            //  팀원 1명 죽을때마다 별 1개 감소
            int total_members = team_mng.GetTotalMemberCount();
            int alive_members = team_mng.GetAliveMemberCount();
            star_count = 3 - (total_members - alive_members);
            //  클리어시 최소 별 1개 지급
            if (star_count < 1)
            {
                star_count = 1;
            }
        }
        StartCoroutine(ShowStarsImpact(star_count));
    }
    /// <summary>
    /// 별 도장 애니 완료<br/>
    /// 플레이어 정보 및 캐릭터 등장 애니 시작
    /// </summary>
    void ShowStarImpactComplete()
    {
        Player_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Player_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Character_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, CharacterInfoEaseComplete);
        Character_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }
    /// <summary>
    /// 경험치 획득 전 플레이어 정보 업데이트
    /// </summary>
    void BeforePlayerInfo()
    {
        var player_data = GameData.Instance.GetUserGameInfoDataManager().GetCurrentPlayerInfoData();
        int before_lv = player_data.GetLevel();
        float before_exp_per = player_data.GetExpPercentage();

        int gain_player_exp = Dungeon.GetPlayerExp();
        //int gain_character_exp = Dungeon.GetPlayerCharacterExp();
        //int gain_destiny_exp = Dungeon.GetPlayerCharacterDestinyExp();
        int gain_default_gold = Dungeon.GetDefaultClearReward();

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

    }
    
    /// <summary>
    /// 캐릭터 등장 애니 완료<br/>
    /// 버튼 등장 애니 요청
    /// </summary>
    void CharacterInfoEaseComplete()
    {
        Next_Btn_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Next_Btn_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }
    /// <summary>
    /// 별 도장 애니
    /// </summary>
    /// <param name="cnt"></param>
    /// <returns></returns>
    IEnumerator ShowStarsImpact(int cnt)
    {
        if (cnt > 3)
        {
            cnt = 3;
        }
        for (int i = 0; i < cnt; i++)
        {
            var scale = Complete_Mission_Star_Ease_Scale_List[i];
            scale.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
            var alpha = Complete_Mission_Star_Ease_Alpha_List[i];
            alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
            yield return new WaitForSeconds(0.2f);
        }
        ShowStarImpactComplete();
    }


    public void OnClickGameResultOK()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //  todo go home / next stage
        //HidePopup();

        Result_Info_Container.StartMove(UIEaseBase.MOVE_TYPE.MOVE_OUT, ResultInfoContainerCallback);
    }


    public void OnClickGameRewardNext()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        SceneManager.LoadScene("home");
    }
    public void OnClickGameRewardHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        SceneManager.LoadScene("home");

    }
    /// <summary>
    /// 결과 정보를 숨기고 보상 컨테이너를 보여주기 위해 요청
    /// </summary>
    void ResultInfoContainerCallback()
    {
        Reward_Info_Container.gameObject.SetActive(true);
        Reward_Box_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, RewardBoxEaseComplete);
        Reward_Box_Ease_Scale.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void RewardBoxEaseComplete()
    {
        Reward_Btn_Container_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    public override void Despawned()
    {
        //  backlight alpha 초기화
        Backlight_Ease.ResetEase(0f);
        //  wintextease init
        WinText_Ease.ResetEase(new Vector2(0.5f, 0.5f));

        //  star ease init

        Vector2 star_pos = Stars_Container_Ease_Slide.transform.localPosition;
        star_pos.y = 174;
        Stars_Container_Ease_Slide.ResetEase(star_pos);

        Star_Container_Ease_Alpha.ResetEase(0f);

        int cnt = Complete_Mission_Star_Ease_Scale_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var scale = Complete_Mission_Star_Ease_Scale_List[i];
            scale.ResetEase(new Vector2(5f, 5f));

            var alpha = Complete_Mission_Star_Ease_Alpha_List[i];
            alpha.ResetEase(0f);
        }

        //  player info init
        Vector2 player_info_pos = Player_Info_Ease_Slide.transform.localPosition;
        player_info_pos.y = 81;
        Player_Info_Ease_Slide.ResetEase(player_info_pos);

        Player_Info_Ease_Alpha.ResetEase(0f);

        //  character info init
        Character_Info_Ease_Alpha.ResetEase(0f);

        Vector2 char_pos = Character_Info_Ease_Slide.transform.localPosition;
        char_pos.y = -384;
        Character_Info_Ease_Slide.ResetEase(char_pos);

        //  next btn
        Next_Btn_Ease_Alpha.ResetEase(0f);

        Vector2 next_pos = Next_Btn_Ease_Slide.transform.localPosition;
        next_pos.y = -80;
        Next_Btn_Ease_Slide.ResetEase(next_pos);

        //  result info
        Result_Info_Container.ResetEase(1f);


        Reward_Info_Container.gameObject.SetActive(false);

        Reward_Box_Ease_Alpha.ResetEase(0f);
        Reward_Box_Ease_Scale.ResetEase(Vector2.zero);

        Vector2 reward_btn_pos = Reward_Btn_Container_Ease_Slide.transform.localPosition;
        reward_btn_pos.y = -160;
        Reward_Btn_Container_Ease_Slide.ResetEase(reward_btn_pos);

        ClearCharacterInfos();
    }



}
