
public class UnitStateTurnEnd : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateTurnEnd()
    {
        TransID = UNIT_STATES.TURN_END;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateTurnEndBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateTurnEnd();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateTurnEndExit();
    }
}
