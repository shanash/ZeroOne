using System;

public abstract class RewardDataBase : IDisposable
{
    protected Reward_Set_Data Reward;

    protected bool disposed = false;

    public abstract string RewardItemName { get;  }
    public abstract string RewardItemDesc { get; }
    public int Count => Reward.var2;

    public RewardDataBase()
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
        Reward = null;
    }

    protected virtual void Destroy() { }

    public virtual object GetRewardItemData() { return null; }

    public Reward_Set_Data GetRewardSetData() { return Reward; }

    public virtual void SetRewardData(Reward_Set_Data d)
    {
        Reward = d;
        InitData();
    }

    protected virtual void InitData() { }

    public REWARD_TYPE GetRewardType() { return Reward.reward_type; }

    public virtual string GetRewardItemIconPath() {  return null; }
}
