
public class UnitStatePause : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStatePause()
    {
        TransID = UNIT_STATES.PAUSE;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStatePauseBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStatePause();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStatePauseExit();
    }
}
