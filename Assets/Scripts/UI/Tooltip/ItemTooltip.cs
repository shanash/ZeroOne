using TMPro;
using UnityEngine;

public class ItemTooltip : TooltipBase
{
    [SerializeField]
    TMP_Text NumberText = null;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2 || data[0] is not Rect)
        {
            return false;
        }

        RewardDataBase reward = null;

        if (data[1] != null && data[1] is RewardDataBase)
        {
            reward = data[1] as RewardDataBase;
        }

        Initialize((Rect)data[0], reward);

        return true;
    }

    public void Initialize(Rect hole, RewardDataBase reward_data, bool is_screen_modify = true)
    {
        Initialize(
            hole, 
            (reward_data != null) ? reward_data.RewardItemName : "EMPTY",
            (reward_data != null) ? reward_data.RewardItemDesc : "EMPTY",
            is_screen_modify);

        double quantity = 0;

        if (reward_data != null)
        {
            var obj_data = reward_data.GetRewardItemData();
            switch (obj_data)
            {
                case ItemDataBase_V2 item_data:
                    var u_item_data = GameData.I.GetUserItemDataManager().FindUserItem(item_data.Item_Type, item_data.Item_ID);
                    if (u_item_data != null)
                    {
                        quantity = u_item_data.GetCount();
                    }
                    break;
                case GoodsDataBase goods_data:
                    quantity = (int)GameData.I.GetUserGoodsDataManager().GetGoodsCount(goods_data.Goods_Type);
                    break;
            }
        }
        NumberText.text = (reward_data != null) ?
            string.Format(GameDefine.GetLocalizeString("system_itemgoods_quantity"), quantity.ToString("N0")) :
            "EMPTY";
    }
}
