
public class UnitStateWaveRun_V2 : UnitStateBase_V2
{
    public UnitStateWaveRun_V2() : base(UNIT_STATES.WAVE_RUN)
    {
    }

    public override void EnterState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateWaveRunBegin();
    }
    public override void UpdateState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateWaveRun();
    }
    public override void ExitState(UnitBase_V2 unit, BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        unit.UnitStateWaveRunExit();
    }
}
