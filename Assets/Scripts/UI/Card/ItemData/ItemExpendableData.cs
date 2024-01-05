using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExpendableData : ItemDataBase
{
    protected Expendable_Item_Data Data;
    public override void SetItem(ITEM_TYPE itype, int item_id)
    {
        base.SetItem(itype, item_id);
        Data = MasterDataManager.Instance.Get_ExpendableItemData(Item_ID);
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