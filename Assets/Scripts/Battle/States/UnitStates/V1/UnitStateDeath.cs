
public class UnitStateDeath : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateDeath()
    {
        TransID = UNIT_STATES.DEATH;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateDeathBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateDeath();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateDeathExit();
    }
}
