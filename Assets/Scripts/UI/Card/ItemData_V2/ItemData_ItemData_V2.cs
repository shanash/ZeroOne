using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 아이템 및 아이템 조각 데이터
/// </summary>
public class ItemData_ItemData_V2 : ItemDataBase_V2
{
    protected Item_Data Data;

    public override string ItemName => (Data != null) ?
        GameDefine.GetLocalizeString(Data.name_id) : string.Empty;

    public override void SetItem(ITEM_TYPE_V2 gtype, int item_id)
    {
        base.SetItem(gtype, item_id);
        Data = MasterDataManager.Instance.Get_ItemData(gtype, item_id);
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
