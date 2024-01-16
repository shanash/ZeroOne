
public class ItemGoods_GoodsData : ItemGoodsDataBase
{
    protected Goods_Data Data;

    public override void SetGoods(GOODS_TYPE gtype, int item_id)
    {
        base.SetGoods(gtype, item_id);
        Data = MasterDataManager.Instance.Get_GoodsTypeData(gtype);
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
