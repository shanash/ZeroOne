
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
/// 본 파일은 게임내에 다양하게 사용되는 데이터 타입의 구조체 또는 클래스를 선언하기 위한 파일.
public struct BATTLE_SEND_DATA
{
    public HeroBase_V2 Caster;                      //  시전자
    public List<HeroBase_V2> Targets;               //  대상(타겟)
    public BattleSkillData Skill;                   //  사용 스킬
    public BattleOnetimeSkillData Onetime;          //  일회성 스킬
    public BattleDurationSkillData Duration;        //  지속성 스킬
    public int Effect_Weight_Index;                 //  스킬 효과 비중 인덱스

    public DURATION_EFFECT_TYPE Duration_Effect_Type;

    //public ONETIME_EFFECT_TYPE Onetime_Effect_Type;
    public double Physics_Attack_Point;                   //  물리 공격력
    public double Magic_Attack_Point;                     //  마법 공격력
    public bool Is_Physics_Critical;                      //  물리 치명타 확률
    public bool Is_Magic_Critical;                        //  마법 치명타 확률


    public void Reset()
    {
        Caster = null;
        Skill = null;
        Onetime = null;
        Duration = null;
        Physics_Attack_Point = 0;
        Magic_Attack_Point = 0;
        Is_Physics_Critical = false;
        Is_Magic_Critical = false;
        Effect_Weight_Index = 0;
        //Onetime_Effect_Type = ONETIME_EFFECT_TYPE.NONE;
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
        result.Physics_Attack_Point = Physics_Attack_Point;
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

#region Exp Simulate Data
/// <summary>
/// 스킬/캐릭터 경험치 상승에 사용될 경험치 아이템의 사용 갯수 지정 데이터
/// </summary>
public struct USE_EXP_ITEM_DATA
{
    public ITEM_TYPE_V2 Item_Type;
    public int Item_ID;
    public int Use_Count;
}

/// <summary>
/// 스킬/캐릭터 경험치 아이템 사용 후 시뮬레이션 결과 데이터
/// </summary>
public struct EXP_SIMULATE_RESULT_DATA
{
    public ERROR_CODE Code;         //  레벨업 가능/불가능
    public int Result_Lv;           //  경험치 아이템 사용시 결과 레벨
    public double Result_Accum_Exp;       //  경험치 아이템 사용시 결과 경험치


    public double Add_Exp;          //  경험치 아이템 사용시 증가되는 경험치
    public double Over_Exp;         //  경험치 아이템 사용시 가능한 최대 레벨을 오버할 경우, 오버되는 경험치양
    public double Need_Gold;        //  겅혐치 아이템 사용시 필요 골드

    public EXP_SIMULATE_RESULT_DATA(ERROR_CODE code = default)
    {
        Code = code;
        Result_Lv = 0;
        Result_Accum_Exp = 0;
        Add_Exp = 0;
        Over_Exp = 0;
        Need_Gold = 0;
    }
}
/// <summary>
/// 스킬/캐릭터 경험치 아이템 사용 후 경험치 상승 결과 데이터를 담는다.<br/>
/// 실제 소모된 경험치 아이템 정보만 반환.<br/>
/// 필요한 아이템 보다 더 많은 아이템을 사용할 경우 해당 아이템은 사용되지 않도록 
/// </summary>
public struct USE_EXP_ITEM_RESULT_DATA
{
    public ERROR_CODE Code;

    public int Result_Lv;               //  경험치 아이템 사용 후 결과 레벨
    public double Result_Accum_Exp;     //  경험치 아이템 사용 후 최종 경험치(누적 경험치)

    public double Add_Exp;              //  증가된 경험치
    public double Used_Gold;            //  소모된 골드

    public void ResetAndResultCode(ERROR_CODE code)
    {
        Code = code;
        Result_Lv = 0;
        Result_Accum_Exp = 0;
        Add_Exp = 0;
        Used_Gold = 0;
    }
}
#endregion
