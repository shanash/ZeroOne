
using System.Diagnostics;

public class ItemGoods_ItemPieceData : ItemGoodsDataBase
{
    protected Item_Piece_Data Data;

    public override void SetGoods(GOODS_TYPE gtype, int item_id)
    {
        base.SetGoods(gtype, item_id);
        Data = MasterDataManager.Instance.Get_ItemPieceData(item_id);
        if (Data != null)
        {

        }
    }

    public override object GetItemData()
    {
        return Data;
    }

    public override PIECE_TYPE GetPieceType()
    {
        if (Data != null)
        {
            return Data.piece_type;
        }
        return PIECE_TYPE.NONE;
    }

    public override string GetItemIconPath()
    {
        if (Data != null)
        {
            var m = MasterDataManager.Instance;
            string icon_path = Data.icon_path;
            switch (Data.piece_type)
            {
                case PIECE_TYPE.ITEM:
                    {
                        var item_data = m.Get_ItemData(Data.target_id);
                        icon_path = item_data.icon_path;
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            return icon_path;
        }
        return string.Empty;
    }

}
