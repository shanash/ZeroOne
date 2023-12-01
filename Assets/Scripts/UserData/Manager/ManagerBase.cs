using System;


public enum USER_DATA_MANAGER_TYPE
{
    NONE = 0,

    USER_GAME_INFO_DATA_MANAGER,               //  사용자 플레이어 데이터
    USER_HERO_DATA_MANAGER,                 //  사용자 획득 영웅 데이터
    USER_DECK_DATA_MANAGER,                 //  사용자 영웅의 덱 세팅 데이터(각 게임 타입 및 덱의 최대 개수만큼 보유)
}

public class ManagerBase : IDisposable
{
    /// <summary>
    /// 갱신된 데이터가 있을 경우 True로 변환. 
    /// 내부적으로 어떤 데이터가 갱신되었을 경우 사용.
    /// 어쩌면 실시간으로 갱신 정보를 서버와 통신할 수 있기 때문에 사용하지 않을 수도 있음.
    /// </summary>
    protected bool Is_Update_Data;

    protected USER_DATA_MANAGER_TYPE Manager_Type = USER_DATA_MANAGER_TYPE.NONE;

    private bool disposed = false;

    public ManagerBase(USER_DATA_MANAGER_TYPE utype)
    {
        Reset();
        Manager_Type = utype;
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

    protected virtual void Reset()
    {
        Is_Update_Data = false;
    }

    protected virtual void Destroy() { }

    public virtual bool IsUpdateData() { return Is_Update_Data; }
    public virtual void InitUpdateData() { Is_Update_Data = false; }

    public virtual void SetUpdateData(bool is_update) { Is_Update_Data = is_update; }


    public virtual void InitDataManager() { }

    public USER_DATA_MANAGER_TYPE GetManagerType() { return Manager_Type; }

}
