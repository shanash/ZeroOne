
public class UnitStateAttackEnd : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateAttackEnd()
    {
        TransID = UNIT_STATES.ATTACK_END;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackEndBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackEnd();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttackEndExit();
    }
}
