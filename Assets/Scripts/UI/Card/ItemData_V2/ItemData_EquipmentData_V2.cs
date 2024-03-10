
/// <summary>
/// 장비 및 장비 조각 데이터
/// </summary>
public class ItemData_EquipmentData_V2 : ItemDataBase_V2
{
    protected Equipment_Data Data;

    public override string ItemName => (Data != null) ?
    GameDefine.GetLocalizeString(Data.name_id) : string.Empty;

    public override void SetItem(ITEM_TYPE_V2 gtype, int item_id)
    {
        base.SetItem(gtype, item_id);
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
