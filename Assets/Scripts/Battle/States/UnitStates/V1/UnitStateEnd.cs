
public class UnitStateEnd : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateEnd()
    {
        TransID = UNIT_STATES.END;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateEndBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateEnd();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateEndExit();
    }
}
