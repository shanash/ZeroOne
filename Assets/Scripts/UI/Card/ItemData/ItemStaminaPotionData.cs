using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStaminaPotionData : ItemDataBase
{
    protected Sta_Potion_Data Data;
    public override void SetItem(ITEM_TYPE itype, int item_id)
    {
        base.SetItem(itype, item_id);
        Data = MasterDataManager.Instance.Get_StaPotionData(Item_ID);
    }
    public override object GetItemData()
    {
        return Data;
    }

    public override string GetItemIconPath()
    {
        if (Data != null)
        {
            return Data.icon_path;
        }
        return string.Empty;
    }
}
