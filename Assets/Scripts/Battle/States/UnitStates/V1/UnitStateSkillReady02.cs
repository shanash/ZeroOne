
public class UnitStateSkillReady02 : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateSkillReady02()
    {
        TransID = UNIT_STATES.SKILL_READY_2;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillReady02Begin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillReady02();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateSkillReady02Exit();
    }
}
