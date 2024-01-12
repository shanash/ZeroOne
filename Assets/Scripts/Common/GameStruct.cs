
using System;
using System.Collections.Generic;
/// 본 파일은 게임내에 다양하게 사용되는 데이터 타입의 구조체 또는 클래스를 선언하기 위한 파일.
public struct BATTLE_SEND_DATA
{
    public HeroBase_V2 Caster;
    public List<HeroBase_V2> Targets;
    public BattleSkillData Skill;
    public BattleOnetimeSkillData Onetime;
    public BattleDurationSkillData Duration;
    public int Effect_Weight_Index;

    public DURATION_EFFECT_TYPE Duration_Effect_Type;

    public double Damage;
    public bool Is_Critical;


    public void Reset()
    {
        Caster = null;
        Skill = null;
        Onetime = null;
        Duration = null;
        Damage = 0;
        Is_Critical = false;
        Effect_Weight_Index = 0;
        Duration_Effect_Type = DURATION_EFFECT_TYPE.NONE;
        ClearTargets();
    }

    public void AddTarget(HeroBase_V2 t)
    {
        if (Targets == null)
        {
            Targets = new List<HeroBase_V2>();
        }
        Targets?.Add(t);
    }

    public void AddTargets(List<HeroBase_V2> targets)
    {
        if (Targets == null)
        {
            Targets = new List<HeroBase_V2>();
        }
        Targets?.AddRange(targets);
    }

    public void ClearTargets()
    {
        Targets?.Clear();
    }

    public BATTLE_SEND_DATA Clone()
    {
        BATTLE_SEND_DATA result = new BATTLE_SEND_DATA();
        result.Caster = Caster;
        result.Skill = Skill;
        result.Onetime = Onetime;
        result.Duration = Duration;
        result.Damage = Damage;
        result.Effect_Weight_Index = Effect_Weight_Index;
        result.Duration_Effect_Type = Duration_Effect_Type;
        if (result.Targets == null)
        {
            result.Targets = new List<HeroBase_V2>();
        }
        if (Targets != null && Targets.Count > 0)
        {
            result.Targets.AddRange(Targets);
        }

        return result;
    }
}


[Serializable]
public struct Start_Projectile_Pos_Data
{
    public CASTER_START_POS_TYPE Pos_Type;
    public UnityEngine.Transform Trans;
}

[Serializable]
public struct Target_Reach_Pos_Data
{
    public TARGET_REACH_POS_TYPE Pos_Type;
    public UnityEngine.Transform Trans;
}

public struct Effect_Queue_Data
{
    public string effect_path;
    public UnityEngine.Transform Target_Position;
    public object Data;
    public float Duration;

}

