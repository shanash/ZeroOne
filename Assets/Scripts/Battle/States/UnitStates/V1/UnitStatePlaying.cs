
public class UnitStatePlaying : UnitState<UnitBase, BattleManager, BattleUIManager>
{
    public UnitStatePlaying()
    {
        TransID = UNIT_STATES.PLAYING;
    }

    public override void EnterState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStatePlayingBegin();
    }
    public override void UpdateState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStatePlaying();
    }
    public override void ExitState(UnitBase unit, BattleManager mng, BattleUIManager ui)
    {
        unit.UnitStatePlayingExit();
    }
}
