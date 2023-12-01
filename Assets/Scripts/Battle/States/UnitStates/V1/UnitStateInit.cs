
public class UnitStateInit : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateInit()
    {
        TransID = UNIT_STATES.INIT;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateInitBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateInit();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateInitExit();
    }
}
