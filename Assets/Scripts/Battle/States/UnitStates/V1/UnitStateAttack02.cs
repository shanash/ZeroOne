
public class UnitStateAttack02 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateAttack02()
    {
        TransID = UNIT_STATES.ATTACK_2;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttack02Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttack02();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttack02Exit();
    }
}
