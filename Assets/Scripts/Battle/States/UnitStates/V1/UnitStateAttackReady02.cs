
public class UnitStateAttackReady02 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateAttackReady02()
    {
        TransID = UNIT_STATES.ATTACK_READY_2;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackReady02Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackReady02();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackReady02Exit();
    }
}
