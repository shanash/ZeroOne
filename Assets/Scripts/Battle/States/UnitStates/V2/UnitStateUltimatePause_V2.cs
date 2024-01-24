
public class UnitStateUltimatePause_V2 : UnitStateBase_V2
{
    public UnitStateUltimatePause_V2() : base(UNIT_STATES.ULTIMATE_PAUSE)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateUltimatePauseBegin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateUltimatePause();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateUltimatePauseExit();
    }
}
