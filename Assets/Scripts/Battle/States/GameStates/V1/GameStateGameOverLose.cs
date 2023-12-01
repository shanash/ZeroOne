
public class GameStateGameOverLose : GameState<BattleManager, BattleUIManager>
{
    public GameStateGameOverLose()
    {
        TransID = GAME_STATES.GAME_OVER_LOSE;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateGameOverLoseBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateGameOverLose();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateGameOverLoseExit();
    }
}
