
public class UnitStateAttackReady01_V2 : UnitStateBase_V2
{
    public UnitStateAttackReady01_V2() : base(UNIT_STATES.ATTACK_READY_1)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackReady01Begin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackReady01();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackReady01Exit();
    }
}
