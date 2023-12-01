
public class UnitStateReady : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateReady()
    {
        TransID = UNIT_STATES.READY;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateReadyBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateReady();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateReadyExit();
    }
}
