using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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

    [SerializeField, Tooltip("Reward List View")]
    ScrollRect Reward_List_View;

    
    [Space()]
    [Header("Reward Btn")]
    [SerializeField, Tooltip("Reward Btn Container Ease Slide")]
    UIEaseSlide Reward_Btn_Container_Ease_Slide;

    [SerializeField, Tooltip("Next Stage Btn")]
    UIButtonBase Next_Stage_Btn;

    List<GameResultPlayerCharacterInfo> Used_Player_Character_Info_List = new List<GameResultPlayerCharacterInfo>();

    List<BattleRewardItemCard> Used_Reward_Item_List = new List<BattleRewardItemCard>();

    BattleManager_V2 Battle_Mng;
    BattleDungeonData Dungeon;

    PopupBase Tooltip = null;

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
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/BattleRewardItemCard");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/ItemTooltip");

        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Dungeon.Game_Type);
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
        //  일단 스테미너 사용 완료 하자.
        int cost_stamina = Dungeon.GetPlayerExp();
        if (cost_stamina > 0)
        {
            var stamina_mng = GameData.Instance.GetUserChargeItemDataManager();
            var stamina_item = stamina_mng.FindUserChargeItemData(REWARD_TYPE.STAMINA);
            if (stamina_item != null)
            {
                stamina_item.UseChargeItem(cost_stamina);
            }
            stamina_mng.Save();
        }

        //  player info
        BeforePlayerInfo();

        //  character info
        AddPlayerInfoNodes();

        //  backlight 밝아지기
        Backlight_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        //  승리 텍스트 등장
        WinText_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, WinTextEaseComplete);

        //  next stage btn
        Next_Stage_Btn.gameObject.SetActive(Dungeon.Game_Type == GAME_TYPE.STORY_MODE);
    }

    /// <summary>
    /// 캐릭터 정보 추가<br/>
    /// 덱에 포함되어 있는 캐릭터들의 정보를 보여준다
    /// </summary>
    void AddPlayerInfoNodes()
    {
        var pool = GameObjectPoolManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Dungeon.Game_Type);
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
        int star_count = GetCalcStarPoint();
        StartCoroutine(ShowStarsImpact(star_count));
    }

    /// <summary>
    /// 전투 종료 시점에서 별 획득 계산
    /// </summary>
    /// <returns></returns>
    int GetCalcStarPoint()
    {
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
        return star_count;
    }

    /// <summary>
    /// 별 도장 애니 완료<br/>
    /// 플레이어 정보 및 캐릭터 등장 애니 시작
    /// </summary>
    void ShowStarImpactComplete()
    {
        AfterPlayerInfo();
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
        var player_mng = GameData.Instance.GetUserGameInfoDataManager();
        var player_data = player_mng.GetCurrentPlayerInfoData();
        int before_lv = player_data.GetLevel();
        float before_exp_per = player_data.GetExpPercentage();

        int gain_player_exp = Dungeon.GetPlayerExp();
        int gain_default_gold = Dungeon.GetDefaultClearReward();

        Player_Lv.text = before_lv.ToString();
        Player_Exp_Gauge.value = before_exp_per;
        Player_Exp.text = ZString.Format("+{0}", gain_player_exp);
        Stage_Reward_Gold_Count.text = gain_default_gold.ToString("N0");

        //  금화 획득
        var goods_mng = GameData.Instance.GetUserGoodsDataManager();
        goods_mng.AddUserGoodsCount(GOODS_TYPE.GOLD, gain_default_gold);
        goods_mng.Save();
    }
    /// <summary>
    /// 경험치 획득 후 플레이어 정보 업데이트
    /// </summary>
    void AfterPlayerInfo()
    {
        //  gauge up
        StartCoroutine(StartAfterPlayerInfo());
    }

    IEnumerator StartAfterPlayerInfo()
    {
        var player_mng = GameData.Instance.GetUserGameInfoDataManager();
        var player_data = player_mng.GetCurrentPlayerInfoData();
        int before_lv = player_data.GetLevel();
        int gain_player_exp = Dungeon.GetPlayerExp();
        var result_code = player_data.AddExp(gain_player_exp);
        if (!(result_code == RESPONSE_TYPE.SUCCESS || result_code == RESPONSE_TYPE.LEVEL_UP_SUCCESS))
        {
            yield break;
        }
        player_mng.Save();

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

        if (result_code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
            var stamina_mng = GameData.Instance.GetUserChargeItemDataManager();
            var stamina_item = stamina_mng.FindUserChargeItemData(REWARD_TYPE.STAMINA);
            stamina_item.FullChargeItem();
            stamina_mng.Save();
        }
    }

    /// <summary>
    /// 캐릭터 등장 애니 완료<br/>
    /// 버튼 등장 애니 요청
    /// </summary>
    void CharacterInfoEaseComplete()
    {
        //  캐릭 경험치 / 호감도 경험치 증가
        int character_exp = Dungeon.GetPlayerCharacterExp();
        int destiny_exp = Dungeon.GetPlayerCharacterDestinyExp();

        int cnt = Used_Player_Character_Info_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var player_info = Used_Player_Character_Info_List[i];
            player_info.AfterAddExpHeroInfo(character_exp, destiny_exp);
        }
        //  save data 필요
        Next_Btn_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Next_Btn_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);

        GameData.Instance.GetUserHeroDataManager().Save();
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
        Result_Info_Container.StartMove(UIEaseBase.MOVE_TYPE.MOVE_OUT, ResultInfoContainerCallback);
    }


    public void OnClickGameRewardNext()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        var board = BlackBoard.Instance;
        var m = MasterDataManager.Instance;

        board.RemoveBlackBoardData(BLACK_BOARD_KEY.GAME_TYPE);
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);

        if (Dungeon.Game_Type == GAME_TYPE.STORY_MODE)
        {
            var stage = (Stage_Data)Dungeon.GetDungeonData();
            var next_stage = m.Get_NextStageData(stage.stage_id);
            if (next_stage == null)
            {
                var next_zone = m.Get_ZoneDataByOpenStageID(stage.stage_id);
                if (next_zone != null)
                {
                    var next_zone_stage_list = m.Get_StageDataListByStageGroupID(next_zone.stage_group_id);
                    if (next_zone_stage_list.Count > 0)
                    {
                        next_stage = next_zone_stage_list.FirstOrDefault();
                    }
                }
            }
            if (next_stage != null)
            {
                board.SetBlackBoard(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID, next_stage.stage_id);
            }
        }
        else if (Dungeon.Game_Type == GAME_TYPE.BOSS_DUNGEON_MODE)
        {
            //  보스 던전은 다음 버튼이 필요한가?
        }

        SCManager.I.ChangeScene(SceneName.home);
    }
    public void OnClickGameRewardHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
       
        var board = BlackBoard.Instance;
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.GAME_TYPE);
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);

        SCManager.I.ChangeScene(SceneName.home);
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
    /// <summary>
    /// 보상 박스 진입 완료 후 호출
    /// </summary>
    void RewardBoxEaseComplete()
    {
        var m = MasterDataManager.Instance;
        var pool = GameObjectPoolManager.Instance;
        //  보상 추가
        string reward_prefab = "Assets/AssetResources/Prefabs/UI/Card/BattleRewardItemCard";

        //  first reward list (첫번째 보상인지 여부 체크 필요)
        if (!Dungeon.IsClearedDungeon())
        {
            int first_reward_group_id = Dungeon.GetFirstRewardGroupID();
            if (first_reward_group_id != 0)
            {
                var f_reward_data_list = m.Get_RewardSetDataList(first_reward_group_id);
                if (f_reward_data_list.Count > 0)
                {
                    DROP_TYPE drop_type = f_reward_data_list[0].drop_type;
                    if (drop_type == DROP_TYPE.DROP_EACH)
                    {
                        DropTypeEachReward(reward_prefab, false, f_reward_data_list);
                    }
                    else if (drop_type == DROP_TYPE.DROP_WEIGHT)
                    {
                        DropTypeWeightReward(reward_prefab, f_reward_data_list);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
            }
            
        }

        //  star reward list - (이미 별 보상을 받았는지 체크 필요)
        int star_point = GetCalcStarPoint();
        //  아직 별보상을 받지 않은 상태라면..(별 3개 보상만 있음)
        if (Dungeon.GetStarPoint() < 3 && star_point == 3)
        {
            int star_reward_group_id = Dungeon.GetStarPointRewardGroupID();
            if (star_reward_group_id != 0)
            {
                var star_reward_data_list = m.Get_RewardSetDataList(star_reward_group_id);
                if (star_reward_data_list.Count > 0)
                {
                    DROP_TYPE drop_type = star_reward_data_list[0].drop_type;
                    if (drop_type == DROP_TYPE.DROP_EACH)
                    {
                        DropTypeEachReward(reward_prefab, false, star_reward_data_list);
                    }
                    else if (drop_type == DROP_TYPE.DROP_WEIGHT)
                    {
                        DropTypeWeightReward(reward_prefab, star_reward_data_list);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
            }
            
        }

        //  repeat reward list
        int repeat_reward_group_id = Dungeon.GetRepeatRewardGroupID();
        if (repeat_reward_group_id != 0)
        {
            var repeat_reward_data_list = m.Get_RewardSetDataList(repeat_reward_group_id);
            if (repeat_reward_data_list.Count > 0)
            {
                DROP_TYPE drop_type = repeat_reward_data_list[0].drop_type;
                if (drop_type == DROP_TYPE.DROP_EACH)
                {
                    DropTypeEachReward(reward_prefab, true, repeat_reward_data_list);
                }
                else if (drop_type == DROP_TYPE.DROP_WEIGHT)
                {
                    DropTypeWeightReward(reward_prefab, repeat_reward_data_list);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }
        

        //  버튼 컨테이너 들어오기
        Reward_Btn_Container_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);

        //  스테이지 클리어
        if (Dungeon.Game_Type == GAME_TYPE.STORY_MODE)
        {
            var user_dungeon_data = (UserStoryStageData)Dungeon.GetUserDungeonData();
            if (user_dungeon_data != null)
            {
                var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
                Zone_Data next_zone_data = null;
                bool is_exist_next_zone = false;

                //  다음 존 신규 오픈 여부 판단하기 위해 체크
                var next_stage = m.Get_NextStageData(user_dungeon_data.Stage_ID);
                //  다음 스테이지가 없다는 것은 해당 존의 마지막 스테이지라는것
                if (next_stage == null)
                {
                    //  next zone check
                    next_zone_data = m.Get_ZoneDataByOpenStageID(user_dungeon_data.Stage_ID);
                    if (next_zone_data != null)
                    {
                        if (stage_mng.IsOpenZone(next_zone_data.zone_id))
                        {
                            is_exist_next_zone = true;
                        }
                    }
                }
                
                stage_mng.StoryStageWin(user_dungeon_data.Stage_ID);
                stage_mng.SetStageStarPoint(user_dungeon_data.Stage_ID, star_point);

                //  없었는데, 해당 스테이지 승리 처리 이후 신규 존이 생겼을 경우
                if (!is_exist_next_zone && next_zone_data != null)
                {
                    if (stage_mng.IsOpenZone(next_zone_data.zone_id))
                    {
                        var stage_list = m.Get_StageDataListByStageGroupID(next_zone_data.stage_group_id);
                        var first_stage = stage_list.FirstOrDefault();
                        if (first_stage != null)
                        {
                            string msg = ZString.Format(GameDefine.GetLocalizeString("system_contents_open_message"), GameDefine.GetLocalizeString(first_stage.stage_name_id));
                            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Modal/AlertMessagePopup", POPUP_TYPE.MODAL_TYPE, (popup) =>
                            {
                                popup.ShowPopup(msg);
                            });
                        }
                    }
                }

                
            }
        }
        else if (Dungeon.Game_Type == GAME_TYPE.BOSS_DUNGEON_MODE)
        {
            var user_dungeon_data = (UserBossStageData)Dungeon.GetUserDungeonData();
            if (user_dungeon_data != null)
            {
                var boss_mng = GameData.Instance.GetUserBossStageDataManager();
                boss_mng.BossStageWin(user_dungeon_data.Boss_Dungeon_ID);
                boss_mng.SetStarPoint(user_dungeon_data.Boss_Dungeon_ID, star_point);
            }
        }
        else
        {
            Debug.Assert(false);
        }
        
        GameData.Instance.Save();


    }
    

    /// <summary>
    /// 각각의 아이템 드랍 확률에 따라 드랍한다.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="list"></param>
    void DropTypeEachReward(string prefab, bool is_repeat, IReadOnlyList<Reward_Set_Data> list)
    {
        int cnt = list.Count;
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < cnt; i++)
        {
            var reward_data = list[i];
            int r = Random.Range(0, 1000000);
            if (r < reward_data.drop_per)
            {
                //  스테이지 통상 보상 중, 플레이어 경험치/캐릭터 경험치/호감도 경험치 제외 (통상 보상 중 금화도 제외해야 하는데)
                if (reward_data.reward_type != REWARD_TYPE.FAVORITE && reward_data.reward_type != REWARD_TYPE.EXP_PLAYER && reward_data.reward_type != REWARD_TYPE.EXP_CHARACTER && !(is_repeat && reward_data.reward_type == REWARD_TYPE.GOLD))
                {
                    var obj = pool.GetGameObject(prefab, Reward_List_View.content);
                    var item = obj.GetComponent<BattleRewardItemCard>();
                    item.InitializeData(reward_data, RewardItemCallback);
                    Used_Reward_Item_List.Add(item);

                    //  보상 지급
                    int reward_cnt = AddUserItemReward(reward_data);
                    item.SetCount(reward_cnt);
                }
            }
        }
    }
    /// <summary>
    /// 모든 아이템의 드랍 비중을 합하여 랜덤한 비중 확률로 1개의 아이템만 드랍한다.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="list"></param>
    void DropTypeWeightReward(string prefab, IReadOnlyList<Reward_Set_Data> list)
    {
        int cnt = list.Count;
        int max = list.Sum(x => x.drop_per);
        int sum = 0;
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < cnt; i++)
        {
            var reward_data = list[i];
            sum += reward_data.drop_per;
            int r = Random.Range(0, max);
            if (r < sum)
            {
                var obj = pool.GetGameObject(prefab, Reward_List_View.content);
                var item = obj.GetComponent<BattleRewardItemCard>();
                item.InitializeData(reward_data, RewardItemCallback);
                Used_Reward_Item_List.Add(item);

                //  보상 지급
                int reward_cnt = AddUserItemReward(reward_data);
                item.SetCount(reward_cnt);
                break;
            }
        }
    }

    public void RewardItemCallback(TOUCH_RESULT_TYPE result, System.Func<bool, Rect> hole, object reward_data_obj)
    {
        switch (result)
        {
            case TOUCH_RESULT_TYPE.LONG_PRESS:
                if (reward_data_obj == null || reward_data_obj is not RewardDataBase)
                {
                    Debug.LogWarning("표시 가능한 보상정보가 없습니다!");
                    return;
                }
                RewardDataBase reward_data = reward_data_obj as RewardDataBase;

                TooltipManager.I.Add("Assets/AssetResources/Prefabs/UI/ItemTooltip", hole(false), reward_data);
                break;
        }
    }

    /// <summary>
    /// 실제 사용자 데이터에 지급하는 아이템/재화
    /// </summary>
    /// <param name="reward"></param>
    int AddUserItemReward(Reward_Set_Data reward)
    {
        var gd = GameData.Instance;
        var item_mng = gd.GetUserItemDataManager();     //  각종 아이템 및 조각 아이템
        var goods_mng = gd.GetUserGoodsDataManager();   //  재화
        int item_count = 0;
        var m = MasterDataManager.Instance;
        switch (reward.reward_type)
        {
            case REWARD_TYPE.GOLD:
                item_count = Random.Range(reward.var1, reward.var2);
                goods_mng.AddUserGoodsCount(GOODS_TYPE.GOLD, item_count);
                break;
            case REWARD_TYPE.DIA:
                item_count = Random.Range(reward.var1, reward.var2);
                goods_mng.AddUserGoodsCount(GOODS_TYPE.DIA, item_count);
                break;
            case REWARD_TYPE.STAMINA:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.FAVORITE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EXP_PLAYER:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EXP_CHARACTER:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.CHARACTER:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EQUIPMENT:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.SEND_ESSENCE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.GET_ESSENCE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.BOSS_DUNGEON_TICKET:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.PIECE_EQUIPMENT:
                item_count = reward.var2;
                item_mng.AddUserItemCount(ITEM_TYPE_V2.PIECE_EQUIPMENT, reward.var1, reward.var2);
                break;
            case REWARD_TYPE.PIECE_CHARACTER:
                item_count = reward.var2;
                item_mng.AddUserItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, reward.var1, reward.var2);
                break;
            case REWARD_TYPE.PIECE_ITEM:
                item_count = reward.var2;
                item_mng.AddUserItemCount(ITEM_TYPE_V2.PIECE_ITEM, reward.var1, reward.var2);
                break;
            case REWARD_TYPE.ITEM:
                {
                    var item_data = m.Get_ItemData(reward.var1);
                    if (item_data != null)
                    {
                        item_count = reward.var2;
                        item_mng.AddUserItemCount(item_data.item_type, item_data.item_id, item_count);
                    }
                }
                break;
            default:
                Debug.Assert(false);
                break;
        }

        return item_count;
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
