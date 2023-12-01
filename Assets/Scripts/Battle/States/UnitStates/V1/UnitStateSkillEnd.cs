
public class UnitStateSkillEnd : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateSkillEnd()
    {
        TransID = UNIT_STATES.SKILL_END;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillEndBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillEnd();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillEndExit();
    }
}
