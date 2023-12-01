
public class UnitStateSkill01 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateSkill01()
    {
        TransID = UNIT_STATES.SKILL_1;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkill01Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkill01();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkill01Exit();
    }
}
