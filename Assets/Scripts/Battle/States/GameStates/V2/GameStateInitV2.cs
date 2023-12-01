
public class GameStateInitV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateInitV2()
    {
        TransID = GAME_STATES.INIT;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateInitBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateInit();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateInitExit();
    }
}
