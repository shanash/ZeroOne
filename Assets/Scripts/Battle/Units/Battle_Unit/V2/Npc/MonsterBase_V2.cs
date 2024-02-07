using UnityEngine;

public class MonsterBase_V2 : HeroBase_V2
{
    public override void SetBattleUnitData(BattleUnitData unit_dt)
    {
        
        Unit_Data = unit_dt;

        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetNpcSkillGroups(Unit_Data.GetSkillPattern());

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

    //protected override void UnitTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag(GameDefine.TAG_MONSTER))
    //    {
    //        var monster = other.gameObject.GetComponent<MonsterBase_V2>();
    //        if (monster != null)
    //        {
    //            if (monster.Deck_Order < Deck_Order)
    //            {
    //                //  change reposition
    //                Is_Reposition = true;
    //            }
    //        }
    //    }
    //}
    //protected override void UnitTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag(GameDefine.TAG_MONSTER))
    //    {
    //        var monster = other.gameObject.GetComponent<MonsterBase_V2>();
    //        if (monster != null)
    //        {
    //            if (monster.Deck_Order < Deck_Order)
    //            {
    //                //  stop reposition
    //                Is_Reposition = false;
    //            }
    //        }
    //    }
    //}

}
