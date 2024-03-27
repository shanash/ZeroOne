
public class GameStateGameOverLoseReadyV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateGameOverLoseReadyV2()
    {
        TransID = GAME_STATES.GAME_OVER_LOSE_READY;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverLoseReadyBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverLoseReady();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateGameOverLoseReadyExit();
    }
}
