using FluffyDuck.Util;
using LitJson;
using System;
using System.Text;
using UnityEngine;

public enum USER_DATA_MANAGER_TYPE
{
    NONE = 0,

    USER_PLAYER_INFO_DATA_MANAGER,               //  사용자 플레이어 데이터
    USER_HERO_DATA_MANAGER,                 //  사용자 획득 영웅 데이터
    USER_DECK_DATA_MANAGER,                 //  사용자 영웅의 덱 세팅 데이터(각 게임 타입 및 덱의 최대 개수만큼 보유)
    USER_MEMORIAL_DATA_MANAGER,
    USER_L2D_DATA_MANAGER,

    USER_STORY_STAGE_DATA_MANAGER,          //  기본 스토리 스테이지 데이터 매니져
    USER_BOSS_DUNGEON_DATA_MANAGER,         //  보스 던전 스테이지 데이터 매니져

    USER_GOODS_DATA_MANAGER,
    USER_ITEM_DATA_MANAGER,
    USER_CHARGE_ITEM_DATA_MANAGER,
    USER_HERO_SKILL_DATA_MANAGER,
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

    protected string GetFilePath()
    {
        string path = $"{Application.persistentDataPath}/{Manager_Type}";
        return path;
    }

    public USER_DATA_MANAGER_TYPE GetManagerType() { return Manager_Type; }

    public virtual LitJson.JsonData Serialized() { return null; }
    public virtual bool Deserialized(LitJson.JsonData json) { return false; }

    public virtual ERROR_CODE CheckDateAndTimeCharge()
    {
        return ERROR_CODE.NOT_WORK;
    }

    public virtual void Save()
    {
        JsonData json = Serialized();
        if (json == null)
        {
            return;
        }

        LitJson.JsonWriter writer = new LitJson.JsonWriter();
        writer.PrettyPrint = true;

        LitJson.JsonMapper.ToJson(json, writer);
        string json_data = writer.TextWriter.ToString();

        FileUtils.SaveFileData(json_data, GetFilePath());
    }

    public virtual bool Load()
    {
        string path = GetFilePath();
        if (!FileUtils.IsExistFile(path))
        {
            return false;
        }
        string json_data = System.IO.File.ReadAllText(path, Encoding.UTF8);
        if (string.IsNullOrEmpty(json_data))
        {
            return false;
        }

        JsonData json = JsonMapper.ToObject(json_data);
        return Deserialized(json);

    }

    #region Json Parse
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
    #endregion
}
