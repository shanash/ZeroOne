using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterBase_V2 : HeroBase_V2
{
    public override void SetBattleUnitDataID(params int[] unit_ids)
    {
        if (unit_ids.Length < 1)
        {
            return;
        }
        int npc_id = unit_ids[0];
        Unit_Data = new BattleNpcData();
        Unit_Data.SetUnitID(npc_id);

        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetNpcSkillGroups(Unit_Data.GetSkillPattern());
    }


    public BattleUnitData GetNpcData()
    {
        return Unit_Data;
    }

    protected override float GetDistance()
    {
        return Unit_Data.GetDistance();
    }

    protected override float GetApproachDistance()
    {
        return Unit_Data.GetApproachDistance();
    }


    #region Cal Ability Point
    protected override void CalcMaxLife()
    {
        Max_Life = Unit_Data.GetLifePoint();
        Life = Max_Life;
    }

    protected override void CalcAttackPoint()
    {
        Attack = Unit_Data.GetAttackPoint();
    }
    protected override void CalcDefensePoint()
    {
        Defense = Unit_Data.GetDefensePoint();
    }
    protected override void CalcMoveSpeed()
    {
        Move_Speed = Unit_Data.GetMoveSpeed();
    }

    #endregion

    protected override void UnitTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameDefine.TAG_MONSTER))
        {
            var monster = other.gameObject.GetComponent<MonsterBase_V2>();
            if (monster != null)
            {
                if (monster.Deck_Order < Deck_Order)
                {
                    //  change reposition
                    Is_Reposition = true;
                }
            }
        }
    }
    protected override void UnitTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(GameDefine.TAG_MONSTER))
        {
            var monster = other.gameObject.GetComponent<MonsterBase_V2>();
            if (monster != null)
            {
                if (monster.Deck_Order < Deck_Order)
                {
                    //  stop reposition
                    Is_Reposition = false;
                }
            }
        }
    }

}
