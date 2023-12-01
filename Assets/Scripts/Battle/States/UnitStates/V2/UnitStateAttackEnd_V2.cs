
public class UnitStateAttackEnd_V2 : UnitStateBase_V2
{
    public UnitStateAttackEnd_V2() : base(UNIT_STATES.ATTACK_END)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackEndBegin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackEnd();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateAttackEndExit();
    }
}
