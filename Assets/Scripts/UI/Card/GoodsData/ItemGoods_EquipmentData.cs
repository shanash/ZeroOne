using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoods_EquipmentData : ItemGoodsDataBase
{
    protected Equipment_Data Data;

    public override void SetGoods(GOODS_TYPE gtype, int item_id)
    {
        base.SetGoods(gtype, item_id);
        Data = MasterDataManager.Instance.Get_EquipmentData(item_id);
        
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

    public override EQUIPMENT_TYPE GetEquipmentType()
    {
        if (Data != null)
        {
            return Data.equipment_type;
        }
        return EQUIPMENT_TYPE.NONE;
    }
}
