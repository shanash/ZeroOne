
using System.Diagnostics;

public class Reward_ItemData : RewardDataBase
{
    protected ItemDataBase_V2 Data;


    protected override void InitData()
    {
        switch (Reward.reward_type)
        {
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
                Data = new ItemData_CharacterData_V2();
                Data.SetItem(ITEM_TYPE_V2.CHARACTER, Reward.var1);
                break;
            case REWARD_TYPE.EQUIPMENT:
                Data = new ItemData_EquipmentData_V2();
                Data.SetItem(ITEM_TYPE_V2.EQUIPMENT, Reward.var1);
                break;
            case REWARD_TYPE.SEND_ESSENCE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.GET_ESSENCE:
                Debug.Assert(false);
                break;
            case REWARD_TYPE.EXP_POTION_P:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.EXP_POTION_P, Reward.var1);
                break;
            case REWARD_TYPE.EXP_POTION_C:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.EXP_POTION_C, Reward.var1);
                break;
            case REWARD_TYPE.STA_POTION:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.STA_POTION, Reward.var1);
                break;
            case REWARD_TYPE.FAVORITE_ITEM:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.FAVORITE_ITEM, Reward.var1);
                break;
            case REWARD_TYPE.STAGE_SKIP:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.STAGE_SKIP, Reward.var1);
                break;
            case REWARD_TYPE.TICKET_DUNGEON:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.TICKET_DUNGEON, Reward.var1);
                break;
            case REWARD_TYPE.EQ_GROWUP:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.EQ_GROWUP, Reward.var1);
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
                Data = new ItemData_EquipmentData_V2();
                Data.SetItem(ITEM_TYPE_V2.PIECE_EQUIPMENT, Reward.var1);
                break;
            case REWARD_TYPE.PIECE_CHARACTER:
                Data = new ItemData_CharacterData_V2();
                Data.SetItem(ITEM_TYPE_V2.PIECE_CHARACTER, Reward.var1);
                break;
            case REWARD_TYPE.EXP_SKILL:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.EXP_SKILL, Reward.var1);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    public override object GetRewardItemData()
    {
        return Data;
    }

    public override string GetRewardItemIconPath()
    {
        if (Data != null)
        {
            return Data.GetItemIconPath();
        }
        return null;
    }
}
