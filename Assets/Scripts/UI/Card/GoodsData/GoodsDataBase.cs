using System;

public class GoodsDataBase : IDisposable
{
    public GOODS_TYPE Goods_Type { get; protected set; } = GOODS_TYPE.NONE;
    public double Goods_Count { get; protected set; } = 0;

    protected Goods_Data Data;


    protected bool disposed = false;

    public GoodsDataBase()
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
        Data = null;
        Goods_Count = 0;
    }

    protected virtual void Destroy() { }

    public virtual void SetGoods(GOODS_TYPE gtype)
    {
        Goods_Type = gtype;
        Data = MasterDataManager.Instance.Get_GoodsData(gtype);
    }

    public void AddGoodsCount(int cnt) { Goods_Count += cnt; }

    public virtual string GetGoodsIconPath() { return Data.icon_path; }

    public Goods_Data GetGoodsData() { return Data; }

}
