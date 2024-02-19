
public class GameStatePlayReadyV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStatePlayReadyV2()
    {
        TransID = GAME_STATES.PLAY_READY;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePlayReadyBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePlayReady();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePlayReadyExit();
    }
}
