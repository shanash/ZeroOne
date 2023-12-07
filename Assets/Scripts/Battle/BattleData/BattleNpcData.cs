using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNpcData : BattleDataBase
{
    Npc_Data Npc;

    Npc_Battle_Data Battle_Data;


    public void SetNpcDataID(int npc_id)
    {
        var m = MasterDataManager.Instance;
        Npc = m.Get_NpcData(npc_id);

        Battle_Data = m.Get_NpcBattleData(Npc.npc_battle_id);
    }



    public Npc_Data GetNpcData() { return Npc; }

    public Npc_Battle_Data GetNpcBattleData() { return Battle_Data; }


    public float GetMoveSpeed()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.move_speed;
        }
        return 0;
    }

    public float GetDistance()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.distance;
        }
        return 0;
    }

    public float GetApproachDistance()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.approach;
        }
        return 0;
    }

    public int[] GetSkillPattern()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.skill_pattern;
        }
        return null;
    }

    public POSITION_TYPE GetPositionType()
    {
        if (Battle_Data != null)
            return Battle_Data.position_type;
        return POSITION_TYPE.NONE;
    }
}
