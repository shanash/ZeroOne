
public class GameStateGameOverWinReadyV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateGameOverWinReadyV2()
    {
        TransID = GAME_STATES.GAME_OVER_WIN_READY;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverWinReadyBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverWinReady();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverWinReadyExit();
    }
}
