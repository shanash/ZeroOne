
public class UnitStateIdle : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateIdle()
    {
        TransID = UNIT_STATES.IDLE;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateIdleBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateIdle();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateIdleExit();
    }
}
