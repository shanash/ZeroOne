using System;
public class ItemDataBase : IDisposable
{
    public ITEM_TYPE Item_Type { get; protected set; } = ITEM_TYPE.NONE;

    public int Item_ID { get; protected set; } = 0;
    public int Item_Count { get; protected set; } = 0;

    protected bool disposed = false;

    public ItemDataBase()
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
        Item_Type = ITEM_TYPE.NONE;
        Item_ID = 0;
        Item_Count = 0;
    }
    protected virtual void Destroy() { }

    public virtual void SetItem(ITEM_TYPE itype, int item_id)
    {
        Item_Type = itype;
        Item_ID = item_id;
    }

    public void AddItemCount(int cnt) { Item_Count += cnt; }

    public virtual string GetItemIconPath() { return string.Empty; }

    public virtual object GetItemData() { return null; }
}
