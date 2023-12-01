using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterBase_V2 : HeroBase_V2
{
    protected BattleNpcData Npc;
    

    public void SetBattleNpcData(BattleNpcData npc)
    {
        this.Npc = npc;

        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetNpcSkillGroups(Npc.GetSkillPattern());
    }

    
    public BattleNpcData GetNpcData()
    {
        return Npc;
    }

    protected override float GetDistance()
    {
        return Npc.GetDistance();
    }


    #region Cal Ability Point
    protected override void CalcMaxLife()
    {
        var bdata = Npc.GetNpcBattleData();
        Max_Life = bdata.hp;
        Life = Max_Life;
    }

    protected override void CalcAttackPoint()
    {
        var bdata = Npc.GetNpcBattleData();
        Attack = bdata.attack;
    }
    protected override void CalcDefensePoint()
    {
        var bdata = Npc.GetNpcBattleData();
        Defense = bdata.defend;
    }
    protected override void CalcMoveSpeed()
    {
        var bdata = Npc.GetNpcBattleData();
        Move_Speed = bdata.move_speed;
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
