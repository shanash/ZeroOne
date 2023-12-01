
public class UnitStateAttack01 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateAttack01()
    {
        TransID = UNIT_STATES.ATTACK_1;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttack01Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttack01();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateAttack01Exit();
    }
}
