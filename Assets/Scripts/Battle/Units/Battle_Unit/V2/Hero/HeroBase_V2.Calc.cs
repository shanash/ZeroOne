using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HeroBase_V2 : UnitBase_V2
{
    /// <summary>
    /// 영웅의 능력치를 계산한다. 
    /// 기본 능력 + 레벨에 따른 능력 등 개별 개산
    /// </summary>
    protected void CalcHeroAbility()
    {
        CalcMaxLife();

        CalcAttackPoint();
        CalcDefensePoint();
        CalcMoveSpeed();

    }
    /// <summary>
    /// 최대 체력 계산
    /// </summary>
    protected virtual void CalcMaxLife()
    {
        Max_Life = Unit_Data.GetLifePoint();

        Life = Max_Life;
    }
    /// <summary>
    /// 공격력 계산, 패시브 공격력 등 계산
    /// </summary>
    protected virtual void CalcAttackPoint()
    {
        Attack = Unit_Data.GetAttackPoint();
    }
    /// <summary>
    /// 방어력 계산. 패시브 방어력 등 계산
    /// </summary>
    protected virtual void CalcDefensePoint()
    {
        Defense = Unit_Data.GetDefensePoint();
    }
    /// <summary>
    /// 이동속도 계산
    /// </summary>
    protected virtual void CalcMoveSpeed()
    {
        Move_Speed = Unit_Data.GetMoveSpeed();
    }
}
