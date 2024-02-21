using FluffyDuck.Util;
using UnityEngine;
using UnityEngine.UI;

public class RewardCardBase : UIBase
{
    [SerializeField, Tooltip("Reward Item Icon")]
    protected Image Reward_Item_Icon;

    [SerializeField, Tooltip("Reward Character Icon")]
    protected Image Reward_Character_Icon;

    [SerializeField, Tooltip("Piece Icon")]
    protected Image Piece_Icon;

    protected RewardDataBase Data;
    
    public void SetRewardSetData(Reward_Set_Data d)
    {
        Data = CreateRewardData(d);
        UpdateRewardItemIcon();
    }

    protected void UpdateRewardItemIcon()
    {
        if (Reward_Item_Icon == null)
        {
            return;
        }
        string icon_path = Data.GetRewardItemIconPath();
        if (!string.IsNullOrEmpty(icon_path)) 
        {
            CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.GetRewardItemIconPath(), (spr) =>
            {
                if (Data.GetRewardType() == REWARD_TYPE.CHARACTER || Data.GetRewardType() == REWARD_TYPE.PIECE_CHARACTER)
                {
                    Reward_Item_Icon.gameObject.SetActive(false);
                    Reward_Character_Icon.gameObject.SetActive(true);
                    Reward_Character_Icon.sprite = spr;
                }
                else
                {
                    Reward_Item_Icon.gameObject.SetActive(true);
                    Reward_Item_Icon.sprite = spr;
                    Reward_Character_Icon.gameObject.SetActive(false);
                }
                
            });

            Piece_Icon.gameObject.SetActive(Data.GetRewardType() == REWARD_TYPE.PIECE_CHARACTER || Data.GetRewardType() == REWARD_TYPE.PIECE_EQUIPMENT || Data.GetRewardType() == REWARD_TYPE.PIECE_ITEM);
        }
    }

    RewardDataBase CreateRewardData(Reward_Set_Data data)
    {
        RewardDataBase reward = null;
        switch (data.reward_type)
        {
            case REWARD_TYPE.GOLD:
                reward = new Reward_GoodsData();
                break;
            case REWARD_TYPE.DIA:
                reward = new Reward_GoodsData();
                break;
            case REWARD_TYPE.STAMINA:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.FAVORITE:
                Debug.Assert(false);    //  클라이언트에서 사용하지 않는 보상 타입
                break;
            case REWARD_TYPE.EXP_PLAYER:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.EXP_CHARACTER:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.CHARACTER:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.EQUIPMENT:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.SEND_ESSENCE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.GET_ESSENCE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.BOSS_DUNGEON_TICKET:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.PIECE_EQUIPMENT:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.PIECE_CHARACTER:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.ITEM:
                {
                    var item_data = MasterDataManager.Instance.Get_ItemData(data.var1);
                    if (item_data != null)
                    {
                        reward = new Reward_ItemData();
                    }
                }

                break;
            default:
                Debug.Assert(false);
                break;
        }
        reward?.SetRewardData(data);
        return reward;
    }
}
