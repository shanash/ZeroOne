using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoodsDataBase : IDisposable
{
    public GOODS_TYPE Goods_Type { get; protected set; } = GOODS_TYPE.NONE;

    public int Item_ID { get; protected set; } = 0;
    public int Item_Count { get; protected set; } = 0;

    protected bool disposed = false;

    public ItemGoodsDataBase()
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
        Goods_Type = GOODS_TYPE.NONE;
        Item_ID = 0;
        Item_Count = 0;
    }

    protected virtual void Destroy() { }

    public virtual void SetGoods(GOODS_TYPE gtype, int item_id)
    {
        Goods_Type = gtype;
        Item_ID = item_id;
    }

    public void AddItemCount(int cnt) { Item_Count += cnt; }

    public virtual string GetItemIconPath() { return string.Empty; }

    public virtual object GetItemData() { return null; }

    public virtual PIECE_TYPE GetPieceType() {  return PIECE_TYPE.NONE; }

    public virtual EQUIPMENT_TYPE GetEquipmentType() { return EQUIPMENT_TYPE.NONE; }
}
