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


        CalcEvationPoint();
        CalcAccuracyPoint();
        CalcAutoRecoveryLife();
        CalcVampirePoint();

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
        Physics_Attack = Unit_Data.GetAttackDamagePoint();
    }
    /// <summary>
    /// 방어력 계산. 패시브 방어력 등 계산
    /// </summary>
    protected virtual void CalcDefensePoint()
    {
        Physics_Defense = Unit_Data.GetAttackDefensePoint();
    }
    /// <summary>
    /// 치명타 확률
    /// </summary>
    protected virtual void CalcCriticalChance()
    {
        Physics_Critical_Rate = Unit_Data.GetCriticalChance();
    }
    /// <summary>
    /// 치명타 데미지(파워)
    /// </summary>
    protected virtual void CalcCriticalPower()
    {
        Physics_Critical_Power = Unit_Data.GetCriticalPower();
    }

    /// <summary>
    /// 이동속도 계산
    /// </summary>
    protected virtual void CalcMoveSpeed()
    {
        Move_Speed = Unit_Data.GetMoveSpeed();
    }
    /// <summary>
    /// 회피율 계산
    /// </summary>
    protected virtual void CalcEvationPoint()
    {
        Evasion = Unit_Data.GetEvasionPoint();
    }
    /// <summary>
    /// 명중률 계산
    /// </summary>
    protected virtual void CalcAccuracyPoint()
    {
        Accuracy = Unit_Data.GetAccuracyPoint();
    }
    /// <summary>
    /// 한 웨이브 클리어 했으 ㄹ때 회복되는 수치(체력)<br/>
    /// 자동 회복량 = 최대체력 * 자동 회복 값(비율인가 보네)
    /// </summary>
    protected virtual void CalcAutoRecoveryLife()
    {
        Auto_Recovery_Life = Unit_Data.GetAutoRecoveryLife();
    }

    /// <summary>
    /// 체력 흡수(피빨기) 포인트
    /// </summary>
    protected virtual void CalcVampirePoint()
    {
        Vampire_Point = Unit_Data.GetVampirePoint();
    }

}
