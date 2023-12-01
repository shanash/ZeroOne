
public class GameStateGameOverWin : GameState<BattleManager, BattleUIManager>
{
    public GameStateGameOverWin()
    {
        TransID = GAME_STATES.GAME_OVER_WIN;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateGameOverWinBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateGameOverWin();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateGameOverWinExit();
    }
}
