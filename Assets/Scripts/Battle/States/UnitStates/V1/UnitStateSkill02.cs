
public class UnitStateSkill02 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateSkill02()
    {
        TransID = UNIT_STATES.SKILL_2;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkill02Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkill02();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkill02Exit();
    }
}
