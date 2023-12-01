
public class UnitStateAttackReady02_V2 : UnitStateBase_V2
{
    public UnitStateAttackReady02_V2() : base(UNIT_STATES.ATTACK_READY_2)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackReady02Begin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackReady02();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackReady02Exit();
    }
}
