
public class UnitStateMoveIn_V2 : UnitStateBase_V2
{
    public UnitStateMoveIn_V2() : base(UNIT_STATES.MOVE_IN)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateMoveInBegin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateMoveIn();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateMoveInExit();
    }
}
