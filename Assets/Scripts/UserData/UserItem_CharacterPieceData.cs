using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem_CharacterPieceData : UserItemData
{
    Player_Character_Data Data;

    protected override void InitMasterData()
    {
        Data = MasterDataManager.Instance.Get_PlayerCharacterData(Item_ID);
    }

    public override object GetItemData()
    {
        return Data;
    }
}
