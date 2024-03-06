using UnityEngine;

public class MonsterBase_V2 : HeroBase_V2
{
    public override void SetBattleUnitData(BattleUnitData unit_dt)
    {
        
        Unit_Data = unit_dt;
        Unit_Data.SetHeroBase(this);

    }

    public BattleUnitData GetNpcData()
    {
        return Unit_Data;
    }

    public override float GetApproachDistance()
    {
        return Unit_Data.GetApproachDistance();
    }

    #region Cal Ability Point
    protected override void CalcMaxLife()
    {
        Max_Life = Unit_Data.GetMaxLifePoint();
        Life = Max_Life;
    }


    #endregion

}
