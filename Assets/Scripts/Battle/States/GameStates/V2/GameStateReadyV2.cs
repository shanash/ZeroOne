
public class GameStateReadyV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateReadyV2()
    {
        TransID = GAME_STATES.READY;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateReadyBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateReady();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateReadyExit();
    }
}
