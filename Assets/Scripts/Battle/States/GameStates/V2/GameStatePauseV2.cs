
public class GameStatePauseV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStatePauseV2()
    {
        TransID = GAME_STATES.PAUSE;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePauseBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePause();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePauseExit();
    }
}
