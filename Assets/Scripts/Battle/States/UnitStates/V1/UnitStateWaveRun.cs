
public class UnitStateWaveRun : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStateWaveRun()
    {
        TransID = UNIT_STATES.WAVE_RUN;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateWaveRunBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateWaveRun();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStateWaveRunExit();
    }
}
