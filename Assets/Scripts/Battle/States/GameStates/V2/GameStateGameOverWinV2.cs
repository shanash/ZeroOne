
public class GameStateGameOverWinV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateGameOverWinV2()
    {
        TransID = GAME_STATES.GAME_OVER_WIN;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverWinBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverWin();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverWinExit();
    }
}
