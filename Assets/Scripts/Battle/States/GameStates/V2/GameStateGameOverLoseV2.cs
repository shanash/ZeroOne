
public class GameStateGameOverLoseV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateGameOverLoseV2()
    {
        TransID = GAME_STATES.GAME_OVER_LOSE;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverLoseBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverLose();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverLoseExit();
    }
}
