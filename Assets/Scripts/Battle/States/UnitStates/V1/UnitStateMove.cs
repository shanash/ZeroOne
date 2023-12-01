
public class UnitStateMove : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateMove()
    {
        TransID = UNIT_STATES.MOVE;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateMoveBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateMove();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateMoveExit();
    }
}
