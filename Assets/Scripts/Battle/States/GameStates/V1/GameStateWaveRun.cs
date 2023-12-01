
public class GameStateWaveRun : GameState<BattleManager, BattleUIManager>
{
    public GameStateWaveRun()
    {
        TransID = GAME_STATES.WAVE_RUN;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateWaveRunBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateWaveRun();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateWaveRunExit();
    }
}
