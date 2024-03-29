using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultRewardPopup : PopupBase
{
    [Space()]
    [Header("Container")]
    [SerializeField, Tooltip("BG Image")]
    RawImage BG_Image;

    [SerializeField, Tooltip("컨테이너")]
    GameObject Container;

    [SerializeField, Tooltip("등장 애니")]
    Animator Container_Anim;

    [Space()]
    [Header("리워드")]
    [SerializeField, Tooltip("리워드 컨테이너")]
    RectTransform Reward_Container;

    [Space()]
    [Header("Buttons")]
    [SerializeField, Tooltip("홈 버튼 텍스트")]
    TMP_Text Home_Btn_Text;

    [SerializeField, Tooltip("다음 버튼 텍스트")]
    TMP_Text Next_Btn_Text;

    [SerializeField, Tooltip("Anim End Callback")]
    BattleResultAnimEventCallback End_Callback;

    #region Vars

    BattleDungeonData Dungeon;
    Texture BG_Texture;         //  blur 배경 이미지 텍스쳐
    /// <summary>
    /// 보상 획득 정보
    /// </summary>
    List<BATTLE_RESULT_REWARD_DATA> Result_Reward_List = new List<BATTLE_RESULT_REWARD_DATA>();

    List<BattleResultRewardItem> Used_Reward_Item_List = new List<BattleResultRewardItem>();

    #endregion


    protected override bool Initialize(object[] data)
    {
        if (data.Length != 3)
        {
            return false;
        }
        Dungeon = (BattleDungeonData)data[0];
        BG_Texture = (Texture)data[1];
        var reward_list = (List<BATTLE_RESULT_REWARD_DATA>)data[2];
        Result_Reward_List.AddRange(reward_list);

        BG_Image.texture = BG_Texture;

        End_Callback.SetAnimationEndEventCallback(AnimationEndCallback);

        SetEnableEscKeyExit(false);
        FixedUpdatePopup();
        return true;
    }
    void AnimationEndCallback(string evt_name)
    {
        //if (callback_type == BattleResultAnimEventCallback.BATTLE_RESULT_ANIM_CALLBACK_TYPE.REWARD_OPEN)
        //{

        //}
    }
    protected override void FixedUpdatePopup()
    {
        ClearRewardItemList();

        //  container open
        Container.SetActive(true);
        
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < Result_Reward_List.Count; i++)
        {
            var reward = Result_Reward_List[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleResultRewardItem", Reward_Container);
            var item = obj.GetComponent<BattleResultRewardItem>();
            item.InitializeData(reward.Data, RewardItemCallback);
            item.SetCount(reward.Count);
            Used_Reward_Item_List.Add(item);
        }
    }

    void ClearRewardItemList()
    {
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < Used_Reward_Item_List.Count; i++)
        {
            pool.UnusedGameObject(Used_Reward_Item_List[i].gameObject);
        }
        Used_Reward_Item_List.Clear();
    }



    void RewardItemCallback(TOUCH_RESULT_TYPE result, System.Func<bool, Rect> hole, object reward_data_obj)
    {
        if (result == TOUCH_RESULT_TYPE.LONG_PRESS)
        {
            if (reward_data_obj == null || reward_data_obj is not RewardDataBase)
            {
                Debug.LogWarning("표시 가능한 보상정보가 없습니다!");
                return;
            }
            RewardDataBase reward_data = reward_data_obj as RewardDataBase;

            TooltipManager.I.Add("Assets/AssetResources/Prefabs/UI/ItemTooltip", hole(false), reward_data);
        }
    }

    public void OnClickHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var board = BlackBoard.Instance;
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.GAME_TYPE);
        board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);

        SCManager.I.ChangeScene(SceneName.home);

    }
    public void OnClickNext()
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
}
