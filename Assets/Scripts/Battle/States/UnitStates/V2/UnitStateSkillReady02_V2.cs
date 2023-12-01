
public class UnitStateSkillReady02_V2 : UnitStateBase_V2
{
    public UnitStateSkillReady02_V2() : base(UNIT_STATES.SKILL_READY_2)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSkillReady02Begin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSkillReady02();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateSkillReady02Exit();
    }
}
