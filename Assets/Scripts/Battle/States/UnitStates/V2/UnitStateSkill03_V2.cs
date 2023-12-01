
public class UnitStateSkill03_V2 : UnitStateBase_V2
{
    public UnitStateSkill03_V2() : base(UNIT_STATES.SKILL_3)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSkill03Begin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSkill03();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSkill03Exit();
    }
}
