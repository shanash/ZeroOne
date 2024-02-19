
public class GameStateMoveInV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateMoveInV2()
    {
        TransID = GAME_STATES.MOVE_IN;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateMoveInBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateMoveIn();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateMoveInExit();
    }
}
