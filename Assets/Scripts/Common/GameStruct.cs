
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
/// 본 파일은 게임내에 다양하게 사용되는 데이터 타입의 구조체 또는 클래스를 선언하기 위한 파일.
public struct BATTLE_SEND_DATA
{
    public HeroBase_V2 Caster;                      //  시전자
    public List<HeroBase_V2> Targets;               //  대상(타겟)
    public BattleSkillGroup Skill_Group;
    public BattleSkillData Skill;                   //  사용 스킬
    public BattleOnetimeSkillData Onetime;          //  일회성 스킬
    public BattleDurationSkillData Duration;        //  지속성 스킬
    public int Effect_Weight_Index;                 //  스킬 효과 비중 인덱스

    public DURATION_EFFECT_TYPE Duration_Effect_Type;

    public double Physics_Attack_Point;                   //  물리 공격력
    public double Magic_Attack_Point;                     //  마법 공격력
    public bool Is_Physics_Critical;                      //  물리 치명타 확률
    public bool Is_Magic_Critical;                        //  마법 치명타 확률

    public bool Is_Weak;                            //  역상성 공격을 받았을 경우


    public void Reset()
    {
        Caster = null;
        Skill_Group = null;
        Skill = null;
        Onetime = null;
        Duration = null;
        Physics_Attack_Point = 0;
        Magic_Attack_Point = 0;
        Is_Physics_Critical = false;
        Is_Magic_Critical = false;
        Effect_Weight_Index = 0;
        Duration_Effect_Type = DURATION_EFFECT_TYPE.NONE;
        Is_Weak = false;
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

    public HeroBase_V2 GetFirstTarget()
    {
        if (Targets.Count > 0)
        {
            return Targets[0];
        }
        return null;
    }

    public void ClearTargets()
    {
        Targets?.Clear();
    }

    public BATTLE_SEND_DATA Clone()
    {
        BATTLE_SEND_DATA result = new BATTLE_SEND_DATA();
        result.Caster = Caster;
        result.Skill_Group = Skill_Group;
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

/// <summary>
/// 누적 데미지 데이터
/// </summary>
public record Total_Damage_Data
{
    public int Skill_ID;
    public int Max_Effect_Weight_Count;
    public ONETIME_EFFECT_TYPE Onetime_Effect_Type = ONETIME_EFFECT_TYPE.NONE;
    List<double> Damage_List = new List<double>();

    public Total_Damage_Data(int skill_id)
    {
        Skill_ID = skill_id;
        Max_Effect_Weight_Count = 0;
        Damage_List.Clear();
    }
    /// <summary>
    /// 데미지 합산을 위해 더해줌
    /// </summary>
    /// <param name="effect_weight_index"></param>
    /// <param name="damage"></param>
    /// <returns>막타면 true, 막타가 아니면 false</returns>
    public bool AddDamage(int effect_weight_index, double damage)
    {
        if (effect_weight_index < Max_Effect_Weight_Count)
        {
            Damage_List.Add(damage);
            return effect_weight_index == Max_Effect_Weight_Count - 1;
        }
        return false;
    }

    public double GetTotalDamage()
    {
        double sum = Damage_List.Sum();
        return sum;
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

public enum DAMAGE_TYPE
{
    NORMAL,
    WEAK,
    CRITICAL,
    WEAK_CRITICAL,
    TOTAL
}

public struct Effect_Queue_Data
{
    public string Effect_path;
    public UnityEngine.Vector3 Target_Position;
    public object Data;
    public float Duration;
    public UnityEngine.Transform Parent_Transform;
    public DAMAGE_TYPE Damage_Type;
    public bool Need_Move;
    public TEAM_TYPE Target_Team_Type;
}

#region Exp Simulate Data
/// <summary>
/// 아이템의 사용 갯수 지정 데이터
/// </summary>
public record USABLE_ITEM_DATA
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
    public RESPONSE_TYPE Code;         //  레벨업 가능/불가능

    public int Result_Lv;           //  경험치 아이템 사용시 결과 레벨
    public double Result_Accum_Exp;       //  경험치 아이템 사용시 결과 경험치

    public double Add_Exp;          //  경험치 아이템 사용시 증가되는 경험치
    public double Over_Exp;         //  경험치 아이템 사용시 가능한 최대 레벨을 오버할 경우, 오버되는 경험치양
    public double Need_Gold;        //  겅혐치 아이템 사용시 필요 골드

    public List<USABLE_ITEM_DATA> Auto_Item_Data_List;      //  자동 설정시 아이템 리스트

    public EXP_SIMULATE_RESULT_DATA(RESPONSE_TYPE code = default)
    {
        Code = code;
        Result_Lv = 0;
        Result_Accum_Exp = 0;
        Add_Exp = 0;
        Over_Exp = 0;
        Need_Gold = 0;
        Auto_Item_Data_List = new List<USABLE_ITEM_DATA>();
    }

    public void Reset()
    {
        Code = RESPONSE_TYPE.FAILED;
        Result_Lv = 0;
        Result_Accum_Exp = 0;
        Add_Exp = 0;
        Over_Exp = 0;
        Need_Gold = 0;
        if (Auto_Item_Data_List == null)
        {
            Auto_Item_Data_List = new List<USABLE_ITEM_DATA>();
        }
        Auto_Item_Data_List.Clear();
    }
}
/// <summary>
/// 스킬/캐릭터 경험치 아이템 사용 후 경험치 상승 결과 데이터를 담는다.<br/>
/// 실제 소모된 경험치 아이템 정보만 반환.<br/>
/// 필요한 아이템 보다 더 많은 아이템을 사용할 경우 해당 아이템은 사용되지 않도록 
/// </summary>
public struct USE_EXP_ITEM_RESULT_DATA
{
    public RESPONSE_TYPE Code;

    public int Before_Lv;
    public double Before_Accum_Exp;

    public int Result_Lv;               //  경험치 아이템 사용 후 결과 레벨
    public double Result_Accum_Exp;     //  경험치 아이템 사용 후 최종 경험치(누적 경험치)

    public double Add_Exp;              //  증가된 경험치
    public double Used_Gold;            //  소모된 골드

    public void ResetAndResultCode(RESPONSE_TYPE code)
    {
        Code = code;
        Before_Lv = 0;
        Before_Accum_Exp = 0;
        Result_Lv = 0;
        Result_Accum_Exp = 0;
        Add_Exp = 0;
        Used_Gold = 0;
    }
}
#endregion
