
public class UnitStateSpawn : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateSpawn()
    {
        TransID = UNIT_STATES.SPAWN;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSpawnBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSpawn();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSpawnExit();
    }
}
