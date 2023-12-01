
public class UnitStateAttackReady01 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateAttackReady01()
    {
        TransID = UNIT_STATES.ATTACK_READY_1;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackReady01Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackReady01();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackReady01Exit();
    }
}
