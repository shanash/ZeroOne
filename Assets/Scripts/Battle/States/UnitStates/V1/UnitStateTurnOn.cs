
public class UnitStateTurnOn : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateTurnOn()
    {
        TransID = UNIT_STATES.TURN_ON;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateTurnOnBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateTurnOn();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateTurnOnExit();
    }
}
