
public class GameStateEndV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateEndV2()
    {
        TransID = GAME_STATES.END;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateEndBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateEnd();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateEndExit();
    }
}
