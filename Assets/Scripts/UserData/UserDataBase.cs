using System;


public class UserDataBase : IDisposable, ICloneable
{
    /// <summary>
    /// 갱신된 데이터가 있을 경우 True로 변환. 
    /// 내부적으로 어떤 데이터가 갱신되었을 경우 사용.
    /// 어쩌면 실시간으로 갱신 정보를 서버와 통신할 수 있기 때문에 사용하지 않을 수도 있음.
    /// </summary>
    protected bool Is_Update_Data;

    protected bool disposed = false;

    public UserDataBase()
    {
        Reset();
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

    protected virtual void Reset() { }
    protected virtual void Destroy() { }
    protected virtual void InitSecureVars() { }
    protected virtual void InitMasterData() { }

    public virtual bool IsUpdateData() { return Is_Update_Data; }
    public virtual void InitUpdateData() { Is_Update_Data = false; }
    public void SetUpdateData(bool is_update) { Is_Update_Data = is_update; }

    public virtual ERROR_CODE CheckDateAndTimeChange() { return ERROR_CODE.NOT_WORK; }

    public virtual LitJson.JsonData Serialized() { return null; }
    public virtual bool Deserialized(LitJson.JsonData json) { return false; }

    protected int ParseInt(LitJson.JsonData json, string key)
    {
        int ret = 0;
        if (int.TryParse(json[key].ToString(), out ret))
        {
            return ret;
        }
        return ret;
    }
    protected double ParseDouble(LitJson.JsonData json, string key)
    {
        double ret = 0;
        if (double.TryParse(json[key].ToString(), out ret))
        {
            return ret;
        }
        return ret;
    }
    public string ParseString(LitJson.JsonData json, string key)
    {
        return json[key].ToString();
    }
    public bool ParseBool(LitJson.JsonData json, string key)
    {
        bool ret = false;
        if (bool.TryParse(json[key].ToString(), out ret))
        {
            return ret;
        }
        return ret;
    }

    //  데이터 복제
    public virtual object Clone() { return null; }
}
