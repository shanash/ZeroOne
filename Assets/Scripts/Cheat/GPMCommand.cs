using FluffyDuck.Util;
using Gpm.LogViewer;
using System;
using UnityEngine;

public class GPMCommand : MonoBehaviour
{
    private void Start()
    {
        InitCommand();
    }

    void InitCommand()
    {
        var func = Function.Instance;
        //  cheat key callback
        func.AddCheatKeyCallback(CheatKeyCallback);

        //  commands
        func.AddCommand(this, "PlayerLevelUp", "플레이어 레벨업");
        func.AddCommand(this, "AllCharacterLevelUp", "모든 캐릭터 레벨업");

        func.AddCommand(this, "FullChargeEssence", "근원 포인트 충전");
        func.AddCommand(this, "RechargeHeroEssenceSendCount", "근원 전달 횟수 초기화");

        func.AddCommand(this, "FullChargeStamina", "스테미나 충전");

        func.AddCommand(this, "FullChargeBossStageEntranceCount", "보스전 입장 횟수 충전");

        func.AddCommand(this, "CheatAttackInc", "전투 공격력 <color=#ff0000>x10</color>", new object[] { 10 });
        func.AddCommand(this, "CheatAttackInc", "전투 공격력 <color=#ff0000>x50</color>", new object[] { 50 });

        func.AddCommand(this, "CheatDefenseInc", "전투 방어력 <color=#ffff00>x10</color>", new object[] { 10 });
        func.AddCommand(this, "CheatDefenseInc", "전투 방어력 <color=#ffff00>x50</color>", new object[] { 50 });
        
        func.AddCommand(this, "CheatCriticalChanceInc", "크리티컬 확률 <color=#00ff00>x100</color>", new object[] { 100 });


    }
    /// <summary>
    /// 플레이어 레벨업<br/>
    /// 캐릭터 레벨의 최대 레벨이 플레이어 레벨이기 때문에 플레이어 레벨을 상승 시켜야 함
    /// </summary>
    void PlayerLevelUp()
    {
        var gd = GameData.Instance;
        var player_info_mng = gd.GetUserGameInfoDataManager();
        var player_info = player_info_mng.GetCurrentPlayerInfoData();
        double need_exp = player_info.GetNextExp();
        player_info.AddExp(need_exp);
        player_info_mng.Save();

        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_PLAYER_INFO);

        PopupManager.Instance.CloseAll();

        CommonUtils.ShowToast($"플레이어 레벨 {player_info.GetLevel()} 달성", TOAST_BOX_LENGTH.SHORT);
    }

    /// <summary>
    /// 모든 캐릭터 레벨 +1<br/>
    /// 모든 캐릭터의 레벨을 +1씩 증가<br/>
    /// 최대 레벨은 플레이어 레벨이기 때문에, 캐릭터 레벨이 상승하지 않는다면, 플레이어 레벨을 올릴 필요가 있음
    /// </summary>
    void AllCharacterLevelUp()
    {
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        var hero_list = hero_mng.GetUserHeroDataList();
        for (int i = 0; i < hero_list.Count; i++)
        {
            var hero = hero_list[i];
            double need_exp = hero.GetNextExp();
            hero.AddExp(need_exp);
        }
        hero_mng.Save();

        PopupManager.Instance.CloseAll();

        CommonUtils.ShowToast("모든 캐릭터 레벨 +1 상승", TOAST_BOX_LENGTH.SHORT);
    }
   
    /// <summary>
    /// 근원 포인트 충전
    /// </summary>
    void FullChargeEssence()
    {
        var gd = GameData.Instance;
        var charge_mng = gd.GetUserChargeItemDataManager();
        var stamina = charge_mng.FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
        stamina.FullChargeItem();
        charge_mng.Save();
        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE);

        CommonUtils.ShowToast("근원 포인트 충전 완료", TOAST_BOX_LENGTH.SHORT);
    }

    /// <summary>
    /// 스테미너 풀 충전
    /// </summary>
    void FullChargeStamina()
    {
        var gd = GameData.Instance;
        var charge_mng = gd.GetUserChargeItemDataManager();
        var stamina = charge_mng.FindUserChargeItemData(REWARD_TYPE.STAMINA);
        stamina.FullChargeItem();
        charge_mng.Save();
        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_STAMINA);
        CommonUtils.ShowToast("스테미너 충전 완료", TOAST_BOX_LENGTH.SHORT);
    }
    /// <summary>
    /// 보스전 입장 횟수 충전
    /// </summary>
    void FullChargeBossStageEntranceCount()
    {
        var gd = GameData.Instance;
        var boss_mng = gd.GetUserBossStageDataManager();
        var result = boss_mng.FullChargeEntranceCount();
        if (result == RESPONSE_TYPE.SUCCESS)
        {
            CommonUtils.ShowToast("보스전 입장 횟수 충전 완료", TOAST_BOX_LENGTH.SHORT);
            boss_mng.Save();
        }
        else
        {
            CommonUtils.ShowToast("보스전 입장 횟수 충전 오류", TOAST_BOX_LENGTH.SHORT);
        }

    }

    /// <summary>
    /// 공격력 10배 증가
    /// </summary>
    void CheatAttackInc(int multiple)
    {
        var board = BlackBoard.Instance;
        board.SetBlackBoard(BLACK_BOARD_KEY.PLAYER_ATTACK_INC_MULTIPLE, multiple);
        CommonUtils.ShowToast($"전투 공격력이 {multiple}배 증가했습니다.", TOAST_BOX_LENGTH.SHORT);
    }
    /// <summary>
    /// 방어력 10배 증가
    /// </summary>
    void CheatDefenseInc(int multiple)
    {
        var board = BlackBoard.Instance;
        board.SetBlackBoard(BLACK_BOARD_KEY.PLAYER_DEFENSE_INC_MULTIPLE, multiple);
        CommonUtils.ShowToast($"전투 방어력이 {multiple}배 증가했습니다.", TOAST_BOX_LENGTH.SHORT);
    }

    void CheatCriticalChanceInc(int multiple)
    {
        var board = BlackBoard.Instance;
        board.SetBlackBoard(BLACK_BOARD_KEY.PLAYER_CRITICAL_CHANCE_INC_MULTIPLE, multiple);
        CommonUtils.ShowToast($"전투 크리티컬 확률이 {multiple}배 증가했습니다.", TOAST_BOX_LENGTH.SHORT);
    }
    /// <summary>
    /// 각종 치트 키 사용 콜백<br/>
    /// 키로서 사용할 필요가 있는 경우
    /// </summary>
    /// <param name="cheat_key"></param>
    void CheatKeyCallback(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }

        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");
        if (keys[0].Equals("gold"))
        {
            GainGold(cheat_key);
        }
        else if (keys[0].Equals("dia"))
        {
            GainDia(cheat_key);
        }
        else if (keys[0].Equals("cpiece"))
        {
            GainCharacterPiece(cheat_key);
        }
        else if (keys[0].Equals("item"))
        {
            GainItem(cheat_key);
        }
        else if (keys[0].Equals("hero"))
        {
            GainHero(cheat_key);
        }
        else if (keys[0].Equals("zone"))
        {
            ClearZone(cheat_key);
        }
        else if (keys[0].Equals("story"))
        {
            ClearStoryStage(cheat_key);
        }
        else if (keys[0].Equals("ess"))
        {
            GainSendEssence(cheat_key);
        }
        else
        {
            return;
        }
        PopupManager.Instance.CloseAll();
    }
    /// <summary>
    /// 스토리 존 클리어(지정 존의 스테이지를 모두 클리어) - 존 ID(난이도 포함)
    /// </summary>
    /// <param name="cheat_key">zone [zone_id] [star_count]</param>
    void ClearZone(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        var m = MasterDataManager.Instance;
        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");

        if (keys.Length == 3)
        {
            var stage_mng = gd.GetUserStoryStageDataManager();
            if (int.TryParse(keys[1], out int zone_id) && int.TryParse(keys[2], out int star_point))
            {
                var zone_data = m.Get_ZoneData(zone_id);
                bool is_all_clear = true;
                if (zone_data != null)
                {
                    var stage_list = m.Get_StageDataListByStageGroupID(zone_data.stage_group_id);
                    for (int i = 0; i < stage_list.Count; i++)
                    {
                        var stage_data = stage_list[i];
                        var found = stage_mng.FindUserStoryStageData(stage_data.stage_id);
                        if (found == null)
                        {
                            CommonUtils.ShowToast($"{stage_data.stage_id} 스테이지가 오픈되지 않았습니다.", TOAST_BOX_LENGTH.SHORT);
                            is_all_clear = false;
                            break;
                        }
                        found.AddChallenageCount();
                        stage_mng.StoryStageWin(stage_data.stage_id);
                        stage_mng.SetStageStarPoint(stage_data.stage_id, star_point);
                    }
                    if (is_all_clear)
                    {
                        CommonUtils.ShowToast($"{zone_data.zone_name} 이 모두 클리어 되었습니다.", TOAST_BOX_LENGTH.SHORT);
                        stage_mng.Save();
                    }
                }
                else
                {
                    CommonUtils.ShowToast($"[{zone_id}] 존이 존재하지 않습니다.", TOAST_BOX_LENGTH.SHORT);
                }
            }
        }
        else
        {
            CommonUtils.ShowToast("Usage => zone [zone_id] [star_count]", TOAST_BOX_LENGTH.SHORT);
        }
    }

    /// <summary>
    /// 지정 스토리 스테이지 클리어
    /// </summary>
    /// <param name="cheat_key">story [stage_id] [star_count]</param>
    void ClearStoryStage(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");

        if (keys.Length == 3)
        {
            var stage_mng = gd.GetUserStoryStageDataManager();
            if (int.TryParse(keys[1], out int stage_id) && int.TryParse(keys[2], out int star_point))
            {
                var stage = stage_mng.FindUserStoryStageData(stage_id);
                if (stage != null)
                {
                    stage.AddChallenageCount();
                    stage_mng.StoryStageWin(stage_id);
                    stage_mng.SetStageStarPoint(stage_id, star_point);
                    stage_mng.Save();
                    string stage_name = $"{GameDefine.GetLocalizeString(stage.GetZoneData().zone_name_id)} {GameDefine.GetLocalizeString(stage.GetStageData().stage_name_id)}";
                    CommonUtils.ShowToast($"[{stage_name}] 스테이지가 클리어 되었습니다.", TOAST_BOX_LENGTH.SHORT);
                }
                else
                {
                    CommonUtils.ShowToast($"[{stage_id}] 스테이지가 오픈되지 않았습니다.", TOAST_BOX_LENGTH.SHORT);
                }
            }
        }
        else
        {
            string msg = $"Usage => story [stage_id] [star_count]";
            CommonUtils.ShowToast(msg, TOAST_BOX_LENGTH.SHORT);
        }
    }

    void GainGold(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");

        if (keys.Length == 2)
        {
            if (double.TryParse(keys[1], out double gold))
            {
                gd.GetUserGoodsDataManager().AddUserGoodsCount(GOODS_TYPE.GOLD, gold);
                gd.Save();
                UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ALL);

                CommonUtils.ShowToast($"골드 {gold} 획득 완료", TOAST_BOX_LENGTH.SHORT);
            }
        }
    }
    /// <summary>
    /// 보석 획득
    /// </summary>
    /// <param name="cheat_key"></param>
    void GainDia(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");

        if (keys.Length == 2)
        {
            if (double.TryParse(keys[1], out double dia))
            {
                gd.GetUserGoodsDataManager().AddUserGoodsCount(GOODS_TYPE.DIA, dia);
                gd.Save();
                UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ALL);

                CommonUtils.ShowToast($"보석 {dia} 획득 완료", TOAST_BOX_LENGTH.SHORT);
            }
        }
    }
    /// <summary>
    /// 캐릭터 조각 획득
    /// </summary>
    /// <param name="cheat_key"></param>
    void GainCharacterPiece(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");

        //  using => cpiece character_id count
        if (keys.Length == 3)
        {
            if (int.TryParse(keys[1], out int pc_id) && double.TryParse(keys[2], out double count))
            {
                var pc_data = MasterDataManager.Instance.Get_PlayerCharacterData(pc_id);
                if (pc_data != null)
                {
                    gd.GetUserItemDataManager().AddUserItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, pc_id, count);
                    gd.Save();
                    CommonUtils.ShowToast($"{GameDefine.GetLocalizeString(pc_data.name_id)} 조각을 {count}개 획득 했습니다.", TOAST_BOX_LENGTH.SHORT);
                }
                else
                {
                    CommonUtils.ShowToast("캐릭터 조각을 찾을 수 없습니다.\nUsage => [cpiece] [character_id] [count]", TOAST_BOX_LENGTH.SHORT);
                }
            }
        }
    }

    /// <summary>
    /// 아이템 획득
    /// </summary>
    /// <param name="cheat_key"></param>
    void GainItem(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");
        //  using => item item_id count
        if (keys.Length == 3)
        {
            if (int.TryParse(keys[1], out int item_id) && double.TryParse(keys[2], out double count))
            {
                Item_Data data = MasterDataManager.Instance.Get_ItemData(item_id);
                if (data != null)
                {
                    gd.GetUserItemDataManager().AddUserItemCount(data.item_type, data.item_id, count);
                    gd.Save();

                    CommonUtils.ShowToast($"{GameDefine.GetLocalizeString(data.name_id)}를 {count}개 획득했습니다.", TOAST_BOX_LENGTH.SHORT);
                }
                else
                {
                    CommonUtils.ShowToast("존재하지 않는 아이템 아이디 입니다.\nUsage => [item] [item_id] [count]", TOAST_BOX_LENGTH.SHORT);
                }

            }
        }
    }

    /// <summary>
    /// 근원전달 재화 획득
    /// </summary>
    /// <param name="cheat_key"></param>
    void GainSendEssence(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }

        string key = cheat_key.ToLower();

        string[] keys = key.Split(" ");
        //  using => ess count
        //  using => ess set count
        int total_count = 0;
        switch (keys.Length)
        {
            case 2:
                if (int.TryParse(keys[1], out int count))
                {
                    var charge_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
                    charge_item.AddChargeItem(count);

                    total_count = charge_item.GetCount();
                }
                else
                {
                    CommonUtils.ShowToast($"Usage => [ess] [count]", TOAST_BOX_LENGTH.SHORT);
                    return;
                }
                break;
            case 3:
                if (keys[1].Equals("set") && int.TryParse(keys[2], out int set_count))
                {
                    var charge_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
                    int remain_count = charge_item.GetCount();
                    int modify_count = set_count - remain_count;
                    if (modify_count > 0)
                    {
                        charge_item.AddChargeItem(modify_count);
                    }
                    else if (modify_count < 0)
                    {
                        charge_item.UseChargeItem(-modify_count);
                    }

                    total_count = charge_item.GetCount();
                }
                else
                {
                    CommonUtils.ShowToast($"Usage => [ess] [set] [count]", TOAST_BOX_LENGTH.SHORT);
                    return;
                }
                break;
            default:
                CommonUtils.ShowToast($"Usage => [ess] [count]\nUsage => [ess] [set] [count]", TOAST_BOX_LENGTH.SHORT);
                return;
        }
        GameData.Instance.GetUserChargeItemDataManager().Save();
        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE);

        CommonUtils.ShowToast($"근원전달 재화가 총 {total_count}개가 되었습니다", TOAST_BOX_LENGTH.SHORT);
    }

    /// <summary>
    /// 영웅 획득
    /// </summary>
    /// <param name="cheat_key"></param>
    void GainHero(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }
        var gd = GameData.Instance;
        string key = cheat_key.ToLower();
        //Debug.Log($"{key}");

        string[] keys = key.Split(" ");
        //  using => hero character_id
        string show_msg = string.Empty;
        bool notice_format = true;
        do
        {
            if (keys.Length != 2)
            {
                show_msg += "입력값이 형식에 맞지 않습니다.\n";
                break;
            }

            if (!int.TryParse(keys[1], out int hero_id))
            {
                show_msg += "입력한 캐릭터 ID가 숫자가 아닙니다.\n";
                break;
            }

            try
            {
                var hero_mng = gd.GetUserHeroDataManager();
                var hero = hero_mng.AddUserHeroData(hero_id);
                hero_mng.AddUserHeroSkillData(hero);

                hero_mng.Save();
                GameData.I.GetUserHeroSkillDataManager().Save();
            }
            catch (Exception ex)
            {
                show_msg += $"다음 예외사항이 발생했습니다. {ex.ToString()}";
                notice_format = false;
                break;
            }

        } while (false);

        if (!show_msg.Equals(string.Empty))
        {
            show_msg += notice_format ? "Usage => [hero] [character_id]" : "";
            CommonUtils.ShowToast(show_msg, TOAST_BOX_LENGTH.SHORT);
        }
    }
}
