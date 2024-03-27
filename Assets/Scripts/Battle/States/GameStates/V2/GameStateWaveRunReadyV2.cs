
public class GameStateWaveRunReadyV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateWaveRunReadyV2()
    {
        TransID = GAME_STATES.WAVE_RUN_READY;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveRunReadyBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveRunReady();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveRunReadyExit();
    }
}
