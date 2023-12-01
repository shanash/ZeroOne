
public class GameStateWaveRunV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateWaveRunV2()
    {
        TransID = GAME_STATES.WAVE_RUN;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveRunBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveRun();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveRunExit();
    }
}
