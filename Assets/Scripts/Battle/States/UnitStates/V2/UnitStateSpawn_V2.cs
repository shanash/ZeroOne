
public class UnitStateSpawn_V2 : UnitStateBase_V2
{
    public UnitStateSpawn_V2() : base(UNIT_STATES.SPAWN)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSpawnBegin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSpawn();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSpawnExit();
    }
}
