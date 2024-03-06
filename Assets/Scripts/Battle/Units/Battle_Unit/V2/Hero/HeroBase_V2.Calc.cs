public partial class HeroBase_V2 : UnitBase_V2
{
    /// <summary>
    /// 영웅의 능력치를 계산한다. 
    /// 기본 능력 + 레벨에 따른 능력 등 개별 개산
    /// </summary>
    protected void CalcHeroAbility()
    {
        CalcMaxLife();

        //CalcAttackPoint();
        //CalcDefensePoint();
        //CalcMoveSpeed();


        //CalcEvationPoint();
        //CalcAccuracyPoint();
        //CalcAutoRecoveryLife();
        //CalcVampirePoint();

    }
    /// <summary>
    /// 최대 체력 계산
    /// </summary>
    protected virtual void CalcMaxLife()
    {
        Max_Life = Unit_Data.GetMaxLifePoint();

        Life = Max_Life;
    }
  
    
}
