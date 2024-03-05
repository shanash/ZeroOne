
public class GameStateTimeOutV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateTimeOutV2()
    {
        TransID = GAME_STATES.TIME_OUT;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateTimeOutBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateTimeOut();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateTimeOutExit();
    }
}
