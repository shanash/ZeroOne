using Cysharp.Text;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultPopup : PopupBase
{
    [Space()]
    [Header("배경")]
    [SerializeField, Tooltip("배경 Blur 이미지")]
    RawImage BG_Image;

    [Space()]
    [Header("시퀀스 0 => 영웅 대기")]
    [SerializeField, Tooltip("영웅 대기 장소")]
    RectTransform Waiting_Room;
    

    [Space()]
    [Header("시퀀스 1 => 승리/패배 애니")]

    [SerializeField, Tooltip("승리 - 자동 플레이")]
    GameObject Title_Victory;
    [SerializeField, Tooltip("패배 - 자동 플레이")]
    GameObject Title_Defeat;

    [Space()]
    [Header("시퀀스 2 => 별 애니")]
    [SerializeField, Tooltip("별 갯수 애니")]
    Animator Result_Star_Anim;

    [Space()]
    [Header("시퀀스 3 => 경험치 결과 애니")]
    [SerializeField, Tooltip("경험치 결과 컨테이너 - 자동 플레이")]
    GameObject Result_Exp;

    [Space()]
    [Header("시퀀스 4 => 플레이어 경험치 증가 보여주기 - 레벨업 시 레벨업 팝업 보여준 후 시퀀스 5로 넘어가기")]
    [SerializeField, Tooltip("플레이어 닉네임")]
    TMP_Text Player_Nickname;
    [SerializeField, Tooltip("플레이어 경험치 게이지")]
    Slider Player_Exp_Gauge;
    [SerializeField, Tooltip("플레이어 획득 경험치")]
    TMP_Text Player_Gain_Exp;
    [SerializeField, Tooltip("플레이어 레벨")]
    TMP_Text Player_Level_Text;
    [SerializeField, Tooltip("기본 보상 아이템")]
    Image Default_Reward_Item_Icon;
    [SerializeField, Tooltip("기본 보상 아이템 획득 갯수")]
    TMP_Text Default_Reward_Item_Count;

    [Space()]
    [Header("시퀀스 5 => 각 캐릭터별 순서대로 경험치 게이지 증가(레벨업 시 레벨업 표시하고 다음으로)")]
    [SerializeField, Tooltip("캐릭터 정보 리스트 컨테이너")]
    RectTransform Hero_Info_List_Container;

    [Space()]
    [Header("시퀀스 6 => 버튼 보이기")]
    [SerializeField, Tooltip("다음 버튼")]
    Button Next_Btn;
    [SerializeField, Tooltip("다음 버튼 텍스트")]
    TMP_Text Next_Btn_Text;

    [Space()]
    [Header("시퀀스 종료 이벤트 콜백")]
    [SerializeField, Tooltip("이벤트 콜백")]
    List<BattleResultAnimEventCallback> Anim_Event_Callback_List;

    /// <summary>
    /// 대기중 영웅 노드 리스트
    /// </summary>
    List<WaitHeroNode> Used_Wait_Hero_Node_List = new List<WaitHeroNode>();

    /// <summary>
    /// 영웅 레벨 정보 리스트
    /// </summary>
    List<BattleResultPlayerCharacterInfo> Used_Hero_Info_List = new List<BattleResultPlayerCharacterInfo>();
    int Hero_Info_Index;

    /// <summary>
    /// 보상 획득 정보
    /// </summary>
    List<BATTLE_RESULT_REWARD_DATA> Result_Reward_List = new List<BATTLE_RESULT_REWARD_DATA>();

    BattleManager_V2 Battle_Mng;
    BattleDungeonData Dungeon;
    bool Is_Win;
    Texture BG_Texture;         //  blur 배경 이미지 텍스쳐

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 4)
        {
            return false;
        }
        Is_Win = (bool)data[0];
        Battle_Mng = (BattleManager_V2)data[1];
        Dungeon = (BattleDungeonData)data[2];
        BG_Texture = (Texture)data[3];

        SetEnableEscKeyExit(false);
        InitAssets();
        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/WaitHeroNode");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/BattleRewardItemCard");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/ItemTooltip");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/Bulkhead");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleResultRewardItem");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleResultPlayerCharacterInfo_V2");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleResultRewardPopup");

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

        BG_Image.texture = BG_Texture;

        //  애니메이션 종료 콜백 등록
        for (int i = 0; i < Anim_Event_Callback_List.Count; i++)
        {
            Anim_Event_Callback_List[i].SetAnimationEndEventCallback(AnimationEndCallback);
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

    void AnimationEndCallback(string evt_name)
    {
        if (evt_name.Equals("victory_end") || evt_name.Equals("defeat_end"))
        {
            StartStarPointAnimation();
        }
        else if (evt_name.Equals("star_end"))
        {
            Result_Exp.SetActive(true);
        }
        else if (evt_name.Equals("exp_end"))
        {
            StartPlayerExpInfo();
        }
        else if (evt_name.Equals("star_fx"))
        {
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/SE_star");
        }
    }

    protected override void FixedUpdatePopup()
    {
        //  스태메너 소모
        if (Dungeon.Game_Type == GAME_TYPE.STORY_MODE)
        {
            int cost_stamina = Dungeon.GetPlayerExp();
            if (!Is_Win)
            {
                cost_stamina = 1;
            }
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
        }

        //  시퀀스 0 (영웅 대기열 추가)
        AddWaitingHeros();
        //  시퀀스 1 승리/패배 연출
        if (Is_Win)
        {
            Title_Victory.SetActive(true);
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/SFX_Win");
        }
        else
        {
            Title_Defeat.SetActive(true);
        }

        //  플레이어 레벨업 전 정보 적용
        BeforePlayerInfo();
        //  캐릭터 레벨업 전 정보 적용
        AddBeforeHeroLevelInfos();

        if (Is_Win)
        {
            //  아이템 보상 정보 미리 획득하고 획득된 데이터 보유하고 있다가 보상 팝업 정보에 전달해주기
            var m = MasterDataManager.Instance;
            //  첫 보상 여부 
            if (Dungeon.IsClearedDungeon())
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
                            DropTypeEachReward(false, f_reward_data_list);
                        }
                        else if (drop_type == DROP_TYPE.DROP_WEIGHT)
                        {
                            DropTypeWeightReward(f_reward_data_list);
                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                    }
                }
            }

            //  별보상 (아직 별보상을 받지 않았고, 별 3개 보상만 지급)
            int star_point = GetCalcStarPoint();
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
                            DropTypeEachReward(false, star_reward_data_list);
                        }
                        else if (drop_type == DROP_TYPE.DROP_WEIGHT)
                        {
                            DropTypeWeightReward(star_reward_data_list);
                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                    }
                }
            }

            //  반복 보상
            int repeat_reward_group_id = Dungeon.GetRepeatRewardGroupID();
            if (repeat_reward_group_id != 0)
            {
                var repeat_reward_data_list = m.Get_RewardSetDataList(repeat_reward_group_id);
                if (repeat_reward_data_list.Count > 0)
                {
                    DROP_TYPE drop_type = repeat_reward_data_list[0].drop_type;
                    if (drop_type == DROP_TYPE.DROP_EACH)
                    {
                        DropTypeEachReward(true, repeat_reward_data_list);
                    }
                    else if (drop_type == DROP_TYPE.DROP_WEIGHT)
                    {
                        DropTypeWeightReward(repeat_reward_data_list);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
            }

            CheckStageClear();
        }
        
    }
    /// <summary>
    /// 스테이지 클리어 체크
    /// </summary>
    void CheckStageClear()
    {
        var m = MasterDataManager.Instance;
        int star_point = GetCalcStarPoint();
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
                            string stage_name = ZString.Format("{0} {1}", GameDefine.GetLocalizeString(next_zone_data.zone_name_id), GameDefine.GetLocalizeString(first_stage.stage_name_id));
                            string msg = ZString.Format(GameDefine.GetLocalizeString("system_contents_open_message"), stage_name);
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

    void DropTypeEachReward(bool is_repeat, IReadOnlyList<Reward_Set_Data> list)
    {
        int cnt = list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var data = list[i];
            int r = UnityEngine.Random.Range(0, 1000000);
            if (r < data.drop_per)
            {
                //  스테이지 통상 보상 중, 플레이어 경험치/캐릭터 경험치/호감도 경험치 제외(통상 보상 중 금화도 제외)
                if (data.reward_type != REWARD_TYPE.FAVORITE && data.reward_type != REWARD_TYPE.EXP_PLAYER && data.reward_type != REWARD_TYPE.EXP_CHARACTER && !(is_repeat && data.reward_type == REWARD_TYPE.GOLD))
                {
                    BATTLE_RESULT_REWARD_DATA reward = new BATTLE_RESULT_REWARD_DATA();
                    reward.Data = data;
                    reward.Count = AddUserItemReward(data);

                    Result_Reward_List.Add(reward);
                }
            }
        }
        
    }

    void DropTypeWeightReward(IReadOnlyList<Reward_Set_Data> list)
    {
        //BATTLE_RESULT_REWARD_DATA reward = new BATTLE_RESULT_REWARD_DATA();
        int cnt = list.Count;
        int max = list.Sum(x => x.drop_per);
        int sum = 0;
        for (int i = 0; i < cnt; i++)
        {
            var data = list[i];
            sum += data.drop_per;
            int r = UnityEngine.Random.Range(0, max);
            if (r < sum)
            {
                BATTLE_RESULT_REWARD_DATA reward = new BATTLE_RESULT_REWARD_DATA();
                reward.Data = data;
                reward.Count = AddUserItemReward(data);
                Result_Reward_List.Add(reward);
                break;
            }
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
        var hero_mng = gd.GetUserHeroDataManager();     //  영웅 리스트
        int item_count = 0;
        var m = MasterDataManager.Instance;
        switch (reward.reward_type)
        {
            case REWARD_TYPE.GOLD:
                item_count = UnityEngine.Random.Range(reward.var1, reward.var2);
                goods_mng.AddUserGoodsCount(GOODS_TYPE.GOLD, item_count);
                break;
            case REWARD_TYPE.DIA:
                item_count = UnityEngine.Random.Range(reward.var1, reward.var2);
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
                {
                    //  캐릭터가 있는지 여부 판단(있으면 조각 60개로 변경)
                    if (hero_mng.IsExistHeroData(reward.var1))
                    {
                        item_count = 60;
                        item_mng.AddUserItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, reward.var1, item_count);
                    }
                    else // 없으면 신규 추가
                    {
                        item_count = reward.var2;
                        hero_mng.AddUserHeroData(reward.var1);
                    }
                }
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


    /// <summary>
    /// 레벨업 전 플레이어의 정보 적용
    /// </summary>
    void BeforePlayerInfo()
    {
        var player_mng = GameData.Instance.GetUserGameInfoDataManager();
        var player_data = player_mng.GetCurrentPlayerInfoData();
        int before_lv = player_data.GetLevel();
        float before_exp_per = player_data.GetExpPercentage();

        int gain_player_exp = Dungeon.GetPlayerExp();
        int gain_default_gold = Dungeon.GetDefaultClearReward();
        if (!Is_Win)
        {
            gain_player_exp = 1;
            gain_default_gold = 0;
        }

        Player_Level_Text.text = before_lv.ToString();
        Player_Exp_Gauge.value = before_exp_per;
        Player_Gain_Exp.text = ZString.Format(GameDefine.GetLocalizeString("system_plus_format"), gain_player_exp);

        //  gold
        Default_Reward_Item_Count.text = gain_default_gold.ToString("N0");

        //  금화 획득
        var goods_mng = GameData.Instance.GetUserGoodsDataManager();
        goods_mng.AddUserGoodsCount(GOODS_TYPE.GOLD, gain_default_gold);
        goods_mng.Save();
    }

    /// <summary>
    /// 경험치 등장 애니메이션 종효 후 플레이어의 레벨업 게이지 표시
    /// </summary>
    void AfterPlayerInfo()
    {
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
        if (gauge_full_count > 1)
        {
            gauge_full_count = 1;
        }
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  게이지 풀 횟수
        while (loop_count < gauge_full_count)
        {
            delta += Time.deltaTime * 3f;

            Player_Exp_Gauge.value = Mathf.Lerp(Player_Exp_Gauge.value, 1f, delta / duration);
            if (delta >= duration)
            {
                delta = 0f;
                Player_Exp_Gauge.value = 0f;
                ++loop_count;
                Player_Level_Text.text = after_lv.ToString();

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
                delta += Time.deltaTime * 3f;
                Player_Exp_Gauge.value = Mathf.Lerp(Player_Exp_Gauge.value, last_exp, delta / duration);
                yield return wait;
            }
        }

        if (result_code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.SetHideCompleteCallback(() =>
                {
                    //  캐릭터 레벨 증가 표시
                    AfterHeroLevelInfos();
                });
                popup.ShowPopup();
            });
            var stamina_mng = GameData.Instance.GetUserChargeItemDataManager();
            var stamina_item = stamina_mng.FindUserChargeItemData(REWARD_TYPE.STAMINA);
            stamina_item.FullChargeItem();
            stamina_mng.Save();
        }
        else
        {
            //  캐릭터 레벨 증가 표시
            AfterHeroLevelInfos();
        }
        
    }

    /// <summary>
    /// 전투 참여 영웅의 레벨 정보 추가(레벨 정보 적용전)
    /// </summary>
    void AddBeforeHeroLevelInfos()
    {
        var pool = GameObjectPoolManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Dungeon.Game_Type);
        var hero_list = deck.GetDeckHeroes();
        for (int i = 0; i < hero_list.Count; i++)
        {
            //  seperator line
            if (0 < i)
            {
                pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Battle/Bulkhead", Hero_Info_List_Container);
            }
            var hero_data = hero_list[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleResultPlayerCharacterInfo_V2", Hero_Info_List_Container);
            var info_node = obj.GetComponent<BattleResultPlayerCharacterInfo>();
            info_node.SetUserHeroData(hero_data.GetUserHeroData());
            Used_Hero_Info_List.Add(info_node);
        }

        Used_Hero_Info_List.Reverse();
    }

    void AfterHeroLevelInfos()
    {
        //  캐릭 경험치 / 호감도 경험치 증가
        if (Is_Win)
        {
            int character_exp = Dungeon.GetPlayerCharacterExp();
            int destiny_exp = Dungeon.GetPlayerCharacterDestinyExp();
            Hero_Info_Index = 0;
            var info_node = Used_Hero_Info_List[Hero_Info_Index];
            info_node.AfterAddExpHeroInfo(character_exp, destiny_exp, HeroLevelUpEndCallback);
        }
        else
        {
            //  next step
            Next_Btn.gameObject.SetActive(true);
            Next_Btn.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    void HeroLevelUpEndCallback()
    {
        Hero_Info_Index++;
        if (Hero_Info_Index < Used_Hero_Info_List.Count)
        {
            //  캐릭 경험치 / 호감도 경험치 증가
            int character_exp = Dungeon.GetPlayerCharacterExp();
            int destiny_exp = Dungeon.GetPlayerCharacterDestinyExp();
            if (!Is_Win)
            {
                character_exp = 1;
                destiny_exp = 0;
            }
            var info_node = Used_Hero_Info_List[Hero_Info_Index];
            info_node.AfterAddExpHeroInfo(character_exp, destiny_exp, HeroLevelUpEndCallback);
        }
        else
        {
            //  next step
            Next_Btn.gameObject.SetActive(true);
            Next_Btn.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }
    

    /// <summary>
    /// 대기 영웅 추가
    /// </summary>
    void AddWaitingHeros()
    {
        var pool = GameObjectPoolManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Dungeon.Game_Type);
        var hero_list = deck.GetDeckHeroes();

        for (int i = 0; i < hero_list.Count; i++)
        {
            var hero_data = hero_list[i];


            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Battle/WaitHeroNode", Waiting_Room);
            var wait = obj.GetComponent<WaitHeroNode>();
            wait.SetUserHeroData(hero_data.GetUserHeroData());
            Used_Wait_Hero_Node_List.Add(wait);
        }
    }

    /// <summary>
    /// 별 획득 애니메이션 시작
    /// </summary>
    void StartStarPointAnimation()
    {
        Result_Star_Anim.gameObject.SetActive(true);
        if (Is_Win)
        {
            int star_point = Math.Max(Dungeon.GetStarPoint(), GetCalcStarPoint());
            Result_Star_Anim.Play($"Result_Star{star_point.ToString("D2")}");
        }
        else
        {
            Result_Star_Anim.Play("Result_Star00");
        }
    }
    /// <summary>
    /// 플레이어 경험치 획득 모양 추가
    /// </summary>
    void StartPlayerExpInfo()
    {
        //  플레이어 경험치 증가 애니 보여주기
        AfterPlayerInfo();
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

    public void OnClickNext()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        if (Is_Win)
        {
            HidePopup(() => {
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleResultRewardPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
                {
                    popup.ShowPopup(Dungeon, BG_Texture, Result_Reward_List);
                });
            });
        }
        else
        {
            //  go home
            var board = BlackBoard.Instance;
            board.RemoveBlackBoardData(BLACK_BOARD_KEY.GAME_TYPE);
            board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);

            SCManager.I.ChangeScene(SceneName.home);

        }
    }

    public override void Spawned()
    {
        Title_Victory.SetActive(false);
        Title_Defeat.SetActive(false);
        Result_Star_Anim.gameObject.SetActive(false);
        Result_Exp.SetActive(false);
    }
}
