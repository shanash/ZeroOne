using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCharacterPieceData : ItemDataBase
{
    protected Character_Piece_Data Data;

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
