
using System.Diagnostics;

public class Reward_ItemData : RewardDataBase
{
    protected ItemDataBase_V2 Data;

    public override string RewardItemName => (Data != null) ?
        Data.ItemName : "EMPTY";

    public override string RewardItemDesc => (Data != null) ?
        Data.ItemDesc : "EMPTY";

    protected override void InitData()
    {
        var m = MasterDataManager.Instance;
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
            case REWARD_TYPE.BOSS_DUNGEON_TICKET:
                Data = new ItemData_ItemData_V2();
                Data.SetItem(ITEM_TYPE_V2.TICKET_DUNGEON, Reward.var1);
                break;
            case REWARD_TYPE.PIECE_EQUIPMENT:
                Data = new ItemData_EquipmentData_V2();
                Data.SetItem(ITEM_TYPE_V2.PIECE_EQUIPMENT, Reward.var1);
                break;
            case REWARD_TYPE.PIECE_CHARACTER:
                Data = new ItemData_CharacterData_V2();
                Data.SetItem(ITEM_TYPE_V2.PIECE_CHARACTER, Reward.var1);
                break;
            case REWARD_TYPE.ITEM:
                {
                    var item_data = m.Get_ItemData(Reward.var1);
                    if (item_data != null)
                    {
                        Data = new ItemData_ItemData_V2();
                        Data.SetItem(item_data.item_type, item_data.item_id);
                    }
                }
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
