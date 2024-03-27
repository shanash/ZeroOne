using FluffyDuck.Util;
using System;
using UnityEngine;

public class HeroData : IDisposable, ICloneable// MonoBehaviour, FluffyDuck.Util.MonoFactory.IProduct
{
    public Player_Character_Data Data;

    public Player_Character_Battle_Data Battle_Data;

    public Player_Character_Level_Stat_Data Stat_Data;

    protected bool disposed = false;

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

    protected virtual void Destroy() { }
    public virtual object Clone() { return null; }

    // 초기화할때 필요한 파라미터를 Initialize에 추가해주세요
    bool Initialize()
    {
        return true;
    }

    public HeroData SetUnitID( int hero_id )
    {
        var m = MasterDataManager.Instance;

        Data = m.Get_PlayerCharacterData( hero_id );

        if(Data != null )
        {
            Battle_Data = m.Get_PlayerCharacterBattleData(hero_id, 1);

            Stat_Data = m.Get_PlayerCharacterLevelStatData(hero_id, 1);

            return this;
        }

        return null;

    }

    public int GetUnitID()
    {
        if (Data != null)
        {
            return Data.player_character_id;
        }
        return 0;
    }

    public object GetUnitData()
    {
        return Data;
    }

    public object GetBattleData()
    {
        return Battle_Data;
    }

    public int GetStarGrade()
    {
        return Data.default_star;
    }

    public string GetUnitName()
    {
        if (Data != null)
            return GameDefine.GetLocalizeString(Data.name_id);
        return null;
    }

    public int GetLevel()
    {
        return Data.default_star;
    }

    public ATTRIBUTE_TYPE GetAttributeType()
    {
        if (Data != null)
        {
            return Data.attribute_type;
        }
        return ATTRIBUTE_TYPE.NONE;
    }


}
