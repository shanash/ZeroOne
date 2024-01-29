using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem_NormalItemPieceData : UserItemData
{
    Item_Piece_Data Data;

    protected override void InitMasterData()
    {
        Data = MasterDataManager.Instance.Get_ItemPieceData(Item_ID);
    }
    public override object GetItemData()
    {
        return Data;
    }
}
