using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem_NormalItemData : UserItemData
{
    public Item_Data Data { get; private set; }

    protected override void InitMasterData()
    {
        Data = MasterDataManager.Instance.Get_ItemData(Item_Type, Item_ID);
    }

    public override object GetItemData()
    {
        return Data;
    }
}
