using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardCardBase : UIBase
{
    [SerializeField, Tooltip("Reward Item Icon")]
    protected Image Reward_Item_Icon;

    protected RewardDataBase Data;
    
    public void SetRewardSetData(Reward_Set_Data d)
    {
        Data = CreateRewardData(d);
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
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EXP_PLAYER:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EXP_CHARACTER:
                Debug.Assert(false);
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
            case REWARD_TYPE.EXP_POTION_P:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.EXP_POTION_C:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.STA_POTION:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.FAVORITE_ITEM:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.STAGE_SKIP:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.TICKET_DUNGEON:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.EQ_GROWUP:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.TICKET_REWARD_SELECT:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.TICKET_REWARD_RANDOM:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.TICKET_REWARD_ALL:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.PIECE_EQUIPMENT:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.PIECE_CHARACTER:
                reward = new Reward_ItemData();
                break;
            case REWARD_TYPE.EXP_SKILL:
                Debug.Assert(false);
                break;
            default:
                Debug.Assert(false);
                break;
        }
        reward?.SetRewardData(data);
        return reward;
    }
}
