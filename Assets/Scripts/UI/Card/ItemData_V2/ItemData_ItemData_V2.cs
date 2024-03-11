using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 아이템 및 아이템 조각 데이터
/// </summary>
public class ItemData_ItemData_V2 : ItemDataBase_V2
{
    protected Item_Data Data;

    public override string ItemName
    {
        get
        {
            if (Item_Type == ITEM_TYPE_V2.PIECE_ITEM)
            {
                var item_p_data = MasterDataManager.Instance.Get_ItemPieceData(Item_ID);
                return GameDefine.GetLocalizeString(item_p_data.desc_id);
            }
            else
            {
                if (Data != null) return "EMPTY";
                return GameDefine.GetLocalizeString(Data.name_id);
            }
        }
    }

    public override string ItemDesc => (Data != null) ?
        GameDefine.GetLocalizeString(Data.desc_id) : string.Empty;

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
