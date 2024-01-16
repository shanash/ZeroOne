
public class ItemGoods_CharacterPieceData : ItemGoodsDataBase
{
    Player_Character_Data Data;

    public override void SetGoods(GOODS_TYPE gtype, int item_id)
    {
        base.SetGoods(gtype, item_id);
        Data = MasterDataManager.Instance.Get_PlayerCharacterData(item_id);
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
    public override PIECE_TYPE GetPieceType()
    {
        return PIECE_TYPE.CHARACTER;
    }
}
