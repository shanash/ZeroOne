using TMPro;
using UnityEngine;

public class ItemTooltip : TooltipBase
{
    [SerializeField]
    TMP_Text NumberText = null;

    public void Initialize(Rect hole, RewardDataBase reward_data, bool is_screen_modify = true)
    {
        Initialize(hole, reward_data.RewardItemName, reward_data.RewardItemDesc, is_screen_modify);

        double quantity = 0;

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
        
        NumberText.text = string.Format(GameDefine.GetLocalizeString("system_itemgoods_quantity"), quantity.ToString("N0"));
    }
}
