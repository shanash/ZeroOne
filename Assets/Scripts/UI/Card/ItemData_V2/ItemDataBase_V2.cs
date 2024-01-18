using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase_V2 : IDisposable
{
    public ITEM_TYPE_V2 Item_Type { get; protected set; } = ITEM_TYPE_V2.NONE;

    public int Item_ID { get; protected set; } = 0;
    public int Item_Count { get; protected set; } = 0;

    protected bool disposed = false;

    public ItemDataBase_V2()
    {
        Init();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //  관리되는 자원 해제
                Destroy();
            }
            disposed = true;
        }
    }

    protected virtual void Init() 
    {
        Item_Type = ITEM_TYPE_V2.NONE;
        Item_ID = 0;
        Item_Count = 0;
    }

    protected virtual void Destroy() { }

    public virtual void SetItem(ITEM_TYPE_V2 gtype, int item_id)
    {
        Item_Type = gtype;
        Item_ID = item_id;
    }

    public void AddItemCount(int cnt) { Item_Count += cnt; }

    public virtual string GetItemIconPath() { return string.Empty; }

    public virtual object GetItemData() { return null; }

    public virtual PIECE_TYPE GetPieceType() {  return PIECE_TYPE.NONE; }

    public virtual EQUIPMENT_TYPE GetEquipmentType() { return EQUIPMENT_TYPE.NONE; }
}
