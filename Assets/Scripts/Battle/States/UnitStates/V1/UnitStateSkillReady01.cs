
public class UnitStateSkillReady01 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateSkillReady01()
    {
        TransID = UNIT_STATES.SKILL_READY_1;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillReady01Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillReady01();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillReady01Exit();
    }
}
