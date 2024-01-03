
public class ItemExpPotionData : ItemDataBase
{
    protected Exp_Potion_Data Data;

    public override void SetItem(ITEM_TYPE itype, int item_id)
    {
        base.SetItem(itype, item_id);
        Data = MasterDataManager.Instance.Get_ExpPotionData(Item_ID);
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
