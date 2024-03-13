using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageListCell : UIBase
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Stage Number")]
    TMP_Text Stage_Number;

    [SerializeField, Tooltip("Star List")]
    List<Image> Stage_Star_Points;

    [SerializeField, Tooltip("Stage Name")]
    TMP_Text Stage_Name;

    [SerializeField, Tooltip("Entrance Remain Count")]
    TMP_Text Daily_Entrance_Remain_Count;

    [SerializeField, Tooltip("Icon Box")]
    RectTransform Icon_Box;

    [SerializeField, Tooltip("Reward Icon")]
    Image Reward_Icon;
    [SerializeField, Tooltip("Piece Icon")]
    Image Piece_Icon;

    [SerializeField, Tooltip("Entrance Btn")]
    UIButtonBase Entrance_Btn;

    [SerializeField, Tooltip("Lock Cover")]
    RectTransform Lock_Cover;

    Stage_Data Stage;
    Zone_Data Zone;
    World_Data World;
    UserStoryStageData User_Data;

    public void SetStageData(Stage_Data data)
    {
        var m = MasterDataManager.Instance;
        Stage = data;
        Zone = m.Get_ZoneDataByStageGroupID(Stage.stage_group_id);
        World = m.Get_WorldDataByZoneGroupID(Zone.zone_group_id);
        User_Data = GameData.Instance.GetUserStoryStageDataManager().FindUserStoryStageData(Stage.stage_id);
        UpdateStageListCell();
    }

    void UpdateStageListCell()
    {
        //  stage number
        Stage_Number.text = ZString.Format("{0}-{1}", Zone.zone_ordering, Stage.stage_ordering);
        
        //  stage name
        Stage_Name.text = GameDefine.GetLocalizeString(Stage.stage_name_id);

        //  reward item
        if (Zone.zone_difficulty != STAGE_DIFFICULTY_TYPE.NORMAL)
        {
            if (User_Data != null)
            {
                //  남은 횟수 보여주기
                Daily_Entrance_Remain_Count.text = ZString.Format(GameDefine.GetLocalizeString("system_remaining_number"), User_Data.GetDailyEntranceCount(), User_Data.GetMaxDailyEntranceCount());
            }
            else
            {
                //  남은 횟수 보여주기
                Daily_Entrance_Remain_Count.text = ZString.Format(GameDefine.GetLocalizeString("system_remaining_number"), Stage.entrance_limit_count, Stage.entrance_limit_count);
            }

            //  icon
            string icon_path = CreateItemIcon(Stage.reward_type, Stage.reward_id);
            if (!string.IsNullOrEmpty(icon_path))
            {
                Icon_Box.gameObject.SetActive(true);
                CommonUtils.GetResourceFromAddressableAsset<Sprite>(icon_path, (spr) =>
                {
                    Reward_Icon.sprite = spr;
                });
                Piece_Icon.gameObject.SetActive(Stage.reward_type == REWARD_TYPE.PIECE_CHARACTER || Stage.reward_type == REWARD_TYPE.PIECE_ITEM || Stage.reward_type == REWARD_TYPE.PIECE_EQUIPMENT);
            }
            else
            {
                Icon_Box.gameObject.SetActive(false);
            }
        }

        if (User_Data != null)
        {
            //  star point
            MarkStarPoint(User_Data.GetStarPoint());

            //  lock cover
            Lock_Cover.gameObject.SetActive(false);
        }
        else
        {
            //  star point
            MarkStarPoint(0);

            //  lock cover
            Lock_Cover.gameObject.SetActive(true);
        }

    }

    string CreateItemIcon(REWARD_TYPE rtype, int reward_id)
    {
        var m = MasterDataManager.Instance;
        string prefab = string.Empty;
        switch(rtype)
        {
            case REWARD_TYPE.GOLD:
                {
                    var goods = m.Get_GoodsData(GOODS_TYPE.GOLD);
                    if (goods != null)
                    {
                        prefab = goods.icon_path;
                    }
                }
                break;
            case REWARD_TYPE.DIA:
                {
                    var goods = m.Get_GoodsData(GOODS_TYPE.DIA);
                    if (goods != null)
                    {
                        prefab = goods.icon_path;
                    }
                }
                break;
            case REWARD_TYPE.STAMINA:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.FAVORITE:
                {
                    var item = m.Get_ItemData(reward_id);
                    if (item != null)
                    {
                        prefab = item.icon_path;
                    }
                }
                break;
            case REWARD_TYPE.EXP_PLAYER:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EXP_CHARACTER:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.CHARACTER:
                {
                    var character = m.Get_PlayerCharacterData(reward_id);
                    if (character != null)
                    {
                        prefab = character.icon_path;
                    }
                }
                break;
            case REWARD_TYPE.EQUIPMENT:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.BOSS_DUNGEON_TICKET:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.PIECE_EQUIPMENT:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.PIECE_CHARACTER:
                {
                    var character = m.Get_PlayerCharacterData(reward_id);
                    if (character != null)
                    {
                        prefab = character.icon_path;
                    }
                }
                break;
            case REWARD_TYPE.PIECE_ITEM:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.ITEM:
                {
                    var item = m.Get_ItemData(reward_id);
                    if (item != null)
                    {
                        prefab = item.icon_path;
                    }
                }
                break;
            default:
                break;
        }
        return prefab;
    }

    void MarkStarPoint(int pt)
    {
        int cnt = Stage_Star_Points.Count;
        for (int i = 0; i < cnt; i++)
        {
            Stage_Star_Points[i].gameObject.SetActive(false);
        }
        if (pt >= cnt)
        {
            pt = cnt;
        }
        for (int i = 0; i < pt; i++)
        {
            Stage_Star_Points[i].gameObject.SetActive(true);
        }
    }

    public void OnClickStageEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Mission/StageInfoPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(World, Zone, Stage);
        });
    }

    
}
